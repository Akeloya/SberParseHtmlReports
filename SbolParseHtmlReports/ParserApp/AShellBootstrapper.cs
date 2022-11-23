using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace ParserApp
{
    public class ShellBootstrapper<TModel> : Bootstrapper
    {

        public ShellBootstrapper()
        {

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            StartApplication();
        }

        public void StartApplication()
        {
            var inst = GetInstance(typeof(TModel), "");
            var manager = IoC.Get<IWindowManager>();
            dynamic settings = new ExpandoObject();
            settings.Height = 500;
            settings.Width = 800;
            settings.SizeToContent = SizeToContent.Manual;
            manager.ShowWindowAsync(inst,settings: settings);
        }
    }
}
