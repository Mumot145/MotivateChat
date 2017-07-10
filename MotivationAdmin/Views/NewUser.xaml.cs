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
        int groupSpecified = 0;
        User newUser = new User();
		public NewUser (AdminViewModel _avm, int groupId = 0)
		{
			InitializeComponent ();
            _azure = AzureDataService.DefaultService;
            _thisAdmin = _avm;
            groupSpecified = groupId;
        }
        async void AddMember(object sender, EventArgs e)
        {
            string ngMember = newGroupMember.Text;
            if (!String.IsNullOrEmpty(ngMember))
            {
                var isEmail = checkEmail(ngMember);
               // var user = _azure.GetUser();
                
                if (_thisAdmin.ThisUser != null && isEmail == true)
                {
                    if (groupSpecified > 0)
                    {
                        _azure.AddUserPending(ngMember, _thisAdmin.ThisUser, groupSpecified);
                        newUser = new User();
                        newUser.Email = ngMember;
                        _thisAdmin.UsersChatGroups.Where(g=>g.Id == groupSpecified).First().UserList.Add(newUser);
                        OnNewUser(this, new EventArgs());
                        await Navigation.PopAsync();
                    }  else
                    {
                        _azure.AddUserPending(ngMember, _thisAdmin.ThisUser);
                        User newUser = new User();
                        newUser.Email = ngMember;
                        _thisAdmin.PendingUsers.Add(newUser);
                        OnNewUser(this, new EventArgs());
                        await Navigation.PopAsync();

                    }

                }
                else
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
        public User returnUser()
        {
            return newUser;
        }

    }
}
