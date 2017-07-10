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
	public partial class GroupUsers : ContentPage
	{
        ChatGroup thisChatgroup = new ChatGroup();
        AdminViewModel avm = new AdminViewModel();
        AzureDataService _azure;
        NewUser nu;
        public GroupUsers (ChatGroup _chatgroup, AdminViewModel _admin)
		{
			InitializeComponent ();
            _azure = AzureDataService.DefaultService;
            avm = _admin;
            thisChatgroup = _chatgroup;
            BindingContext = thisChatgroup;
            
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            User selectedUser = (User)e.SelectedItem;
            UserInfo ui = new UserInfo(selectedUser);
            Navigation.PushAsync(ui);
            ((ListView)sender).SelectedItem = null;
        }
        async void OnAlertYesNoClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("New User", "Add a pending user or new user for group "+ thisChatgroup.GroupName, "Pending", "New");
            Console.WriteLine("Answer: " + answer); //pending == true, new == false
            if(answer == true)
            {
                //pending select multiple page
                //var selectPage = new SelectMultipleBasePage<User>(avm.PendingUsers);
               // await Navigation.PushAsync(selectPage);
            } else
            {
                //new add new user page
                nu = new NewUser(avm, thisChatgroup.Id);
                nu.OnNewUser += new EventHandler(addingUser);
                await Navigation.PushAsync(nu);
            }
        }

        private void addingUser(object sender, EventArgs e)
        {
            User usr = nu.returnUser();
            thisChatgroup.UserList.Add(usr);
        }
    }
}
