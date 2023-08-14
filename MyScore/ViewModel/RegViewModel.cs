using MyScore.Model;
using MyScore.Model.DB;
using MyScore.ViewModel.Base;
using MyScore.Logic;
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
    public class RegViewModel : NavigateViewModel
    {
        public string Email { get; set; } = "";
        public ICommand RegCommand
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
            Regex femail = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");///для почты
            if (Email == null || !femail.IsMatch(Email))
            {
                MessageBox.Show("Неправильный формат электронной почты");
                return;
            }
            using(MyScoreDB db=new MyScoreDB())
            {
                var count = db.Users.Count(p => p.Email == Email);
                if (count == 1)
                {
                    MessageBox.Show("Пользователь с таким Email уже зарегистрирован");
                    return;
                }
            }
            if (password.Password.Length < 7)
            {
                MessageBox.Show("Длина пароля должна быть от 8 до 16 символов");
                return;
            }
            using(MyScoreDB db=new MyScoreDB())
            {
                User user = new User();
                user.Email = Email;
                user.Password = HashPassword.Hash(password.Password);
                db.Users.Add(user);
                db.SaveChanges();
                id = db.Users.FirstOrDefault(p => p.Email == Email).Id;
            }
            CurrentObject.Admin = false;
            CurrentObject.Email = Email;
            CurrentObject.UserId = id;
            Cancel();
        }
        public ICommand LoginCommand
        {
            get => new DelegateCommand(Login);
        }
        public void Login()
        {
            Navigate("View/LoginPage.xaml");
        }
        public ICommand CancelCommand
        {
            get => new DelegateCommand(Cancel);
        }
        public void Cancel()
        {
            Navigate("View/AllChampionshipPage.xaml");
        }
        public RegViewModel() { }
    }
}
