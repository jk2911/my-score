using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using MyScore.Logic;
using MyScore.Model;
using MyScore.Model.DB;
using MyScore.ViewModel.Base;

namespace MyScore.ViewModel
{
    public class EditChampionshipViewModel : NavigateViewModel
    {
        public Championship ch;
        public string Name
        {
            get
            {
                return ch.Name;
            }
            set
            {
                ch.Name = value.Trim();
                OnPropertyChanged();
            }
        }
        public string Region
        {
            get
            {
                return ch.Region;
            }
            set
            {
                ch.Region = value.Trim();
                OnPropertyChanged();
            }
        }
        public ImageSource Image
        {
            get
            {
                return SaveAndLoadPicture.LoadImage(ch.Image);
            }
            set { }
        }
        #region Command
        public ICommand EditChampionshipCommand
        {
            get
            {
                return new DelegateCommand(EditChampionship);
            }
        }
        public void EditChampionship()
        {
            using (MyScoreDB db = new MyScoreDB())
            {
                var count = db.Championships.Count(p => p.Name == this.ch.Name && p.Region == this.ch.Region && p.Image == this.ch.Image);
                if (count > 0)
                {
                    MessageBox.Show("Вы ничего не изменили");
                    return;
                }
                count = db.Championships.Count(p => p.Name == ch.Name && p.Region == ch.Region && p.Id != ch.Id);
                if (count > 0)
                {
                    MessageBox.Show("Такой чемпионат уже есть");
                    return;
                }
                if (ch.Name == "" || ch.Region == "" || ch.Name == null || ch.Region == null)
                {
                    MessageBox.Show("Не все поля введены");
                    return;
                }
                var stationRule = new Regex(@"^[а-яА-Я]+(?:[\s-][а-яА-Я]+)*$");
                if (!stationRule.IsMatch(ch.Name))
                {
                    MessageBox.Show("В строке не может быть 2 пробела(Название)");
                    return;
                }
                if (!stationRule.IsMatch(ch.Region))
                {
                    MessageBox.Show("В строке не может быть 2 пробела(Регион)");
                    return;
                }
                var championship = db.Championships.FirstOrDefault(p => p.Id == this.ch.Id);
                championship.Name = ch.Name;
                championship.Region = ch.Region;
                championship.Image = ch.Image;
                db.SaveChanges();
                MessageBox.Show("Лига изменена");

            }
            Navigate("View/ChampionshipWindow.xaml");
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
                ch.Image = SaveAndLoadPicture.PictureToByte(openwnd.FileName);
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
            Navigate("View/ChampionshipWindow.xaml");
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
            ch.Image = null;
            OnPropertyChanged("Image");
        }
        #endregion

        public EditChampionshipViewModel()
        {
            using(MyScoreDB db = new MyScoreDB())
            {
                 ch = db.Championships.FirstOrDefault(p => p.Id == CurrentObject.IdChampionship);
            }
        }
    }
}
