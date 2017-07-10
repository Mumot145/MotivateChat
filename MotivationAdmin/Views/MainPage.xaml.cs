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
	public partial class MainPage : TabbedPage
	{
        AdminViewModel thisAdmin = new AdminViewModel();
        TodoList messagesPage;
        GroupList groupPage;
        SoloChatList soloChatPage;
        NewGroup newGroupPage;
        [System.Obsolete("Use RuntimePlatform instead.")]
        public MainPage (AdminViewModel allInfo)
		{
            BindingContext = allInfo;
			InitializeComponent ();
            thisAdmin = allInfo;
            messagesPage = new TodoList(thisAdmin);
            UserPage userpage = (new UserPage(thisAdmin));
            userpage.OnEditUser += new EventHandler<UserArgs>(editUser);
            groupPage = new GroupList(thisAdmin);
            groupPage.OnSelectGroup += new EventHandler<GroupArgs>(selectGroup);
            groupPage.OnNewGroup += new EventHandler<EventArgs>(addingGroups);
            soloChatPage = new SoloChatList(thisAdmin);
            soloChatPage.OnAddSolo += new EventHandler<EventArgs>(addingGroups);
            soloChatPage.OnSelectSolo += new EventHandler<GroupArgs>(selectSolo);
            Children.Add(userpage);
            Children.Add(groupPage);
            Children.Add(soloChatPage);
        }
        protected override void OnAppearing()
        {
            foreach(var nav in Navigation.NavigationStack)
            {
                Console.WriteLine("looking at =>"+nav.Parent);
            }
            Console.WriteLine("OUR STACK IS AT :"+ Navigation.NavigationStack.Count);
        }
        //Navigation.NavigationStack.Count
        void addingGroups(object sender, EventArgs e)
        {
            Console.WriteLine("would add group");
            //newGroupPage = new NewGroup(thisAdmin.ThisUser);
           // newGroupPage.OnNewGroupAdded += new EventHandler<GroupAddArgs>(AddGroups);
            //await Navigation.PushAsync(newGroupPage);
        }
        //UserInfo ui = new UserInfo(selectedUser);
        //await Navigation.PushAsync(ui);
        async void selectGroup(object sender, GroupArgs e)
        {
            Console.WriteLine("info =>" + e.Data.GroupName);
            GroupDetails selectedGroup = new GroupDetails(e.Data, thisAdmin);
            await Navigation.PushAsync(selectedGroup);
        }
        async void selectSolo(object sender, GroupArgs e)
        {
            Console.WriteLine("info =>" + e.Data.GroupName);
            SoloDetails selectedSolo = new SoloDetails(e.Data, thisAdmin);
            await Navigation.PushAsync(selectedSolo);
        }
        async void editUser(object sender, UserArgs e)
        {
            Console.WriteLine("info =>" + e.Data.Name);
            UserInfo selectedSolo = new UserInfo(e.Data);
            await Navigation.PushAsync(selectedSolo);
            //await Navigation.PushAsync(new GroupDetails(e.Data, thisAdmin));
        }
        //retVM()
        //thisAdmin = retVM();
       
        private async void Messages_Clicked(object sender, EventArgs e)
        {
            
            //messagesPage.OnNewMessages += new EventHandler(AddMessagePage);
            // toDo.OnAddMessages += new EventHandler(AddMessagePage);
            await Navigation.PushAsync(messagesPage);
        }
        void AddMessagePage(object sender, EventArgs e)
        {
            thisAdmin = messagesPage.retVM();
            //Console.WriteLine("AddMessagePage");
            //addmsg = new AddMessages(_currentChatGroup);
            //addmsg.OnNewMessages += new EventHandler(AddNewMessages);
            //await Navigation.PushModalAsync(addmsg);

        }
    }
}
