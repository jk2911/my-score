using MyScore.Model;
using MyScore.Model.DB;
using MyScore.View;
using MyScore.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyScore.ViewModel
{
    public class ChampionshipsListViewModel : NavigateViewModel
    {
        public string Email { get; set; }
        public ObservableCollection<ChampionshipsViewModel> ChampionshipsList { get; set; }
        public ObservableCollection<TeamsViewModel> TeamsList { get; set; }
        public ObservableCollection<TeamsViewModel> MyTeamsList { get; set; }
        public Visibility Visibility { get; set; } = Visibility.Collapsed;
        public Visibility VisibilityUser { get; set; } = Visibility.Collapsed;
        public Visibility VisibilityReg { get; set; } = Visibility.Visible;
        public Visibility VisibilityExit { get; set; } = Visibility.Collapsed;

        public ICommand AddChampionshipCommand
        {
            get
            {
                return new DelegateCommand(AddChampionship);
            }
        }
        public void AddChampionship()
        {
            Navigate("View/AddNewChampionshipPage.xaml");
        }
        public ICommand AddTeamCommand
        {
            get
            {
                return new DelegateCommand(AddTeam);
            }
        }
        public void AddTeam()
        {
            Navigate("View/AddNewTeamPage.xaml");
        }

        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand(Login);
            }
        }
        public void Login()
        {
            Navigate("View/LoginPage.xaml");
        }
        public ICommand RegCommand
        {
            get
            {
                return new DelegateCommand(Reg);
            }
        }
        public void Reg()
        {
            Navigate("View/RegPage.xaml");
        }
        public ICommand ExitCommand
        {
            get
            {
                return new DelegateCommand(Exit);
            }
        }
        public void Exit()
        {
            CurrentObject.Admin = null;
            App.Current.MainWindow.Hide();
            App.Current.MainWindow = new MainWindow();
            App.Current.MainWindow.Show();
        }


        #region Конструктор

        public ChampionshipsListViewModel()
        {
            AddChampionships();
            Email = CurrentObject.Email;
        }

        #endregion

        public void AddChampionships()
        {
            ChampionshipsList = new ObservableCollection<ChampionshipsViewModel>();
            TeamsList = new ObservableCollection<TeamsViewModel>();
            MyTeamsList = new ObservableCollection<TeamsViewModel>();
            using (MyScoreDB db = new MyScoreDB())
            {
                var championships = db.Championships.OrderBy(p => p.Region);
                var selected = db.SelectedTeams.Where(p => p.UserId == CurrentObject.UserId);
                foreach (var i in championships)
                    ChampionshipsList.Add(new ChampionshipsViewModel(i));
                if (CurrentObject.Admin == true)
                {
                    var teams = db.Teams.OrderBy(p => p.Name);
                    foreach (var i in teams)
                        TeamsList.Add(new TeamsViewModel(i));
                }
                foreach(var i in selected)
                {
                    MyTeamsList.Add(new TeamsViewModel(db.Teams.FirstOrDefault(p => p.Id == i.TeamId)));
                }
            }
            if (CurrentObject.Admin != null)
            {
                VisibilityExit = Visibility.Visible;
                VisibilityReg = Visibility.Collapsed;
                if (CurrentObject.Admin == false)
                {
                    VisibilityUser = Visibility.Visible;
                }
                else
                    Visibility = Visibility.Visible;
            }
        }
    }
}

