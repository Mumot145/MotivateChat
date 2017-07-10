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
        List<TodoFullItem> oldList = new List<TodoFullItem>();
        SchedulePage selectedMessage;
        EditMessage em;
        public GroupMessages (ChatGroup _thisGroup, AdminViewModel _thisAdmin)
		{
            InitializeComponent();
            // dateTodo.ItemsSource = null;
            azure = AzureDataService.DefaultService;
            var save = new ToolbarItem() { Text = "Save" };
            save.Clicked += OnSaveClicked;
            this.ToolbarItems.Add(save);

            thisAdmin = _thisAdmin;
            thisGroup = _thisGroup;
             selectedDT = dpicker;
            //Console.WriteLine("selected =>"+selectedDT.ToString());
            //if (_thisGroup.ReadyToDoList.Count > 0)
            //    dateTodo.ItemsSource = _thisGroup.ReadyToDoList;
            InitializeComponent();
            if (_thisGroup.ReadyToDoList.Count > 0)
            {
                
                oldList = thisGroup.ReadyToDoList.OrderBy(x=>x.SendTimeSpan).ToList();
                dateTodo.ItemsSource = oldList;
                //BindingContext = _thisGroup.ReadyToDoList;
            }
           
			
		}
        void OnSaveClicked(object sender, EventArgs e)
        {
            List<TodoFullItem> newList = new List<TodoFullItem>();
            foreach(var s in dateTodo.ItemsSource)
            {
                var td = (TodoFullItem)s;
                Console.WriteLine("cehcktime----->"+td.ScheduleId +"---"+td.SendTimeSpan);
                newList.Add(td);
            }
             azure.EditMessageTimes(oldList, newList);
            dateTodo.ItemsSource = null;
            dateTodo.ItemsSource = newList.OrderBy(n=>n.SendTimeSpan);
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
            dateTodo.ItemsSource = thisGroup.ReadyToDoList.Where(rl => rl.SendDateTime.Date == dp.Date).OrderBy(x=>x.SendTimeSpan);
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
        private async void OnEdit(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            //DisplayAlert("Delete Context Action",  + " delete context action", "OK");
            var selected = (TodoFullItem)mi.CommandParameter;
            em = new EditMessage(selected);
            em.OnEditMessage += Em_OnEditMessage;
            await Navigation.PushAsync(em);
            //
            //azure.EditGroupMessage(selected);
            //oldList.Remove(selected);
            //dateTodo.ItemsSource = oldList.OrderBy(x => x.SendTimeSpan);
        }

        private void Em_OnEditMessage(object sender, EventArgs e)
        {
            var newItem = em.returnFixItem;
            bool chk = false;
            var check = thisGroup.ReadyToDoList.Where(tg => tg.AttachedToDo.Id == newItem.AttachedToDo.Id).Select(x => { x = newItem; return true; });
            foreach(var c in check)
            {
                if (c == true)
                    chk = true;
                    
            }
            if (chk == true)
                displayList(selectedDT);
            else
                Console.WriteLine("shiiiit");
        }

        private void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            //DisplayAlert("Delete Context Action",  + " delete context action", "OK");
            var selected = (TodoFullItem)mi.CommandParameter;
            azure.DeleteMessageFromGroup(selected);

            thisGroup.ReadyToDoList.Remove(selected);
            //dateTodo.ItemsSource = null;
           // dateTodo.ItemsSource = oldList.OrderBy(x => x.SendTimeSpan);
            //selectedDT.Date = selectedDT.Date.AddDays(-1);
            displayList(selectedDT);
        }

        private void dateTodo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            ((ListView)sender).SelectedItem = null;
        }
    }
}
