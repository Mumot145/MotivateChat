using MotivationAdmin.Models;

using MotivationAdmin.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MotivationAdmin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupDetails : ContentPage
    {
        AzureDataService _azure;
        User _currentUser = new User();
        ChatGroup _currentChatGroup = new ChatGroup();
      
        GroupMessages groupMsgs;
        GroupUsers groupUsrs;
        List<TodoItem> selectedToDoItems = new List<TodoItem>();
        //Week _week = new Week();
        Week _week = new Week();
        AdminViewModel thisAdmin = new AdminViewModel();
        List<Day> weekDays = new List<Day>();

        public GroupDetails(ChatGroup _chatGroup, AdminViewModel _thisAdmin)
        {

            InitializeComponent();
            _currentChatGroup = _chatGroup;
            BindingContext = _currentChatGroup;
            thisAdmin = _thisAdmin;

        }

        private async void ShowMessages_Clicked(object sender, EventArgs e)
        {
            groupMsgs = new GroupMessages(_currentChatGroup, thisAdmin);
           // addmsg.OnNewMessages += new EventHandler(AddNewMessages);
            await Navigation.PushAsync(groupMsgs);
        }

        private async void Users_Clicked(object sender, EventArgs e)
        {
            groupUsrs = new GroupUsers(_currentChatGroup);
            // addmsg.OnNewMessages += new EventHandler(AddNewMessages);
            await Navigation.PushAsync(groupUsrs);
        }
      
    } 
}
