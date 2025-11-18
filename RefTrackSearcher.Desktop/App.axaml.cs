using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefTrackSearcher.Core.Data;
using RefTrackSearcher.Core.Interfaces.Services;
using RefTrackSearcher.Desktop.Views;
using RefTrackSearcher.Infrastructure.Services;
using RefTrackSearcher.Desktop.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RefTrackSearcher.Core.Config;

namespace RefTrackSearcher.Desktop
{
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                DisableAvaloniaDataAnnotationValidation();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.Configure<JamendoConfig>(configuration.GetSection("Jamendo"));
            services.Configure<CacheConfig>(configuration.GetSection("Cache"));
            
            var JamendoTags = File.ReadAllText("JamendoTags.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var tagsData = JsonSerializer.Deserialize<JamendoTagsRoot>(JamendoTags, options);


            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
#if DEBUG
                builder.AddDebug();
#endif
            });

            services.AddHttpClient();
            
            services.AddSingleton<IJamendoTagsService>(new JamendoTagsService(tagsData));
            services.AddSingleton<IFileCacheService, FileCacheService>();
            services.AddSingleton<IJamendoService, JamendoService>();

            services.AddTransient<MainWindowViewModel>();

        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}