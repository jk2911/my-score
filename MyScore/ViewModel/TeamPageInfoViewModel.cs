using MyScore.Logic;
using MyScore.Model;
using MyScore.Model.DB;
using MyScore.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MyScore.ViewModel
{
    public class TeamPageInfoViewModel : NavigateViewModel
    {
        public Team team;
        //public bool myteam = false;
        public ObservableCollection<TeamInLeagueViewModel> LeagueTeams { get; set; }
        public ObservableCollection<MatchViewModel> LeagueMatchCalender { get; set; }
        public ObservableCollection<MatchViewModel> LeagueMatchResults { get; set; }
        public string Team => team.Name; 
        public string MyTeam { get; set; } = "Добавить в мои команды";
        public ImageSource ImageSrc
        {
            get { return SaveAndLoadPicture.LoadImage(team.Image); }
        }
        public Visibility Visibility { get; set; } = Visibility.Visible;
        public Visibility VisibilityUser { get; set; } = Visibility.Collapsed;

        #region Player
        public int number = 1;
        public string Number
        {
            get => number.ToString();
            set
            {
                try
                {
                    number = Convert.ToInt32(value);
                    if (number == 0)
                        number = 1;
                    OnPropertyChanged("Number");
                }
                catch { return; }
            }
        }
        //public string name;
        public string Name { get; set; } = "";
        public string Country { get; set; } = "";
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public string Position { get; set; }

        #endregion
        #region Command
        public ICommand HomePageCommand
        {
            get
            {
                return new DelegateCommand(HomePage);
            }
        }
        public void HomePage()
        {
            Navigate("View/AllChampionshipPage.xaml");
        }
        public ICommand EditTeamCommand
        {
            get => new DelegateCommand(EditTeam);
        }
        public void EditTeam()
        {
            Navigate("View/EditTeamPage.xaml");
        }
        public ICommand RemoveTeamCommand
        {
            get => new DelegateCommand(RemoveTeam);
        }
        public void RemoveTeam()
        {
            var message = MessageBox.Show("Вы точно хотите удалить команду?", Team, MessageBoxButton.OKCancel);
            if (message == MessageBoxResult.Cancel)
                return;
            ResultsMatch.DeleteTeam(team.Id);
            Navigate("View/AllChampionshipPage.xaml");
        }
        public ICommand EditMyCommand
        {
            get => new DelegateCommand(EditMy);
        }
        public void EditMy()
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                var count = db.SelectedTeams.Count(p => p.TeamId == team.Id && p.UserId == CurrentObject.UserId);
                if (count > 0)
                {
                    MyTeam = "Добавить в мои команды";
                    OnPropertyChanged("MyTeam");
                    var selected = db.SelectedTeams.FirstOrDefault(p => p.UserId == CurrentObject.UserId && p.TeamId == team.Id);
                    db.SelectedTeams.Remove(selected);
                    db.SaveChanges();
                }
                else
                {
                    MyTeam = "Убрать из моих команд";
                    OnPropertyChanged("MyTeam");
                    SelectedTeam selected = new SelectedTeam() { TeamId = team.Id, UserId = CurrentObject.UserId };
                    db.SelectedTeams.Add(selected);
                    db.SaveChanges();
                }
            }
        }
        #endregion
        public TeamPageInfoViewModel()
        {
            var now = DateTime.Now;
            DateStart = now.AddYears(-50);
            DateEnd = now.AddYears(-15);
            Date = now.AddYears(-25);

            LeagueTeams = new ObservableCollection<TeamInLeagueViewModel>();
            LeagueMatchCalender = new ObservableCollection<MatchViewModel>();
            LeagueMatchResults = new ObservableCollection<MatchViewModel>();
            if (CurrentObject.Admin == false)
            {
                Visibility = Visibility.Collapsed;
                VisibilityUser = Visibility.Visible;
            }
            using (MyScoreDB db = new MyScoreDB())
            {

                team = db.Teams.FirstOrDefault(p => p.Id == CurrentObject.IdTeam);
                if (db.Leagues.FirstOrDefault(p => p.TeamId == team.Id) != null)
                {
                    CurrentObject.IdChampionship = db.Leagues.FirstOrDefault(p => p.TeamId == team.Id).ChampionshipId;
                    var teams = db.Leagues.Where(p => p.ChampionshipId == CurrentObject.IdChampionship).OrderBy(p => p.Place);
                    foreach (var i in teams)
                        LeagueTeams.Add(new TeamInLeagueViewModel(i));

                    var matchesDesc = db.Matches.Where(p => p.HomeId == team.Id || p.AwayId == team.Id).OrderByDescending(p => p.Date);
                    var matches = db.Matches.Where(p => p.HomeId == team.Id || p.AwayId == team.Id).OrderBy(p => p.Date);
                    foreach (var i in matches)
                    {
                        if (i.Goal1 == null)
                            LeagueMatchCalender.Add(new MatchViewModel(i));
                    }
                    foreach (var i in matchesDesc)
                    {
                        if (i.Goal1 != null)
                            LeagueMatchResults.Add(new MatchViewModel(i));
                    }
                }

                var count = db.SelectedTeams.Count(p => p.TeamId == team.Id && p.UserId == CurrentObject.UserId);
                if (count > 0)
                {
                    MyTeam = "Убрать из моих команд";
                }
            }
        }
    }
}
