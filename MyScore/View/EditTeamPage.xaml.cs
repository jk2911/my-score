using MyScore.ViewModel;
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

namespace MyScore.View
{
    /// <summary>
    /// Логика взаимодействия для EditTeamViewModel.xaml
    /// </summary>
    public partial class EditTeamPage : Page
    {
        public EditTeamPage()
        {
            InitializeComponent();
            DataContext = new EditTeamViewModel();
        }
    }
}
