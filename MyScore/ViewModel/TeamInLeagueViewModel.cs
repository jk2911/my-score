using MyScore.Logic;
using MyScore.Model;
using MyScore.Model.DB;
using MyScore.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyScore.ViewModel
{
    public class TeamInLeagueViewModel : NavigateViewModel
    {
        public League league;
        #region Свойства
        public string Championship
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    var championship = db.Championships.FirstOrDefault(p => p.Id == league.ChampionshipId);
                    return championship.Name;
                }
            }
        }
        public string Place
        {
            get
            {
                return league.Place.ToString();
            }
        }
        public string Team
        {
            get
            {
                using(MyScoreDB db = new MyScoreDB())
                {
                    var team = db.Teams.FirstOrDefault(p => p.Id == league.TeamId);
                    return team.Name;
                } 
            }
        }
        public string CountMatches
        {
            get
            {
                return league.CountMatches.ToString();
            }
        }
        public string Win
        {
            get
            {
                return league.Win.ToString();
            }
        }
        public string Draw
        {
            get
            {
                return league.Draw.ToString();
            }
        }
        public string Lose
        {
            get
            {
                return league.Lose.ToString();
            }
        }
        public string ScoredGoals
        {
            get
            {
                return league.ScoreGoals.ToString();
            }
        }
        public string ConcedeGoals
        {
            get
            {
                return league.ConcededGoals.ToString();
            }
        }
        public string Point
        {
            get
            {
                return league.Point.ToString();
            }
        }

        public Visibility Visibility { get; set; } = Visibility.Visible;
        #endregion

        public ICommand OnTeamPage
        {
            get 
            {
                return new DelegateCommand(() =>
                {
                    OpenTeamPage();
                });
            }
        }
        public void OpenTeamPage() 
        {
            if (CurrentObject.Admin == null)
            {
                MessageBox.Show("Незарегистрированный пользователь не может смотреть страницу команды");
                return;
            }
            CurrentObject.IdTeam = league.TeamId;
            Navigate("View/TeamPage.xaml");
        }

        public ICommand DeleteTeamCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    DeleteTeam();
                });
            }
        }
        public void DeleteTeam()
        {
            using(MyScoreDB db=new MyScoreDB())
            {
                var league = db.Leagues.FirstOrDefault(p => p.Id == this.league.Id);
                if (league != null)
                {
                    db.Leagues.Remove(league);
                    db.SaveChanges();
                }
            }
            ResultsMatch.DeleteMatches(league.ChampionshipId, league.TeamId);
            MessageBox.Show("Команда удалена из лиги");
            Navigate("View/ChampionshipWindow.xaml");
        }
        public TeamInLeagueViewModel(League l)
        {
            league = l;
            if (CurrentObject.Admin == false || CurrentObject.Admin == null)
                Visibility = Visibility.Collapsed;
        }
    }
}
