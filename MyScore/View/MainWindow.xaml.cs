using MyScore.Model.DB;
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
using GalaSoft.MvvmLight.Messaging;



namespace MyScore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region add
            //try
            //{
            //    using (MyScoreDB dB = new MyScoreDB())
            //    {
            //        Model.Team t = new Model.Team()
            //        {
            //            Name = "Барселона",
            //            Country = "Испания"
            //        };
            //        dB.Teams.Add(t);
            //        Model.Team t2 = new Model.Team()
            //        {
            //            Name = "Реал Мадрид",
            //            Country = "Испания"
            //        };
            //        dB.Teams.Add(t2);
            //        dB.SaveChanges();

            //        Model.Championship ch = new Model.Championship()
            //        {
            //            Name = "Ла Лига",
            //            Region = "Испания"
            //        };
            //        dB.Championships.Add(ch);
            //        Model.Championship ch1 = new Model.Championship()
            //        {
            //            Name = "Бундеслига",
            //            Region = "Германия"
            //        };
            //        dB.Championships.Add(ch1);
            //        dB.SaveChanges();

            //        Model.League l = new Model.League()
            //        {
            //            ChampionshipId = ch.Id,
            //            TeamId = t.Id
            //        };
            //        dB.Leagues.Add(l);

            //        Model.League l2 = new Model.League()
            //        {
            //            ChampionshipId = ch.Id,
            //            TeamId = t2.Id
            //        };
            //        dB.Leagues.Add(l2);
            //        dB.SaveChanges();

            //        Model.Match m = new Model.Match()
            //        {
            //            Date = DateTime.Now,
            //            ChampionshipId = ch.Id,
            //            HomeId = t2.Id,
            //            AwayId = t.Id
            //        };
            //        dB.Matches.Add(m);
            //        dB.SaveChanges();
            //    }
            //}
            //catch { }
            #endregion
            NavigationSetup();
            DataContext = new MainViewModel();
        }
        void NavigationSetup()
        {
            Messenger.Default.Register<NavigateArgs>(this, (x) =>
            {
                MainFrame.Navigate(new Uri(x.Url, UriKind.Relative));
            });
        }
    }
}
