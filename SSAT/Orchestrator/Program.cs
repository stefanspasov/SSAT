using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestEnvironment.Executors;

namespace Orchestrator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.ThreadException += OnApplicationException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AppException.Handle(e.ExceptionObject as Exception);
        }
        private static void OnApplicationException(object sender, ThreadExceptionEventArgs t)
        {
            AppException.Handle(t.Exception);
        }
    }
}
