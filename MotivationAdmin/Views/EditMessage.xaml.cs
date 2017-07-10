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
	public partial class EditMessage : ContentPage
	{
        public event EventHandler OnEditMessage;
        TodoItemManager _manager;
        TodoFullItem thisItem = new TodoFullItem();
        TodoFullItem fixedItem = new TodoFullItem();
		public EditMessage (TodoFullItem editTodo)
		{
			InitializeComponent ();
            _manager = TodoItemManager.DefaultManager;
            thisItem = editTodo;
            var save = new ToolbarItem() { Text = "Save" };
            save.Clicked += OnSaveClicked;
            this.ToolbarItems.Add(save);
            todo.Text = editTodo.AttachedToDo.ToDo;
            pkr.ItemsSource = new MessageLabels().AllLabels.Select(lbl=>lbl.MsgLabel).ToList();
            pkr.SelectedIndex = editTodo.AttachedToDo.MessageLabel;
        }
        async void OnSaveClicked(object sender, EventArgs e)
        {
            TodoItem saveTodo = new TodoItem();
            saveTodo = thisItem.AttachedToDo;
            fixedItem = thisItem;
            saveTodo.ToDo = todo.Text;
            fixedItem.AttachedToDo.ToDo = todo.Text;
            saveTodo.MessageLabel = pkr.SelectedIndex;
            fixedItem.AttachedToDo.MessageLabel = pkr.SelectedIndex;
            await _manager.SaveTaskAsync(saveTodo);
            OnEditMessage?.Invoke(this, new EventArgs());
            await Navigation.PopAsync();
            Console.WriteLine("save");
        }
        public TodoFullItem returnFixItem
        {
            get { return fixedItem; }
            set { fixedItem = value; }
        }
    }
}
