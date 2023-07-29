using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trainingCenter
{
    internal static class Program
    {

        public static Process PriorProcess()
        {
            Process edpcenter = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(edpcenter.ProcessName);
            foreach (Process proc in processes)
            {
                if ((proc.Id != edpcenter.Id) && (proc.MainModule.FileName == edpcenter.MainModule.FileName))
                    return proc;
            }
            return null;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (PriorProcess() != null)
            {
                MessageBox.Show(" ! البرنامج مفتوح بالفعل", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}