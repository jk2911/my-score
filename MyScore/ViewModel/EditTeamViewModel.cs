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
    public class EditTeamViewModel : NavigateViewModel
    {
        public Team team;
        public string Name
        {
            get
            {
                return team.Name.Trim();
            }
            set
            {
                team.Name = value;
                OnPropertyChanged();
            }
        }
        public string Country
        {
            get
            {
                return team.Country.Trim();
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
        public ICommand EditTeamCommand
        {
            get
            {
                return new DelegateCommand(EditTeam);
            }
        }
        public void EditTeam()
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                var count = db.Teams.Count(p => p.Name == this.team.Name && p.Country == this.team.Country && p.Image == this.team.Image);
                if (count > 0)
                {
                    MessageBox.Show("Вы ничего не изменили");
                    return;
                }
                count = db.Teams.Count(p => p.Name == this.team.Name && p.Country == this.team.Country && p.Id != this.team.Id);
                if (count > 0)
                {
                    MessageBox.Show("Такая команда уже есть");
                    return;
                }
                if (this.team.Name == "" || this.team.Country == "" || this.team.Name == null || this.team.Country == null)
                {
                    MessageBox.Show("Не все поля введены");
                    return;
                }
                var stationRule = new Regex(@"^[а-яА-Я]+(?:[\s-][а-яА-Я]+)*$");
                if (!stationRule.IsMatch(this.team.Name))
                {
                    MessageBox.Show("В строке не может быть 2 пробела(Название)");
                    return;
                }
                if (!stationRule.IsMatch(this.team.Country))
                {
                    MessageBox.Show("В строке не может быть 2 пробела(Страна)");
                    return;
                }
                var team = db.Teams.FirstOrDefault(p => p.Id == this.team.Id);
                team.Name = this.team.Name;
                team.Country = this.team.Country;
                team.Image = this.team.Image;
                db.SaveChanges();
                MessageBox.Show("Команда изменена");

            }
            Navigate("View/TeamPage.xaml");
        }

        public ICommand EditImageCommand
        {
            get
            {
                return new DelegateCommand(EditImage);
            }
        }

        public void EditImage()
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
            Navigate("View/TeamPage.xaml");
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

        public EditTeamViewModel()
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                team = db.Teams.FirstOrDefault(p => p.Id == CurrentObject.IdTeam);
            }
        }
    }
}
