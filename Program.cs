using System;
using System.Windows.Forms;

namespace KumarStudioBillingSoftware
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            DataManager.LoadData();
            Application.Run(new LoginForm());
        }
    }
}
