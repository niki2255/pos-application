using System;
using System.Windows.Forms;

namespace SimplePOS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            Db.Init();
            string sifra = Microsoft.VisualBasic.Interaction.InputBox(
    "Unesi admin sifru za puni pristup.\nOstavi prazno za radnicki rezim.",
    "Prijava",
    ""
);

            bool isAdmin = sifra == "555333";
            Application.Run(new MainForm(isAdmin));
        }
    }
}
