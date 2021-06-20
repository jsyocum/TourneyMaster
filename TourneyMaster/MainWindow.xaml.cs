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
using static TourneyMaster.CSharpHelper;
using static TourneyMaster.XAMLHelper;
using static TourneyMaster.LocalDebug;
using static JSYHelpers.CSharp;
using static JSYHelpers.XAML;
using static DebugConsole.MainWindow;

namespace TourneyMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static string testTitle = Title.Text;

        public MainWindow()
        {
            InitializeComponent();
        }


        ///////////////////TournamentDate stuff///////////////////
        private void TournamentDate_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            RestrictToNumbers(e);
        }

        private async void ConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            var debugConsole = new DebugConsole.MainWindow();
            debugConsole.Show();
            await MonitorConsole();
        }

        private void debugTestButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
