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
    public class AddMatchViewModel : NavigateViewModel
    {
        public Championship championship;
        public ObservableCollection<Team> Teams { get; set; }
        public Team SelectedHome { get; set; }
        public Team SelectedAway { get; set; }
        public string Count { get { return Teams.Count().ToString(); } }

        public int stage = 1;
        public int Stage { get; set; }
        //{
        //    get
        //    {
        //        return stage.ToString();
        //    }
        //    set
        //    {
        //        try
        //        {
        //            if (Convert.ToByte(value) > 100)
        //                return;
        //            stage = Convert.ToInt32(value);
        //            OnPropertyChanged("Stage");
        //        }
        //        catch { return; }
        //    }
        //}
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public string Championship => championship.Name;
        public ImageSource ImageSrc
        {
            get { return SaveAndLoadPicture.LoadImage(championship.Image); }
        }
        public AddMatchViewModel()
        {
            var now = DateTime.Now;
            DateStart = now.AddYears(-2);
            DateEnd = now.AddYears(2);
            Date = now;
            Teams = new ObservableCollection<Team>();
            using (MyScoreDB db = new MyScoreDB())
            {
                championship = db.Championships.FirstOrDefault(p => p.Id == CurrentObject.IdChampionship);
                var league = db.Leagues.Where(p => p.ChampionshipId == CurrentObject.IdChampionship);
                foreach (var i in league)
                {
                    Teams.Add(db.Teams.FirstOrDefault(p => p.Id == i.TeamId));
                }
            }
        }

        public ICommand AddMatchCommand
        {
            get
            {
                return new DelegateCommand(AddMatch);
            }
        }
        public void AddMatch()
        {
            if (SelectedHome == null || SelectedAway == null)
            {
                MessageBox.Show("Вы не выбрали команду(");
                return;
            }
            if (SelectedHome.Id == SelectedAway.Id)
            {
                MessageBox.Show("Одинаковые команды");
                return;
            }
            if (Stage < 1 || Stage > 100)
            {
                MessageBox.Show("Поле тур введено не корректно(значение может быть от 1 до 99)");
                return;
            }
            Match match = new Match() { ChampionshipId = CurrentObject.IdChampionship, Date = this.Date, HomeId = SelectedHome.Id, AwayId = SelectedAway.Id, Stage = this.Stage };
            //MessageBox.Show(SelectedHome.Name + " " + SelectedAway.Name + " " + match.Id);
            using (MyScoreDB db = new MyScoreDB())
            {
                db.Matches.Add(match);
                MessageBox.Show("Матч добавлен");
                db.SaveChanges();
            }
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
    }
}
