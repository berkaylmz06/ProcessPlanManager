using CEKA_APP.Infrastructure.DependencyInjection;
using CEKA_APP.UsrControl;
using CEKA_APP.UsrControl.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace CEKA_APP
{
    static class Program
    {
        public static IServiceProvider ServiceProvider { get; set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            services.AddApplicationServices();
            services.AddScoped<IUserControlFactory, UserControlFactory>(); 
            ServiceProvider = services.BuildServiceProvider();

            using (var scope = ServiceProvider.CreateScope())
            {
                var form = scope.ServiceProvider.GetRequiredService<frmKullaniciGirisi>();
                Application.Run(form);
            }
        }
    }
}