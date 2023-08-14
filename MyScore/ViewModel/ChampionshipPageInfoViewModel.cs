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
using System.Windows.Media;
using MyScore.Logic;

namespace MyScore.ViewModel
{
    public  class ChampionshipPageInfoViewModel : NavigateViewModel
    {
        public Championship championship;
        public ObservableCollection<TeamInLeagueViewModel> LeagueTeams { get; set; }
        public ObservableCollection<MatchViewModel> LeagueMatchCalender { get; set; }
        public ObservableCollection<MatchViewModel> LeagueMatchResults { get; set; }
        public string Championship => championship.Name;
        public ImageSource ImageSrc
        {
            get { return SaveAndLoadPicture.LoadImage(championship.Image); }
        }

        public Visibility Visibility { get; set; } = Visibility.Visible;
        public ChampionshipPageInfoViewModel()
        {
            UpdateLeague();
            if (CurrentObject.Admin == false || CurrentObject.Admin == null)
                Visibility = Visibility.Collapsed;
        }

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
        public ICommand EditChampionshipCommand
        {
            get => new DelegateCommand(EditChampionship);
        }
        public void EditChampionship()
        {
            Navigate("View/EditChampionshipPage.xaml");
        }
        public ICommand RemoveChampionshipCommand
        {
            get => new DelegateCommand(RemoveChampionship);
        }
        public void RemoveChampionship() 
        {
            var message = MessageBox.Show("Вы точно хотите удалить чемпионат?", Championship, MessageBoxButton.OKCancel);
            if (message == MessageBoxResult.Cancel)
                return;
            ResultsMatch.DeleteChampionship(championship.Id);
            Navigate("View/AllChampionshipPage.xaml");
        }

        public ICommand AddMatchCommand
        {
            get => new DelegateCommand(AddMatch);
        }

        public void AddMatch()
        {
            Navigate("View/AddMatchPage.xaml");
        }
        public ICommand TeamInLeagueCommand
        {
            get => new DelegateCommand(TeamInLeague);
        }

        public void TeamInLeague()
        {
            Navigate("View/AddTeamInLeaguePage.xaml");
        }
        public ICommand UpdateLeagueCommand
        {
            get => new DelegateCommand(UpdateLeague);
        }

        public void UpdateLeague()
        {
            ResultsMatch.EditResults();
            LeagueTeams = new ObservableCollection<TeamInLeagueViewModel>();
            LeagueMatchCalender = new ObservableCollection<MatchViewModel>();
            LeagueMatchResults = new ObservableCollection<MatchViewModel>();
            using (MyScoreDB db = new MyScoreDB())
            {
                championship = db.Championships.FirstOrDefault(p => p.Id == CurrentObject.IdChampionship);
                var teams = db.Leagues.Where(p => p.ChampionshipId == CurrentObject.IdChampionship).OrderBy(p => p.Place);
                foreach (var i in teams)
                    LeagueTeams.Add(new TeamInLeagueViewModel(i));

                var matches = db.Matches.Where(p => p.ChampionshipId == CurrentObject.IdChampionship).OrderBy(p => p.Date);
                var matchesResults = db.Matches.Where(p => p.ChampionshipId == CurrentObject.IdChampionship).OrderByDescending(p => p.Date);
                foreach (var i in matches)
                {
                    if (i.Goal1 == null)
                        LeagueMatchCalender.Add(new MatchViewModel(i));
                }
                foreach (var i in matchesResults)
                {
                    if (i.Goal1 != null)
                        LeagueMatchResults.Add(new MatchViewModel(i));
                }
            }
            OnPropertyChanged("LeagueTeams");
            OnPropertyChanged("LeagueMatchCalender");
            OnPropertyChanged("LeagueMatchResults");
        }
        #endregion
    }
}

