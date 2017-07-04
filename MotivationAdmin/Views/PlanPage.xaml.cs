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
        //public event EventHandler OnAddMessages;
        //public event EventHandler OnScheduleMessages;
        //StackLayout fullStack;
        //private List<TodoFullItem> filteredList = new List<TodoFullItem>();
        //private List<TodoItem> multiList = new List<TodoItem>();
        //SelectMultipleBasePage<TodoItem> toDo;
        //private TodoItemManager manager;
        //Button btn2;
        public PlanPage(ChatGroup _chatGroup)
        {
            InitializeComponent();
            //manager = TodoItemManager.DefaultManager;
  
        }


    }
}
