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
	public partial class SchedulePage : ContentPage
	{
        private string[] dayNames = new string[7];
        public event EventHandler OnAddSchedule;
        SelectMultipleBasePage<TodoItem> selectPage;
        List<TodoItem> _fullList = new List<TodoItem>();
        DatePicker _thisDate = new DatePicker();
        public SchedulePage (List<TodoItem> fullItemList, DatePicker dPicker)
		{
            _fullList = fullItemList;
            _thisDate = dPicker;
           // Week week = new Week();
            InitializeComponent();
           // List<Day> days = new List<Day>();
            StackLayout stack = new StackLayout();
            Button btn = new Button();
            btn.Text = "Add Messages to "+dPicker.Date.Year+"/"+ dPicker.Date.Month+"/"+ dPicker.Date.Day;
            selectPage = new SelectMultipleBasePage<TodoItem>(_fullList);
            stack.Children.Add(selectPage);
            stack.Children.Add(btn);
            selectPage.VerticalOptions = LayoutOptions.StartAndExpand;
            btn.VerticalOptions = LayoutOptions.End;
            btn.Clicked += AddingMessagesToDay;
            Content = stack;

        }
        private void AddingMessagesToDay(object sender, EventArgs e)
        {
            OnAddSchedule(this, new EventArgs());
        }
        
        public List<TodoItem> ProvideSelected()
        {
            List<TodoItem> selectedList = selectPage.GetSelection();
           // thisItem.toDoDays = days;
            return selectedList;
        }     
    }
}
