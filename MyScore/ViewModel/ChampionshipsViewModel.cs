using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MyScore.Model;
using MyScore.View;
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
    public class ChampionshipsViewModel : NavigateViewModel
    {
        public Championship championship;
        public string Championship
        {
            get => championship.Name + "(" + championship.Region + ")";
        }


        
        public ICommand OnChampionshipPageInfoCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    OnChampionshipPageInfo();
                });
                //if (_OnChampionshipPageInfoCommand == null)
                //{
                //    _OnChampionshipPageInfoCommand = new DelegateCommand(() =>
                //    {
                //        ghkjl();
                //        //Navigate("ChampionshipWindow.xaml");
                //    });
                //}
                //return _OnChampionshipPageInfoCommand;
            }
            //set { _OnChampionshipPageInfoCommand = value; }
        }
        public void OnChampionshipPageInfo()
        {
            CurrentObject.IdChampionship = championship.Id;
            Navigate("View/ChampionshipWindow.xaml");
        }
        public ChampionshipsViewModel(Championship ch)
        {
            championship = ch;
        }
    }
}
