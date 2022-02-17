using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using CommonSnappableTypes;

namespace ExtendableApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("***** Welcome to TypeViewer *****");

            do
            {
                Console.WriteLine("\nWould you like to load a snapin? [Y,N]");

                string answer = Console.ReadLine();

                if (!answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                try
                {
                    LoadSnapin();
                }
                catch
                {
                    Console.WriteLine("Sorry, can't find snapin");
                }
            } while (true);
        }

        static void LoadSnapin()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Filter = "assemblies (*.dll) | *.dll | All files (*.*) | *.*",
                FilterIndex = 2
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("User cancelled out of the open file dialog.");
                return;
            }

            if (dialog.FileName.Contains("CommonSnappableTypes"))
            {
                Console.WriteLine("CommonSnappableTypes has no snap-ins!");
            }
            else if(!LoadExternalModules(dialog.FileName))
            {
                Console.WriteLine("Nothing implements IAppFunctionality!");
            }
        }

        private static bool LoadExternalModules(string path)
        {
            bool foundSnapIn = false;
            Assembly theSnapInAsm = null;

            try
            {
                theSnapInAsm = Assembly.LoadFrom(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred loading the snapin: {ex.Message}");
                return foundSnapIn;
            }

            var theClassTypes = from t in theSnapInAsm.GetTypes()
                                where t.IsClass && (t.GetInterface("IAppFunctionality") != null)
                                select t;

            foreach (Type type in theClassTypes)
            {
                foundSnapIn = true;

                Console.WriteLine();
                IAppFunctionality itfApp = (IAppFunctionality)theSnapInAsm.CreateInstance(type.FullName, true);
                itfApp?.DoIt();

                DisplayCompanyData(type);
            }

            return foundSnapIn;
        }

        private static void DisplayCompanyData(Type t)
        {
            var compInfo = from ci in t.GetCustomAttributes(false)
                           where (ci is CompanyInfoAttribute)
                           select ci;

            foreach (CompanyInfoAttribute c in compInfo)
            {
                Console.WriteLine($"More info about {c.CompanyName} can be found at {c.CompanyUrl}");
            }
        }
    }
}
