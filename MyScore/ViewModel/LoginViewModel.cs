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
using System.Windows.Controls;
using System.Windows.Input;

namespace MyScore.ViewModel
{
    public class LoginViewModel : NavigateViewModel
    {
        public string Email { get; set; }
        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand<object>((x) =>
                {
                    if (x != null) { Reg(x); }
                });
            }
        }
        public void Reg(object pass)
        {
            var password = (PasswordBox)pass;
            int id;
            using (MyScoreDB db = new MyScoreDB())
            {
                var count = db.Users.Count(p => p.Email == Email);
                if (count == 0)
                {
                    MessageBox.Show("Зарегистрированного пользователя с таким Email нет");
                    return;
                }
                var user = db.Users.FirstOrDefault(p => p.Email == Email);
                id = user.Id;
                if (!HashPassword.VerifyHashedPassword(user.Password, password.Password))
                {
                    MessageBox.Show("Неверный пароль");
                    return;
                }
                CurrentObject.Admin = user.Admin;
                CurrentObject.UserId = id;
            }
            CurrentObject.Email = Email;
            Cancel();
        }
        public ICommand RegCommand => new DelegateCommand(Reg);
        public void Reg()
        {
            Navigate("View/RegPage.xaml");
        }
        public ICommand CancelCommand
        {
            get => new DelegateCommand(Cancel);
        }
        public void Cancel()
        {
            Navigate("View/AllChampionshipPage.xaml");
        }
        public LoginViewModel() { }
    }
}
