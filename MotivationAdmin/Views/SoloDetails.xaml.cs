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
	public partial class SoloDetails : ContentPage
	{
        ChatGroup _currentChatGroup = new ChatGroup();
        GroupMessages groupMsgs;
        AdminViewModel thisAdmin = new AdminViewModel();
        public SoloDetails (ChatGroup _soloChat, AdminViewModel avm)
		{
			InitializeComponent ();
            thisAdmin = avm;
            _currentChatGroup = _soloChat;
            soloUser.Text = _currentChatGroup.GroupName;
            lblCode.Text = _currentChatGroup.GroupShareId;
            if (_currentChatGroup.UserList != null)
            {
                if (_currentChatGroup.UserList.Count > 0)
                    soloUser.Text = _soloChat.UserList.Where(sc => sc.Admin != true).Select(s => s.Name).FirstOrDefault();
            } else
            {
                noUser.Text = "No user yet.";
                noUser.IsVisible = true;
                soloUser.IsVisible = false;

            }
            
        }
        private async void ShowMessages_Clicked(object sender, EventArgs e)
        {
            groupMsgs = new GroupMessages(_currentChatGroup, thisAdmin);
            // addmsg.OnNewMessages += new EventHandler(AddNewMessages);
            await Navigation.PushAsync(groupMsgs);
        }
    }
}
