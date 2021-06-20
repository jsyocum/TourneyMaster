using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static TourneyMaster.MainWindow;
using static DebugConsole.MainWindow;
using static DebugConsole.DebugConsoleTools;
using static JSYHelpers.CSharp;

namespace TourneyMaster
{
    public class LocalDebug
    {
        private static List<HelpItem> customHelpItems = new List<HelpItem>
        {
            new HelpItem("Title", "Gets the title of the program.", "This is an example of a program-specific HelpItem."),
        };

        public static async Task MonitorConsole()
        {
            while(true)
            {
                if (consoleInput != null)
                {
                    //Put custom commands here
                    switch (consoleInput[0])
                    {
                        case "pingstatuscheck":
                            //pingStatus = true;

                            knownCommand = true;
                            break;

                        case "help":
                            if (consoleInput.Count() == 2)
                            {
                                Help(customHelpItems, consoleInput[1]);
                            }
                            else
                            {
                                Help(customHelpItems);
                            }

                            knownCommand = true;
                            break;

                        case "title":
                            //PopulateDebugConsole("Application title: " + testTitle);
                            break;
                    }

                    if (!knownCommand)
                    {
                        PopulateDebugConsole("\"" + StringArrayToString(consoleInput) + "\" is not a valid command. Type \"Help\" for a list of commands.");
                    }

                    //PopulateDebugConsole("Command detected: " + consoleInput[0]);
                    consoleInput = null;
                }
                else if (!IsWindowOpen<DebugConsole.MainWindow>())
                {
                    goto ConsoleClosed;
                }

                await Task.Delay(1);
            }

            ConsoleClosed:;
        }
    }
}
