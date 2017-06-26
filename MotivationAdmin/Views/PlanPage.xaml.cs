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
	public partial class PlanPage : ContentPage
	{
        public event EventHandler OnAddMessages;
        public event EventHandler OnScheduleMessages;
        StackLayout fullStack;
        private List<TodoFullItem> filteredList = new List<TodoFullItem>();
        private List<TodoItem> multiList = new List<TodoItem>();
        SelectMultipleBasePage<TodoItem> toDo;
        private TodoItemManager manager;
        Button btn2;
        public PlanPage (ChatGroup _chatGroup)
		{
			InitializeComponent ();
            manager = TodoItemManager.DefaultManager;
            fullStack = new StackLayout();
            StackLayout btnStack = new StackLayout();
            btnStack.Orientation = StackOrientation.Horizontal;
            Button btn = new Button()
            {
                Text = "Add New Messages"
            };
            btn2 = new Button()
            {
                Text = "Add To Schedule"
            };
            btnStack.Children.Add(btn);
            btnStack.Children.Add(btn2);
            fullStack.Children.Add(btnStack);
            if (_chatGroup.ToDoList != null)
            {
                filteredList = _chatGroup.ToDoList.GroupBy(td => td.AttachedToDo.Id)
                                   .Select(grp => grp.First())
                                   .ToList();
                foreach (var fl in filteredList)
                {
                    multiList.Add(fl.AttachedToDo);
                }
                toDo = new SelectMultipleBasePage<TodoItem>(multiList);
                if (multiList.Count > 0)
                {
                    fullStack.Children.Add(toDo);
                }
            }                   
            Content = fullStack;
            btn.Clicked += AddingNewMessages;
            btn2.Clicked += SendingToSchedule;
        }
        public void AddToList(List<TodoItem> addedList)
        {
            if(fullStack.Children.IndexOf(toDo) > 0){
                fullStack.Children.Remove(toDo);
            }          
            foreach (var added in addedList)
            {
                multiList.Add(added);
            }
            toDo = new SelectMultipleBasePage<TodoItem>(multiList);
            fullStack.Children.Add(toDo);
            Content = fullStack;
        }
        public List<TodoItem> ProvideSelected()
        {
            List<TodoItem> todo = toDo.GetSelection();
            return todo;
        }
        private void AddingNewMessages(object sender, EventArgs e)
        {
            OnAddMessages(this, new EventArgs());
            
        }
        private void SendingToSchedule(object sender, EventArgs e)
        {
            OnScheduleMessages(this, new EventArgs());
        }
        
    }
}
