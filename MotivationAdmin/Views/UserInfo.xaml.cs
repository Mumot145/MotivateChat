using MotivationAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MotivationAdmin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInfo : ContentPage
    {
        public UserInfo(User _user)
        {
            InitializeComponent();
            int _agelimit = 3;
            int _phonelimit = 10;
            if(_user != null)
            {
                if (_user.Name != null)
                    nameEntry.Text = _user.Name;

                if (_user.Phone != null)
                    phoneEntry.Text = _user.Phone;

                phoneEntry.TextChanged += (sender, args) =>
                {
                    string _text = phoneEntry.Text;      //Get Current Text
                    if (_text.Length > _phonelimit)       //If it is more than your character restriction
                    {
                        _text = _text.Remove(_text.Length - 1);  // Remove Last character
                        phoneEntry.Text = _text;        //Set the Old value
                    }
                };

                if (_user.Age > 0)
                    ageEntry.Text = _user.Age.ToString();

                ageEntry.TextChanged += (sender, args) =>
                {
                    string _text = ageEntry.Text;      //Get Current Text
                    if (_text.Length > _agelimit)       //If it is more than your character restriction
                    {
                        _text = _text.Remove(_text.Length - 1);  // Remove Last character
                        ageEntry.Text = _text;        //Set the Old value
                    }
                };

                if (_user.Email != null)
                    emailEntry.Text = _user.Email.ToString();

                if (_user.Gender == true)
                    genderSwitch.IsEnabled = true;
                else
                    genderSwitch.IsEnabled = false;
            }
        }
	}
}
