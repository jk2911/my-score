using Microsoft.Win32;
using MyScore.Logic;
using MyScore.Model;
using MyScore.Model.DB;
using MyScore.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MyScore.ViewModel
{
    public class AddNewTeamViewModel : NavigateViewModel
    {
        public Team team;
        public string imagename = null;
        public string Name
        {
            get
            {
                return team.Name;
            }
            set
            {
                team.Name = value.Trim();
                OnPropertyChanged();
            }
        } 
        public string Country
        {
            get
            {
                return team.Country;
            }
            set
            {
                team.Country = value;
                OnPropertyChanged();
            }
        }
        public ImageSource Image
        {
            get
            {
                return SaveAndLoadPicture.LoadImage(team.Image);
            }
            set { }
        }
        #region Command
        public ICommand AddTeamCommand
        {
            get
            {
                return new DelegateCommand(AddTeam);
            }
        }
        public void AddTeam()
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                var count = db.Teams.Count(p => p.Name == team.Name && p.Country == team.Country);
                if (count > 0)
                {
                    MessageBox.Show("Такая команда уже есть");
                    return;
                }
                if (team.Name == "" || team.Country == "" || team.Name == null || team.Country == null)
                {
                    MessageBox.Show("Не все поля введены");
                    return;
                }
                var stationRule = new Regex(@"^[а-яА-Я]+(?:[\s-][а-яА-Я]+)*$");
                if (!stationRule.IsMatch(team.Name))
                {
                    MessageBox.Show("В строке не может быть 2 пробела(Название)");
                    return;
                }
                if (!stationRule.IsMatch(team.Country))
                {
                    MessageBox.Show("В строке не может быть 2 пробела(Страна)");
                    return;
                }
                db.Teams.Add(team);
                db.SaveChanges();
                MessageBox.Show("Команда добавлена");
                Navigate("View/AllChampionshipPage.xaml");
            }
        }
        public ICommand AddImageCommand
        {
            get
            {
                return new DelegateCommand(AddImage);
            }
        }

        public void AddImage()
        {
            try
            {
                OpenFileDialog openwnd = new OpenFileDialog
                {
                    Filter = "Image files(*.png)|*.png|Image files(*.jpg)|*.jpg"
                };
                openwnd.ShowDialog();
                team.Image = SaveAndLoadPicture.PictureToByte(openwnd.FileName);
                OnPropertyChanged("Image");

            }
            catch
            {
                return;
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
            Navigate("View/AllChampionshipPage.xaml");
        }

        public ICommand RemoveImageCommand
        {
            get
            {
                return new DelegateCommand(RemoveImage);
            }
        }

        public void RemoveImage()
        {
            team.Image = null;
            OnPropertyChanged("Image");
        }
        #endregion
        public AddNewTeamViewModel() {  team = new Team(); }
    }
}
