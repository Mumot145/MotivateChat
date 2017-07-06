using MotivationAdmin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MotivationAdmin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewUser : ContentPage
	{
        public event EventHandler OnNewUser;
        AzureDataService _azure;
        //ChatGroup chatGroup = new ChatGroup();
        AdminViewModel _thisAdmin = new AdminViewModel();
		public NewUser (AdminViewModel _avm)
		{
			InitializeComponent ();
            _thisAdmin = _avm;
        }
        async void AddMember(object sender, EventArgs e)
        {
            string ngMember = newGroupMember.Text;
            if (!String.IsNullOrEmpty(ngMember))
            {
                var isEmail = checkEmail(ngMember);
                var user = _azure.GetUser();
                if(user != null && isEmail == true)
                {
                    _azure.AddUserPending(ngMember, user);
                    User newUser = new User();
                    newUser.Email = ngMember;                   
                    _thisAdmin.PendingUsers.Add(newUser);
                    OnNewUser(this, new EventArgs());
                    await Navigation.PopAsync();

                }  else
                {
                    badFormat.IsVisible = true;
                }        
            } else
            {
                notFound.IsVisible = true;
                return;
            }   
        }
        bool checkEmail(string email)
        {
            if (email.Contains("@"))
                return true;
            else
                return false;
        }
    }
}
