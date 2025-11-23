using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.Common;
using AegisTasks.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainMenu = AegisTasks.UI.Forms.MainMenu;

namespace AegisTasks.UI
{
    internal static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {           
                if (DataAccessBLL.CanConnect())
                {
                    if (!DataAccessBLL.IsAppDatabaseInstalled())
                    {
                        DataAccessBLL.InstallDatabase();

                        MessageBox.Show("Database installed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    run();
                }
                else
                {
                    MessageBox.Show("App database not found. AegisTask terminated", "Database not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex) {
                Logger.LogException(ex);
                MessageBox.Show("App launch error. AegisTask terminated", "Launch error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void run()
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                    //HACK LOGIN RÁPIDO PARA DEBUG. Comentar el login de debajo
                    //SessionManager.Login("admin", "Password123!");
                //Mostrar el login
                using (Login loginForm = new Login())
                {
                    loginForm.ShowDialog();

                    // Si no se logueó correctamente, salir de la aplicación
                    if (!SessionManager.IsLoggedIn)
                    {
                        keepRunning = false;
                        break;
                    }
                }

                // Si llegamos aquí, el login fue exitoso
                if (SessionManager.IsLoggedIn)
                    {
                        using (MainMenu mainMenu = new MainMenu())
                        {
                            bool logout = false;

                            // Suscribirse al evento de logout
                            mainMenu.Logout += (s, e) =>
                            {
                                logout = true;
                                mainMenu.Close();
                            };

                            // Mostrar el menú principal
                            Application.Run(mainMenu);

                            // Si NO fue logout (se cerró directamente), salir de la app
                            if (!logout)
                            {
                                keepRunning = false;
                            }
                            // Si fue logout, el bucle continúa y vuelve al login
                        }
                    }
  

            }
        }
    }
}
