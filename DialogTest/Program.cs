using System;
using System.Windows.Forms;

namespace DialogTest
{
    internal static class Program
    {
        public static MainUI UI;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UI=new MainUI();
            Application.Run(UI);
        }
    }
}
