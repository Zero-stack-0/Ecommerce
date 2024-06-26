using Data.Repository;
using Data.Repository.Interface;
using Service;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.DependencyInjection
{
    public class DependencyInjection
    {
        public DependencyInjection() { }
        public static void InjectDependencies(IServiceCollection services)
        {
            //repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserPasswordResetRequestRepository, UserPasswordResetRequestRepository>();

            //services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<CookieUserDetailsHandler>();
            services.AddAutoMapper(typeof(Service.Helper.Mapper).Assembly);
            services.AddSingleton<SendInBlueEmailNotificationService>();
        }
    }
}