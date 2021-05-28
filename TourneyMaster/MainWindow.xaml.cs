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
using static TourneyMaster.GenericXAMLHelper;

namespace TourneyMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }


        ///////////////////TournamentDate stuff///////////////////

        private void TournamentDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            HideSearchText(TournamentDate, TournamentDateHint);
        }

        private void TournamentDate_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            EnterDate(TournamentDate, e);
        }
    }
}
