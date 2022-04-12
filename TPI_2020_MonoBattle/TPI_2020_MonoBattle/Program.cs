/*
 * Author       : Nelson Jeanrenaud
 * 
 * Teacher      : Stéphane Garchery
 * 
 * Experts      : Pierre Conrad, Philippe Bernard
 * 
 * Date         : 06.06.2020
 * 
 * File         : Program.cs
 */
using System;
using System.Windows.Forms;

namespace TPI_2020_MonoBattle
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
             Application.EnableVisualStyles();
             Application.SetCompatibleTextRenderingDefault(false);
             frmConnection menuForm = new frmConnection();
             Application.Run(menuForm);
             if (menuForm.NeedlaunchApplication)
             {
                 GameManager game = new GameManager();
                 game.Run();
             }
            /*GameManager game = new GameManager();
            game.Run();*/
        }
    }
#endif
}
