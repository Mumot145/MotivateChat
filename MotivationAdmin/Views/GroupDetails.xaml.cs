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
    public partial class GroupDetails : TabbedPage
    {
        AzureDataService _azure;
        User _currentUser = new User();
        ChatGroup _currentChatGroup = new ChatGroup();
        PlanPage toDo;
        AddMessages addmsg;
        List<TodoItem> selectedToDoItems = new List<TodoItem>();
        //Week _week = new Week();
        Week _week = new Week();
        List<Day> weekDays = new List<Day>();
        ScheduleMessagePage schdmsg;
        public GroupDetails(ChatGroup _chatGroup, User _user)
        {
            // toDo = new SelectMultipleBasePage<TodoItem>(_chatGroup);
            toDo = new PlanPage(_chatGroup);
            //weekDays = week.getWeek();
            InitializeComponent();
            _currentChatGroup = _chatGroup;
            BindingContext = _currentChatGroup;
           // schedList.ItemsSource = _currentChatGroup.ToDoList;
            _currentUser = _user;
            Children.Add(toDo);
            _azure = AzureDataService.DefaultService;
            //Schedule _schedule = _azure.GetSchedule(_currentChatGroup.Id.ToString());
            toDo.OnAddMessages += new EventHandler(AddMessagePage);
            toDo.OnScheduleMessages += new EventHandler(ScheduleMessagePage);
            displayList(1);
        }
        void displayList(int day)
        {
            Week wk = new Week();
            List<TodoFullItem> dList = new List<TodoFullItem>();
            schedList.ItemsSource = null;
            var getDay = wk.getDay(day).ToDo;
            todayText.Text = getDay;
            if (_currentChatGroup.ToDoList != null)
            {
                _currentChatGroup.ToDoList = _currentChatGroup.ToDoList.OrderBy(cg => cg.getTime).ToList();
                foreach (var ccg in _currentChatGroup.ToDoList)
                {

                    Console.WriteLine("ccg.Id" + ccg.AttachedToDo.Id);
                    Console.WriteLine("ccg.ToDo" + ccg.AttachedToDo.ToDo);
                   // Console.WriteLine("ccg.toDoDays" + ccg.DayStr);
                    //Console.WriteLine();
                    //Console.WriteLine();
                    if (ccg.toDoDays.Count > 0)
                    {

                        foreach (var d in ccg.toDoDays)
                        {
                            if (d.Id == day)
                            {
                                Console.WriteLine(ccg.AttachedToDo.ToDo);
                                Console.WriteLine(ccg.getTime);
                                //ccg.SendTime = ccg.SendTime.Trim();
                                dList.Add(ccg);
                            }
                        }
                        schedList.ItemsSource = dList;
                        
                    }
                }
                
            }
            
            
        }

        void OnRefresh(object sender, RefreshEventArgs e)
        {
            refresh();
            //userList.IsRefreshing = false;
        }
        async void AddMessagePage(object sender, EventArgs e)
        {
            Console.WriteLine("AddMessagePage");
            addmsg = new AddMessages(_currentChatGroup);
            addmsg.OnNewMessages += new EventHandler(AddNewMessages);
            await Navigation.PushModalAsync(addmsg);

        }
        async void ScheduleMessagePage(object sender, EventArgs e)
        {
            List<TodoFullItem> fullSelectedItems = new List<TodoFullItem>();
            Console.WriteLine("ScheduleMessagePage");
             selectedToDoItems = toDo.ProvideSelected();
            foreach(var s in selectedToDoItems)
            {
                TodoFullItem addItem = new TodoFullItem();
                addItem.AttachedToDo = new TodoItem();
                addItem.AttachedToDo = s;
                fullSelectedItems.Add(addItem);
            }
            if(selectedToDoItems.Count != 0)
            {
                schdmsg = new ScheduleMessagePage(fullSelectedItems, _currentChatGroup.Id);
                // addmsg.OnNewMessages += new EventHandler(AddNewMessages);
                await Navigation.PushModalAsync(schdmsg);
            }
                

        }
        async void OnClick(object sender, EventArgs e)
        {
            //ToolbarItem tbi = (ToolbarItem)sender;
            //this.DisplayAlert("Selected!", tbi.Name, "OK");
          //  UserPage up = new UserPage(_currentChatGroup);
           // await Navigation.PushAsync(up);
        }
        async void AddNewMessages(object sender, EventArgs e)
        {
            Console.WriteLine("in Group Details - AddNewMEssages");
            //BindingContext = _currentChatGroup;
            //_currentChatGroup.ToDoList = schdmsg.provideDates();
            
            await Navigation.PopModalAsync();
            toDo.AddToList(addmsg.returnMessages());
            //toDo.RefreshItems(true);
        }
        void refresh()
        {
            List<ChatGroup> _group = new List<ChatGroup>();
            _group.Add((ChatGroup)BindingContext);
            List<ChatGroup> cgList = _azure.GetGroupUsers(_group);
          
            // if (userList != null)
            // {
            //    userList.ItemsSource = users;
            // }
        }

        private void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine("change -");
            //weekDay
        }

        private void slider_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("change +");
        }

        private void MO_Clicked(object sender, EventArgs e)
        {
            displayList(1);
        }

        private void TU_Clicked(object sender, EventArgs e)
        {
            displayList(2);
        }
        private void WE_Clicked(object sender, EventArgs e)
        {
            displayList(3);
        }
        private void TH_Clicked(object sender, EventArgs e)
        {
            displayList(4);
        }
        private void FR_Clicked(object sender, EventArgs e)
        {
            displayList(5);
        }
        private void SA_Clicked(object sender, EventArgs e)
        {
            displayList(6);
        }
        private void SU_Clicked(object sender, EventArgs e)
        {
            displayList(0);
        }

        //async void OnDelete(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    var u = (User)mi.CommandParameter;
        //    _azure.DeleteFromGroup(u.Id, (ChatGroup)BindingContext);
        //    //DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        //    Debug.WriteLine("deleted");
        //}
        //async void OnReset(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    var u = (User)mi.CommandParameter;
        //    _azure.ResetCompletion(u.Id.ToString());
        //    //DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        //    Debug.WriteLine("complete reset");
        //}
    } 
}
