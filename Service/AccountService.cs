using AutoMapper;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto;
using Service.Dto.Request;
using Service.Dto.Request.Admin;
using Service.Dto.Response;
using Service.Helper;
using Service.Interface;
using static Entities.Constants;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly SendInBlueEmailNotificationService sendInBlueEmailNotificationService;
        private readonly IUserPasswordResetRequestRepository userPasswordResetRequestRepository;
        public AccountService(IUserRepository userRepository, IMapper mapper, SendInBlueEmailNotificationService sendInBlueEmailNotificationService, IUserPasswordResetRequestRepository userPasswordResetRequestRepository)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.sendInBlueEmailNotificationService = sendInBlueEmailNotificationService;
            this.userPasswordResetRequestRepository = userPasswordResetRequestRepository;
        }

        public async Task<ApiResponse> Create(SignUpRequest dto)
        {
            try
            {
                var userExists = await userRepository.GetByEmailId(dto.EmailId);
                if (userExists is not null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.EMAIL_EXISTS, null);
                }

                var userByUserName = await userRepository.GetByUserName(dto.UserName);
                if (userByUserName is not null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.USER_NAME_ALREADY_EXISTS, null);
                }

                if (!IsDateOfBirthValid(dto.DateOfBirth))
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.INVALID_DATE_OF_BIRTH, null);
                }

                var user = new Users(dto.FirstName, dto.LastName, dto.EmailId, HashPassword(dto.PassWord), dto.DateOfBirth, dto.UserName, dto.CountryId, dto.StateId, dto.CityId, dto.ProfilePicUrl);

                SendEmailVerificationEmail(user);

                userRepository.Add(user);

                await userRepository.SaveAsync();

                return new ApiResponse(user, StatusCodes.Status200OK, Account.USER_REGISTERED_SUCCESSFULLY, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetUserByLoginCredential(LoginRequest dto)
        {
            try
            {
                var user = await userRepository.GetByUserNameOrEmailId(dto.EmailIdOrUserName);
                if (user is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.INVALID_CREDENTIAL, null);
                }

                if (user.FailedLoginAttempts >= 5 && (DateTime.UtcNow - user.AccountBlockedDueToWrongCredentialDate.GetValueOrDefault()).Hours < 24)
                {
                    if (user.FailedLoginAttempts >= 5)
                    {
                        var subject = $"Hello {user.FirstName}!";
                        var htmlContent = $"<p>Hello {user.FirstName}, your account is blocked due 5 failed login attempted contact support desk now to unblock your account <br> or try again after 24 hour";

                        sendInBlueEmailNotificationService.SendEmail(user.EmailId, user.FirstName, subject, htmlContent);
                    }
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.FAILED_LOGIN_ATTEMPT_LIMIT, null);
                }

                if (!BCrypt.Net.BCrypt.Verify(dto.PassWord, user.HashedPassword))
                {
                    user.ChangeFailedLoginAttempts();
                    await userRepository.SaveAsync();
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.INVALID_CREDENTIAL, null);
                }

                if (user.FailedLoginAttempts > 0)
                {
                    user.UpdateFailedLoginAttemptAndDate();
                    await userRepository.SaveAsync();
                }

                return new ApiResponse(user, StatusCodes.Status200OK, Account.LOGGED_IN_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<UserResponse?> GetUserProfile(string emailId)
        {
            var user = await userRepository.GetByEmailId(emailId);
            if (user is null)
            {
                return null;
            }

            var userResponse = mapper.Map<UserResponse>(user);

            if (user.SellerRequest is not null)
            {
                userResponse.HasRequestForSeller = true;
            }

            return userResponse;
        }

        public async Task<Country?> GetCountry(long id)
        {
            return await userRepository.GetCountry(id);
        }

        public async Task<ApiResponse> GetUserList(GetUserListRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                if (dto.Requestor.Role.Name != Keys.ADMIN)
                {
                    return new ApiResponse(null, StatusCodes.Status403Forbidden, Keys.FORBIDDEN, null);
                }

                if (dto.PageNo <= 0 || dto.PageSize <= 0)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.INVALID_PAGINATION, null);
                }

                var (users, totalCount) = await userRepository.UsersList(dto.PageNo, dto.PageSize, dto.SearchTerm, dto.Requestor.Id);

                return new ApiResponse(users.Select(mapper.Map<UserResponse>).ToList(), StatusCodes.Status200OK, Keys.USERS, new PagedData(dto.PageNo, dto.PageSize, totalCount, (int)Math.Ceiling((double)totalCount / dto.PageSize)));
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetUserProfile(UserResponse? requestor, string emailId)
        {
            if (requestor is null)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
            }

            if (requestor.Role.Name != Keys.ADMIN)
            {
                return new ApiResponse(null, StatusCodes.Status403Forbidden, Keys.FORBIDDEN, null);
            }

            var user = await userRepository.GetByEmailId(emailId);
            if (user is null)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.USER_DOES_NOT_EXISTS, null);
            }

            return new ApiResponse(mapper.Map<UserResponse2>(user), StatusCodes.Status200OK, Keys.USERS, null);
        }

        public async Task<ApiResponse> VerifyAccount(string verificationCode)
        {
            var user = await userRepository.GetUserByVerificationCode(verificationCode);
            if (user is null)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.INVALID_ACCOUNT_VERIFICATION_LINK, null);
            }

            if (user.IsEmailVerified)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.ACCOUNT_ALREADY_VERIFIED, null);
            }

            user.VerifyAccount();

            await userRepository.SaveAsync();

            return new ApiResponse(user, StatusCodes.Status200OK, Account.ACCOUNT_VERIFICATION_SUCESSFULLY, null);
        }

        public async Task<ApiResponse> ResendAccountVerificationEmail(UserResponse requestor)
        {
            var user = await userRepository.GetByEmailId(requestor.EmailId);
            if (user is null)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
            }

            if (user.IsEmailVerified)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.ACCOUNT_ALREADY_VERIFIED, null);
            }

            if ((DateTime.UtcNow - user.AccountVerificationCodeSentAt.GetValueOrDefault()).Days < 1 && user.AccountVerificationResendCount > 5)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.RESENT_EMAIL_VERIFICATION_LIMIT, null);
            }

            user.ResentVerifyEmail();

            SendEmailVerificationEmail(user);

            await userRepository.SaveAsync();

            return new ApiResponse(user, StatusCodes.Status200OK, Account.EMAIL_VERIFICATION_SENT_SUCESSFULLY, null);
        }

        public async Task<ApiResponse> ResetPassword(string emailIdOrUserName)
        {
            try
            {
                var user = await userRepository.GetByUserNameOrEmailId(emailIdOrUserName);
                if (user is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                var (userPasswordResetRequests, totalCountOfRequestInDay) = await userPasswordResetRequestRepository.GetByUserId(user.Id);
                if (userPasswordResetRequests.Any() && totalCountOfRequestInDay >= 3)
                {
                    return new ApiResponse(user, StatusCodes.Status400BadRequest, Account.PASSWORD_RESET_EMAIL_LIMIT, null);
                }

                var userPasswordResetRequest = new UserPasswordResetRequest(user.Id);

                userPasswordResetRequestRepository.Add(userPasswordResetRequest);

                await userPasswordResetRequestRepository.SaveAsync();

                SendPasswordResetEmail(user, userPasswordResetRequest.ResetToken);

                return new ApiResponse(user, StatusCodes.Status200OK, Account.EMAIL_SENT_TO_RESET_PASSWORD, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> CheckTokenForResetPassword(string resetToken)
        {
            var userPasswordResetRequest = await userPasswordResetRequestRepository.GetByToken(resetToken);
            if (userPasswordResetRequest is null)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.PASSWORD_RESET_LINK_DOES_NOT_EXISTS, null);
            }

            return new ApiResponse(Base64Encode(userPasswordResetRequest.UserId.ToString()), StatusCodes.Status200OK, Keys.SUCESS, null);
        }

        public async Task<ApiResponse> UpdateUserPassword(UpdatePasswordRequest dto)
        {
            try
            {
                var user = await userRepository.GetById(Convert.ToInt64(Base64Decode(dto.Requestor)));
                if (user is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                var userPasswordResetRequest = await userPasswordResetRequestRepository.GetByToken(dto.ResetToken);
                if (userPasswordResetRequest is null)
                {
                    return new ApiResponse(Base64Encode(userPasswordResetRequest.UserId.ToString()), StatusCodes.Status400BadRequest, Account.PASSWORD_RESET_LINK_DOES_NOT_EXISTS, null);
                }

                user.UpdatePassword(HashPassword(dto.PassWord));

                userPasswordResetRequest.LinkUsed();

                await userRepository.SaveAsync();

                PasswordUpdatedEmail(user);

                return new ApiResponse(null, StatusCodes.Status200OK, Account.PASSWORD_UPDATED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> UpdateProfile(UpdateProfileRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                if (dto.EmailId != dto.Requestor.EmailId)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.FORBIDDEN, null);
                }

                var user = await userRepository.GetByEmailId(dto.EmailId);
                if (user is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                if (!user.IsEmailVerified)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.VERIFY_EMAIL_ID, null);
                }

                if (!string.IsNullOrWhiteSpace(user.ProfilePicUrl) && dto.ProfilePicUrl is null)
                {
                    dto.ProfilePicUrl = user.ProfilePicUrl;
                }

                user.Update(dto.FirstName, dto.LastName, dto.UserName, dto.ProfilePicUrl);

                userRepository.Update(user);

                await userRepository.SaveAsync();

                SendProfileUpdatedEmail(user);

                return new ApiResponse(user, StatusCodes.Status200OK, Account.PROFILE_UPDATED_SUCESSFULLY, null);

            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        private void PasswordUpdatedEmail(Users user)
        {
            var subject = $"Hello {user.FirstName}!";
            var htmlContent = $"<p>Hello {user.FirstName}, you have sucessfully changed your password";

            sendInBlueEmailNotificationService.SendEmail(user.EmailId, user.FirstName, subject, htmlContent);
        }

        private void SendPasswordResetEmail(Users user, string resetToken)
        {
            var subject = $"Hello {user.FirstName}!";
            var verfificationLink = $"https://localhost:7041/Account/ResetPassword?resetToken={resetToken}";
            var htmlContent = $"<p>Hello {user.FirstName}, please change your password by clicking on given link </p> <a href={verfificationLink}>Click here</a>";

            sendInBlueEmailNotificationService.SendEmail(user.EmailId, user.FirstName, subject, htmlContent);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void SendEmailVerificationEmail(Users user)
        {
            var subject = $"Hello {user.FirstName}!";
            var verfificationLink = $"https://localhost:7041/Account/VerifyAccount?accountVerificationCode={user.AccountVerificationCode}";
            var htmlContent = $"<p>Hello {user.FirstName}, please confirm your account by clicking on given link</p> <a href={verfificationLink}>Click here</a>";

            sendInBlueEmailNotificationService.SendEmail(user.EmailId, user.FirstName, subject, htmlContent);
        }

        private void SendProfileUpdatedEmail(Users user)
        {
            var subject = $"Hello {user.FirstName}!";
            var htmlContent = $"<p>Hello {user.FirstName}, your profile is updated sucessfully";

            sendInBlueEmailNotificationService.SendEmail(user.EmailId, user.FirstName, subject, htmlContent);
        }

        private bool IsDateOfBirthValid(DateTime dateOfBrith)
        {
            bool result = true;
            if (dateOfBrith >= DateTime.UtcNow || DateTime.UtcNow.Year - dateOfBrith.Year < 12)
            {
                result = false;
            }

            return result;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}