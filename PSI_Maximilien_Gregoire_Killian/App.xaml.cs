using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PSI_Maximilien_Gregoire_Killian
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            
            base.OnStartup(e);
            try
            {
                MainWindow window = new MainWindow();
                window.Show();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        
    }
}
