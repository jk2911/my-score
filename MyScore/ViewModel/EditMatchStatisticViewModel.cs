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
    public class EditMatchStatisticViewModel : NavigateViewModel
    {
        public Match match;
        public Statistic statistic;
        public Championship championship;
        #region Свойства
        public string Championship => championship.Name;
        public ImageSource ImageSrc
        {
            get { return SaveAndLoadPicture.LoadImage(championship.Image); }
        }
        public Team HomeTeam { get; set; }
        public string HomeName
        {
            get => HomeTeam.Name;
        }
        public Team AwayTeam { get; set; }
        public string AwayName
        {
            get => AwayTeam.Name;
        }
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public int Stage { get; set; }

        public int Goal1 { get; set; } = 0;
        public int Goal2 { get; set; } = 0;
        public int BallControl1 { get; set; } = 50;

        public int BallControl2 { get; set; } = 50;

        public int Offside1 { get; set; } = 0;

        public int Offside2 { get; set; } = 0;

        public int Corner1 { get; set; } = 0;

        public int Corner2 { get; set; } = 0;

        public int RedCard1 { get; set; } = 0;

        public int RedCard2 { get; set; } = 0;

        public int YellowCard1 { get; set; } = 0;

        public int YellowCard2 { get; set; } = 0;

        public int Shots1 { get; set; } = 0;

        public int Shots2 { get; set; } = 0;

        #endregion

        public ICommand EditMatchCommand
        {
            get
            {
                return new DelegateCommand(EditMatch);
            }
        }
        public void EditMatch()
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                if (BallControl1 < 1 || BallControl1 > 99 || BallControl2 < 1 || BallControl2 > 99 || BallControl1 + BallControl2 != 100)
                {
                    MessageBox.Show("Не корректно введены данные в поля Владение(Владение должно быть больше 1 и меньше 99, а сумма 2 полей должна быть равна 100)");
                    return;
                }
                if (Goal1 < 0 || Goal1 > 99 || Goal2 < 0 || Goal2 > 99)
                {
                    MessageBox.Show("Поле гол введено не корректно(значение может быть от 0 до 99)");
                    return;
                }
                if (Shots1 < 0 || Shots1 > 99 || Shots2 < 0 || Shots2 > 99)
                {
                    MessageBox.Show("Поле удары введено не корректно(значение может быть от 0 до 99)");
                    return;
                }
                if (Corner1 < 0 || Corner1 > 99 || Corner2 < 0 || Corner2 > 99)
                {
                    MessageBox.Show("Поле угловые введено не корректно(значение может быть от 0 до 99)");
                    return;
                }
                if (RedCard1 < 0 || RedCard1 > 10 || RedCard2 < 0 || RedCard2 > 10)
                {
                    MessageBox.Show("Поле красные карточки введено не корректно(значение может быть от 0 до 10)");
                    return;
                }
                if (YellowCard1 < 0 || YellowCard1 > 30 || YellowCard2 < 0 || YellowCard2 > 30)
                {
                    MessageBox.Show("Поле желтые карточки введено не корректно(значение может быть от 0 до 30)");
                    return;
                }
                if (Offside1 < 0 || Offside1 > 99 || Offside2 < 0 || Offside2 > 99)
                {
                    MessageBox.Show("Поле офсайды введено не корректно(значение может быть от 0 до 99)");
                    return;
                }
                if (Stage < 1 || Stage > 100)
                {
                    MessageBox.Show("Поле тур введено не корректно(значение может быть от 1 до 99)");
                    return;
                }
                if (statistic.Id == 0)
                {
                    statistic.MatchId = this.match.Id;
                    statistic.BallControl1 = (byte)this.BallControl1;
                    statistic.BallControl2 = (byte)this.BallControl2;
                    statistic.Offside1 = (byte)this.Offside1;
                    statistic.Offside2 = (byte)this.Offside2;
                    statistic.Corner1 = (byte)this.Corner1;
                    statistic.Corner2 = (byte)this.Corner2;
                    statistic.RedCard1 = (byte)this.RedCard1;
                    statistic.RedCard2 = (byte)this.RedCard2;
                    statistic.YellowCard1 = (byte)this.YellowCard1;
                    statistic.YellowCard2 = (byte)this.YellowCard2;
                    statistic.Shots1 = (byte)this.Shots1;
                    statistic.Shots2 = (byte)this.Shots2;
                    db.Statistics.Add(statistic);
                    db.SaveChanges();
                }
                else
                {
                    var statistic = db.Statistics.FirstOrDefault(p => p.Id == this.statistic.Id);
                    statistic.MatchId = this.statistic.MatchId;
                    statistic.BallControl1 = (byte)this.BallControl1;
                    statistic.BallControl2 = (byte)this.BallControl2;
                    statistic.Offside1 = (byte)this.Offside1;
                    statistic.Offside2 = (byte)this.Offside2;
                    statistic.Corner1 = (byte)this.Corner1;
                    statistic.Corner2 = (byte)this.Corner2;
                    statistic.RedCard1 = (byte)this.RedCard1;
                    statistic.RedCard2 = (byte)this.RedCard2;
                    statistic.YellowCard1 = (byte)this.YellowCard1;
                    statistic.YellowCard2 = (byte)this.YellowCard2;
                    statistic.Shots1 = (byte)this.Shots1;
                    statistic.Shots2 = (byte)this.Shots2;
                    db.SaveChanges();
                }
                var match = db.Matches.FirstOrDefault(p => p.Id == this.match.Id);
                match.Date = this.Date;
                match.Stage = this.Stage;
                match.Goal1 = (byte)this.Goal1;
                match.Goal2 = (byte)this.Goal2;
                db.SaveChanges();
                MessageBox.Show("Матч изменен");
            }
            ResultsMatch.EditResults();
            Cancel();
        }

        public ICommand CancelCommand
        {
            get
            {
                return new DelegateCommand(Cancel);
            }
        }
        public void Cancel()
        {
            Navigate("View/ChampionshipWindow.xaml");
        }

        public ICommand DeleteResultsCommand
        {
            get
            {
                return new DelegateCommand(DeleteResults);
            }
        }
        public void DeleteResults()
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                if (statistic.Id != 0)
                {
                    var statistic = db.Statistics.FirstOrDefault(p => p.Id == this.statistic.Id);
                    db.Statistics.Remove(statistic);
                    db.SaveChanges();
                } 
                var match = db.Matches.FirstOrDefault(p => p.Id == this.match.Id);
                match.Goal1 = null;
                match.Goal2 = null;
                db.SaveChanges();
            }
            ResultsMatch.EditResults();
            Cancel();
        }
        public ICommand DeleteCommand
        {
            get
            {
                return new DelegateCommand(Delete);
            }
        }
        public void Delete()
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                if (statistic.Id != 0)
                {
                    var statistic = db.Statistics.FirstOrDefault(p => p.Id == this.statistic.Id);
                    db.Statistics.Remove(statistic);
                    db.SaveChanges();
                }
                var match = db.Matches.FirstOrDefault(p => p.Id == this.match.Id);
                db.Matches.Remove(match);
                db.SaveChanges();
            }
            ResultsMatch.EditResults();
            Cancel();
        }
        public EditMatchStatisticViewModel() 
        {
            var now = DateTime.Now;
            DateStart = now.AddYears(-2);
            DateEnd = now.AddYears(2);

            statistic = new Statistic();
            using (MyScoreDB db = new MyScoreDB())
            { 
                championship = db.Championships.FirstOrDefault(p => p.Id == CurrentObject.IdChampionship);
                match = db.Matches.FirstOrDefault(p => p.Id == CurrentObject.IdMatch);
                Date = match.Date;
                Stage = match.Stage;
                HomeTeam = db.Teams.FirstOrDefault(p => p.Id == match.HomeId);
                AwayTeam = db.Teams.FirstOrDefault(p => p.Id == match.AwayId);
                var count = db.Statistics.Count(p => p.MatchId == CurrentObject.IdMatch);
                if (count == 1)
                {
                    statistic = db.Statistics.FirstOrDefault(p => p.MatchId == CurrentObject.IdMatch);
                    if (match.Goal1 != null)
                    {
                        Goal1 = (byte)match.Goal1;
                        Goal2 = (byte)match.Goal2;
                    }

                    BallControl1 = (byte)statistic.BallControl1;
                    BallControl2 = (byte)statistic.BallControl2;
                    Offside1 = (byte)statistic.Offside1;
                    Offside2 = (byte)statistic.Offside2;
                    RedCard1= (byte)statistic.RedCard1;
                    RedCard2 = (byte)statistic.RedCard2;
                    YellowCard1 = (byte)statistic.YellowCard1;
                    YellowCard2 = (byte)statistic.YellowCard2;
                    Shots1 = (byte)statistic.Shots1;
                    Shots1 = (byte)statistic.YellowCard2;
                    Corner1 = (byte)statistic.Corner1;
                    Corner2 = (byte)statistic.Corner2;
                }
            }
            
       
        }

    }
}
