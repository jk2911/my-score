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
    public class AddNewChampionshipViewModel : NavigateViewModel
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
                return  SaveAndLoadPicture.LoadImage(ch.Image); 
            }
            set {  }
        }
        #region Command
        public ICommand AddChampionshipCommand
        {
            get
            {
                return new DelegateCommand(AddChampionship);
            }
        }
        public void AddChampionship()
        {
            using(MyScoreDB db = new MyScoreDB())
            {
                var count = db.Championships.Count(p => p.Name == ch.Name && p.Region == ch.Region);
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
                db.Championships.Add(ch);
                db.SaveChanges();
                MessageBox.Show("Лига добавлена");
                
            }
            Navigate("View/AllChampionshipPage.xaml");
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
            ch.Image = null;
            OnPropertyChanged("Image");
        }
        #endregion
        public AddNewChampionshipViewModel() 
        {
            ch = new Championship();
        }
    }
}
