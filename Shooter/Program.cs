using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Shooter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Log.log"));
                Trace.AutoFlush = true;
                Trace.Indent();
                Trace.TraceInformation("Entering Main");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
            finally
            {
                Trace.TraceInformation("Exiting Main");
                Trace.Unindent();
            }
        }
    }
}
