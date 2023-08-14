using MyScore.ViewModel.Base;
using MyScore.Model;
using MyScore.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyScore.Model.DB;
using System.Windows;

namespace MyScore.ViewModel
{
    public class MatchViewModel : NavigateViewModel
    {
        public Match match;
        //public delegate FunctionAdmin;
        #region Свойства

        public string Time
        {
            get
            {
                return match.Date.ToString();
            }
        }
        public string Championship
        {
            get
            {
                using(MyScoreDB db = new MyScoreDB())
                {
                    var championship = db.Championships.FirstOrDefault(p => p.Id == match.ChampionshipId);
                    return championship.Name;
                }
            }
        }
        public string Stage
        {
            get
            {
                return "Тур " + match.Stage.ToString();
            }
        }

        public string HomeTeam
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    var team = db.Teams.FirstOrDefault(p => p.Id == match.HomeId);
                    return team.Name;
                }
            }
        }

        public string AwayTeam
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    var team = db.Teams.FirstOrDefault(p => p.Id == match.AwayId);
                    return team.Name;
                }
            }
        }

        public string Goals1
        {
            get
            {
                return match.Goal1.ToString();
            }
        }
        public string Goals2
        {
            get
            {
                return match.Goal2.ToString();
            }
        }

        #endregion

        #region OpenMatch

        public ICommand OpenMatchWindow { get; set; }

        private void OnOpenMatchWindow()
        {
            CurrentObject.IdMatch = match.Id;
            Navigate("View/MatchStatistic.xaml");
        }
        private void OnOpenMatchWindowUser()
        {
            CurrentObject.IdMatch = match.Id;
            MatchWindow statistic = new MatchWindow();
            statistic.ShowDialog();
        }
        private void UserNull()
        {
            MessageBox.Show("Незарегистрированный пользователь не может смотреть информацию о  матче");
        }
        #endregion

        #region Конструктор
        public MatchViewModel(Match match)
        {
            OpenMatchWindow = new DelegateCommand(OnOpenMatchWindow);
            this.match = match;
            if (CurrentObject.Admin == false)
                OpenMatchWindow = new DelegateCommand(OnOpenMatchWindowUser);
            if (CurrentObject.Admin == null)
                OpenMatchWindow = new DelegateCommand(UserNull);
        }
        public MatchViewModel()
        {

        }
        #endregion
    }
}
