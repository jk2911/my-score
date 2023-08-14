using MyScore.Model;
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
    public class TeamsViewModel : NavigateViewModel
    {
        public Team team;
        public string Name
        {
            get => team.Name + "(" + team.Country + ")";
        }
        public ICommand TeamPageCommand
        {
            get
            {
                return new DelegateCommand(TeamPage);
            }
        }
        public void TeamPage()
        {
            CurrentObject.IdTeam = team.Id;
            Navigate("View/TeamPage.xaml");
        }
        public TeamsViewModel(Team team)
        {
            this.team = team;
        }
    }
}
