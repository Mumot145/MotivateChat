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
        public MainPage (AdminViewModel allInfo)
		{
            BindingContext = allInfo;
			InitializeComponent ();
            thisAdmin = allInfo;
            UserPage userpage = (new UserPage(thisAdmin.UsersChatGroups));
            groupPage = new GroupList(thisAdmin);
            //groupPage.OnNewGroup += new EventHandler<EventArgs>(addingGroups);
            soloChatPage = new SoloChatList(thisAdmin);
            //soloChatPage.OnAddGroup += new EventHandler<EventArgs>(addingGroups);
            Children.Add(userpage);
            Children.Add(groupPage);
            Children.Add(soloChatPage);
        }
        protected override void OnAppearing()
        {
            Console.WriteLine("OUR STACK IS AT :"+ Navigation.NavigationStack.Count);
        }
        //Navigation.NavigationStack.Count
        //async void addingGroups(object sender, EventArgs e)
        //{
        //    newGroupPage = new NewGroup(thisAdmin.ThisUser);
        //    newGroupPage.OnNewGroupAdded += new EventHandler<GroupAddArgs>(AddGroups);
        //    await Navigation.PushAsync(newGroupPage);
        //}

        //void AddGroups(object sender, GroupAddArgs e)
        //{
        //    Console.WriteLine("info =>"+e.Data.GroupName);

        // }
        //retVM()
        //thisAdmin = retVM();
        [System.Obsolete("Use RuntimePlatform instead.")]
        private async void Messages_Clicked(object sender, EventArgs e)
        {
            messagesPage = new TodoList(thisAdmin);
            messagesPage.OnNewMessages += new EventHandler(AddMessagePage);
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
