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
        SelectMultipleBasePage<Day> selectPage;
        TodoFullItem thisItem = new TodoFullItem();
        public SchedulePage (TodoFullItem _todoItem)
		{
            thisItem = _todoItem;
            Week week = new Week();
            InitializeComponent();
            List<Day> days = new List<Day>();
            StackLayout stack = new StackLayout();
            Button btn = new Button();
            btn.Text = "Add Days To Message";
            selectPage = new SelectMultipleBasePage<Day>(week.aWeek);
            stack.Children.Add(selectPage);
            stack.Children.Add(btn);
            btn.Clicked += AddingDaysToMessage;
            Content = stack;

        }
        private void AddingDaysToMessage(object sender, EventArgs e)
        {
            OnAddSchedule(this, new EventArgs());
        }
        public TodoFullItem ProvideSelected()
        {
            List<Day> days = selectPage.GetSelection();
            thisItem.toDoDays = days;
            return thisItem;
        }     
    }
}
