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
	public partial class GroupMessages : ContentPage
	{
        ChatGroup thisGroup = new ChatGroup();
        AzureDataService azure;
        AdminViewModel thisAdmin = new AdminViewModel();
        DatePicker selectedDT = new DatePicker();
        SchedulePage selectedMessage;
        public GroupMessages (ChatGroup _thisGroup, AdminViewModel _thisAdmin)
		{
            InitializeComponent();
            // dateTodo.ItemsSource = null;
            azure = AzureDataService.DefaultService;
            thisAdmin = _thisAdmin;
            thisGroup = _thisGroup;
             selectedDT = dpicker;
            //Console.WriteLine("selected =>"+selectedDT.ToString());
            //if (_thisGroup.ReadyToDoList.Count > 0)
            //    dateTodo.ItemsSource = _thisGroup.ReadyToDoList;
            InitializeComponent();
            if (_thisGroup.ReadyToDoList.Count > 0)
            {
                dateTodo.ItemsSource = _thisGroup.ReadyToDoList;
            }
           
			
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            selectedMessage = new SchedulePage(thisAdmin.UsersAllMessages.ToList(), selectedDT);
            //groupMsgs = new PlanPage(thisGroup);
            selectedMessage.OnAddSchedule += new EventHandler(AddNewMessages);
            await Navigation.PushAsync(selectedMessage);
        }
        async void AddNewMessages(object sender, EventArgs e)
        {
            dateTodo.ItemsSource = null;
            Console.WriteLine("in Group Details - AddNewMEssages");
            //BindingContext = _currentChatGroup;
            //_currentChatGroup.ToDoList = schdmsg.provideDates();
            List<TodoItem> sm = selectedMessage.ProvideSelected();
            List<TodoFullItem> fsm = new List<TodoFullItem>();
            foreach (var m in sm)
            {
                TodoFullItem tfi = new TodoFullItem();
                tfi.AttachedToDo = m;
                tfi.SendDateTime = selectedDT.Date;
                tfi.SendTimeSpan = TimeSpan.FromHours(12);
                //tfi.SendTimeSpan.Date = ;
                fsm.Add(tfi);
                thisGroup.ReadyToDoList.Add(tfi);
            }

            if (thisGroup.ReadyToDoList.Count > 0)
            {
                displayList(selectedDT);
               // dateTodo.ItemsSource = thisGroup.ReadyToDoList;
                azure.AddMessagesToDay(fsm, thisGroup);
            }
                

            await Navigation.PopAsync();
            // toDo.AddToList(addmsg.returnMessages());
            //toDo.RefreshItems(true);
        }

        private void dpicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var get = (DatePicker)sender;
            selectedDT = get;
            displayList(selectedDT);
            Console.WriteLine("selected =>" + get.Date.Year + "/"+ get.Date.Month + "/"+ get.Date.Day);
        }

        private void displayList(DatePicker dp)
        {
            dateTodo.ItemsSource = thisGroup.ReadyToDoList.Where(rl => rl.SendDateTime.Date == dp.Date);
        }

        private void Up_Clicked(object sender, EventArgs e)
        {
            selectedDT.Date = selectedDT.Date.AddDays(1);
            displayList(selectedDT);
        }
        private void Down_Clicked(object sender, EventArgs e)
        {
            selectedDT.Date = selectedDT.Date.AddDays(-1);
            displayList(selectedDT);
        }
    }
}
