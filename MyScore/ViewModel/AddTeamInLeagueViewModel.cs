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
    public class AddTeamInLeagueViewModel : NavigateViewModel
    {
        public Championship championship;
        public ObservableCollection<Team> Teams { get; set; }
        public Team SelectedTeam { get; set; }
        public string Championship => championship.Name;
        public ImageSource ImageSrc
        {
            get { return SaveAndLoadPicture.LoadImage(championship.Image); }
        }
        public AddTeamInLeagueViewModel()
        {
            Teams = new ObservableCollection<Team>();
            using (MyScoreDB db = new MyScoreDB())
            {
                championship = db.Championships.FirstOrDefault(p => p.Id == CurrentObject.IdChampionship);
                foreach (var i in db.Teams)
                {
                    var count = db.Leagues.Count(p => p.TeamId == i.Id);
                    if (count == 0)
                    {
                        Teams.Add(i);
                    }
                }
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
        public ICommand AddTeamInLeagueCommand
        {
            get
            {
                return new DelegateCommand(AddTeamInLeague);
            }
        }
        public void AddTeamInLeague()
        {
            if (SelectedTeam == null)
            {
                MessageBox.Show("Вы не выбрали команду");
                return;
            }
            League league = new League() { ChampionshipId = championship.Id, TeamId = SelectedTeam.Id };
            using (MyScoreDB db = new MyScoreDB())
            {
                db.Leagues.Add(league);
                db.SaveChanges();
            }
            MessageBox.Show("Команда добавлена в лигу");
            Navigate("View/ChampionshipWindow.xaml");
        }
    }
}
