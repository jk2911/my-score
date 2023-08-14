using MyScore.Logic;
using MyScore.Model;
using MyScore.Model.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MyScore.ViewModel
{
    public class MatchWindowViewModel : NavigateViewModel
    {
        public Match match;
        public Statistic statistic;
        public ObservableCollection<LastMatchViewModel> HomeMatches { get; set; }
        public ObservableCollection<LastMatchViewModel> AwayMatches { get; set; }
        public Visibility Visibility { get; set; } = Visibility.Visible;
        #region Свойства
        public string Championship
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    return db.Championships.FirstOrDefault(p => p.Id == match.ChampionshipId).Name.ToString() + ": Тур " + match.Stage.ToString();
                }
            }
        }
        public ImageSource HomeSrc
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    return SaveAndLoadPicture.LoadImage(db.Teams.FirstOrDefault(p => p.Id == match.HomeId).Image);
                }
            }
        }
        public ImageSource AwaySrc
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    return SaveAndLoadPicture.LoadImage(db.Teams.FirstOrDefault(p => p.Id == match.AwayId).Image);
                }
            }
        }
        public string HomeName
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    return db.Teams.FirstOrDefault(p => p.Id == match.HomeId).Name;
                }
            }
        }
        public string AwayName
        {
            get
            {
                using (MyScoreDB db = new MyScoreDB())
                {
                    return db.Teams.FirstOrDefault(p => p.Id == match.AwayId).Name;
                }
            }
        }
        public string Date
        {
            get => match.Date.ToShortDateString().ToString();
        }

        public string Goal1
        {
            get
            {
                return match.Goal1.ToString();
            } 
        }
        public string Goal2
        {
            get
            {
                return match.Goal2.ToString();
            }
        }
        public string BallControl1
        {
            get
            {
                return statistic.BallControl1.ToString();
            }
        }

        public string BallControl2
        {
            get
            {
                return statistic.BallControl2.ToString();
            }
        }

        public string Offside1
        {
            get
            {
                return statistic.Offside1.ToString();
            }
        }

        public string Offside2
        {
            get
            {
                return statistic.Offside2.ToString();
            } 
        }

        public string Corner1
        {
            get
            {
                return statistic.Corner1.ToString();
            }
        }

        public string Corner2
        {
            get
            {
                return statistic.Corner2.ToString();
            }
        }

        public string RedCard1
        {
            get
            {
                return statistic.RedCard1.ToString();
            }
        }

        public string RedCard2
        {
            get
            {
                return statistic.RedCard2.ToString();
            }
        }

        public string YellowCard1
        {
            get
            {
                return statistic.YellowCard1.ToString();
            }
        }

        public string YellowCard2
        {
            get
            {
                return statistic.YellowCard2.ToString();
            }
        }

        public string Shots1
        {
            get
            {
                return statistic.Shots1.ToString();
            } 
        }

        public string Shots2
        {
            get
            {
                return statistic.Shots2.ToString();
            }
        }
        #endregion
        #region Value

        public int BallControl1Value
        {
            get
            {
                return statistic.BallControl1;
            }
        }

        public int BallControl2Value
        {
            get
            {
                return statistic.BallControl2;
            }
        }

        public int MaxOffside
        {
            get
            {
                return statistic.Offside1 + statistic.Offside2;
            }
        }

        public int Offside1Value
        {
            get
            {
                return statistic.Offside1;
            }
        }

        public int Offside2Value
        {
            get
            {
                return statistic.Offside2;
            }
        }
        public int MaxCorner
        {
            get
            {
                return statistic.Corner1 + statistic.Corner2;
            }
        }
        public int Corner1Value
        {
            get
            {
                return statistic.Corner1;
            }
        }

        public int Corner2Value
        {
            get
            {
                return statistic.Corner2;
            }
        }
        public int MaxRedCard
        {
            get
            {
                return statistic.RedCard1 + statistic.RedCard2;
            }
        }
        public int RedCard1Value
        {
            get
            {
                return statistic.RedCard1;
            }
        }

        public int RedCard2Value
        {
            get
            {
                return statistic.RedCard2;
            }
        }
        public int MaxYellowCard
        {
            get
            {
                return statistic.YellowCard1 + statistic.YellowCard2;
            }
        }
        public int YellowCard1Value
        {
            get
            {
                return statistic.YellowCard1;
            }
        }

        public int YellowCard2Value
        {
            get
            {
                return statistic.YellowCard2;
            }
        }
        public int MaxShots
        {
            get
            {
                return statistic.Shots1 + statistic.Shots2;
            }
        }
        public int Shots1Value
        {
            get
            {
                return statistic.Shots1;
            }
        }

        public int Shots2Value
        {
            get
            {
                return statistic.Shots2;
            }
        }

        #endregion
        public MatchWindowViewModel()
        {
            statistic = new Statistic();
            HomeMatches = new ObservableCollection<LastMatchViewModel>();
            AwayMatches = new ObservableCollection<LastMatchViewModel>();
            using (MyScoreDB db = new MyScoreDB())
            {
                this.match = db.Matches.FirstOrDefault(p => p.Id == CurrentObject.IdMatch);
                if (match.Goal1 != null)
                    statistic = db.Statistics.FirstOrDefault(p => p.MatchId == this.match.Id);
                var home = db.Matches.Where(p => p.Id != match.Id && (p.HomeId == match.HomeId || p.AwayId == match.HomeId) && p.Goal1!=null).OrderByDescending(p => p.Date).Take(5);
                var away = db.Matches.Where(p => p.Id != match.Id && (p.HomeId == match.AwayId || p.AwayId == match.AwayId) && p.Goal1 != null).OrderByDescending(p => p.Date).Take(5);
                //try
                //{
                    foreach (var i in home)
                        HomeMatches.Add(new LastMatchViewModel(i));
                //}
                //catch { }
                try
                {
                    foreach (var i in away)
                        AwayMatches.Add(new LastMatchViewModel(i));
                }
                catch { }
            }
            if (match.Goal1 == null)
                Visibility = Visibility.Collapsed;
        }
    }
}
