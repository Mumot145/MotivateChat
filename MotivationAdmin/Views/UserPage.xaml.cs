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
        ChatGroup _thisChatGroup = new ChatGroup();
        List<User> allUsers = new List<User>();
		public UserPage(List<ChatGroup> _chatGroups)
		{
			InitializeComponent ();   
            if(_chatGroups != null)
            {
                foreach (var cg in _chatGroups)
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
            for(int i = 0; i < 15; i++)
            {
                User user = new User();
                user.Name = "Patrick";
                allUsers.Add(user);
            }
            userList.ItemsSource = allUsers;
        }          
        async void OnClick(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            NewUser nu = new NewUser(_thisChatGroup);
            await Navigation.PushAsync(nu);
        }

        async private void userList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e == null || e.SelectedItem == null) return;

            var selectedUser = (User)e.SelectedItem;
            Console.WriteLine("select =>"+selectedUser.Id+"---"+selectedUser.Name+"----");           
            UserInfo ui = new UserInfo(selectedUser);
            await Navigation.PushAsync(ui);
            userList.SelectedItem = null;
        }
    }
}
