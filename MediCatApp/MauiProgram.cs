using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Logging;
using MediCatApp.Views;

namespace MediCatApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            { 
                ApiKey = "AIzaSyADmlOzFpnEmFnyeVo0MRIeoBXK3bMvDLM",
                AuthDomain = "medicat-5ca75.firebaseapp.com",
                Providers = new FirebaseAuthProvider[] 
                {
                    new EmailProvider()
                }
            }));


            builder.Services.AddSingleton<StartPage>();
            builder.Services.AddSingleton<StartViewModel>();
            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<ProfilePageViewModel>();

            return builder.Build();
        }
    }
}
