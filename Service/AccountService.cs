using AutoMapper;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto;
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
        public AccountService(IUserRepository userRepository, IMapper mapper, SendInBlueEmailNotificationService sendInBlueEmailNotificationService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.sendInBlueEmailNotificationService = sendInBlueEmailNotificationService;
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

                var subject = $"Hello {user.FirstName}!";
                var verfificationLink = $"https://localhost:7041/Account/VerifyAccount?accountVerificationCode={user.AccountVerificationCode}";
                var htmlContent = $"<p>Hello {user.FirstName}, please confirm your account by clicking on given link</p> <a href={verfificationLink}>Click here</a>";

                sendInBlueEmailNotificationService.SendEmail(user.EmailId, user.FirstName, subject, htmlContent);

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

                var subject = $"Hello {user.FirstName}!";
                var htmlContent = $"<p>Hello {user.FirstName}, this is a test email from Brevo.</p>";

                sendInBlueEmailNotificationService.SendEmail(user.EmailId, user.FirstName, subject, htmlContent);

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

            return mapper.Map<UserResponse>(user);
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