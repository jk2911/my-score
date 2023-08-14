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
            CurrentObject.IdTeam = league.TeamId;
            MessageBox.Show("Команда");
            //CurrentObject.IdChampionship = championship.Id;
            Navigate("View/TeamPage.xaml");
        }
        public TeamInLeagueViewModel(League l)
        {
            league = l;
        }
    }
}
