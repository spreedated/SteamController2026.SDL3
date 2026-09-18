using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Demo.Views;
using Serilog;
using Serilog.Events;

namespace Demo
{
    internal static class Program
    {
        internal static StringBuilder LogOutput { get; private set; }
        internal static StringWriter LogWriter { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            LogOutput = new();
            LogWriter = new(LogOutput);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.TextWriter(LogWriter, LogEventLevel.Verbose)
                .CreateLogger();

            Application.Run(new MainWindow());
        }
    }
}