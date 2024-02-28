using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Windows.Forms;

namespace filesigner2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
//            CheckIfAlreadyRun();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void CheckIfAlreadyRun()
        {
            try
            {
                var np = new NamedPipeClientStream("FileSignerPipe");
                np.Connect(100);
                StreamWriter wr = new StreamWriter(np);
                wr.WriteLine(Environment.GetCommandLineArgs()[1]);
                wr.Close();
                np.Close();
                Environment.Exit(0); 
            } catch (Exception e)
            {

            }
        }
    }
}
