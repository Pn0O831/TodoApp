using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using TodoApp.Client.Services;
using TodoApp.Client.ViewModels;

namespace TodoApp.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var todoApi = RestService.For<ITodoApi>(
                new HttpClient { BaseAddress = new Uri("http://localhost:5000/api") },
                new RefitSettings
                {
                    ContentSerializer = new SystemTextJsonContentSerializer(
                     new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    )
                }
            );

            var mainVm = new MainViewModel(todoApi);

            var window = new Views.MainWindow(mainVm);
            window.Show();
        }
    }
}
