using MotivationAdmin.Controls;
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
	public partial class UserPage : ContentPage
	{
        //ChatGroup _thisChatGroup = new ChatGroup();
        List<User> allUsers = new List<User>();
        AdminViewModel _thisAdmin = new AdminViewModel();
        public event EventHandler<UserArgs> OnEditUser;
        public UserPage(AdminViewModel avm)
		{
			InitializeComponent ();
            _thisAdmin = avm;

            refreshUsers();


            userList.ItemsSource = allUsers;
        }          

        async void Button_Clicked(object sender, EventArgs e)
        {
            NewUser nu = new NewUser(_thisAdmin);
            nu.OnNewUser += new EventHandler(addingUser);
            await Navigation.PushAsync(nu);
        }
        void refreshUsers()
        {
            if (_thisAdmin.UsersChatGroups != null)
            {
                foreach (var cg in _thisAdmin.UsersChatGroups)
                {
                    if (cg.UserList != null)
                    {
                        var regUsers = cg.UserList.Where(ul => ul.Admin != true).ToList();
                        foreach (var ru in regUsers)
                        {
                            ru.AttachedGroup = cg.SoloGroup;
                            allUsers.Add(ru);
                        }
                    }
                }
            }
            else
            {
                userList.IsVisible = false;
                //isEmpty.IsVisible = true;
            }
            if (_thisAdmin.PendingUsers != null)
            {
                if(_thisAdmin.PendingUsers.Count > 0)
                {
                    foreach (var u in _thisAdmin.PendingUsers)
                    {                       
                        allUsers.Add(u);                                            
                    }
                }
                
            }
            else
            {
                userList.IsVisible = false;
                //isEmpty.IsVisible = true;
            }
        }
        private void userList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedUser = (User)e.SelectedItem;
            //Console.WriteLine(selectedUser.Name);
            OnEditUser(this, new UserArgs(selectedUser));
           
           ((ListView)sender).SelectedItem = null;
        }
        void addingUser(object sender, EventArgs e)
        {

        }
    }
}
