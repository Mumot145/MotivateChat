using MotivationAdmin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MotivationAdmin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddMessages : ContentPage
	{
        public event EventHandler OnNewMessages;
        private List<TodoItem> MessageList = new List<TodoItem>();
        private AzureDataService _azure;
        private TodoItemManager _todo;
        TodoItem messageAdd;
        ChatGroup _chatGroup = new ChatGroup();
		public AddMessages (ChatGroup chatGroup)
		{
            _chatGroup = chatGroup;

            InitializeComponent ();
            _todo = TodoItemManager.DefaultManager;
            _todo.SetGroup(_chatGroup);
            
        }
        private async void AttachMessages(object sender, EventArgs e)
        {
            Exception error = null;
            try
            {
                foreach(var msg in MessageList)
                {
                    await _todo.SaveTaskAsync(msg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex +"+ex);
                error = ex;
            }
            finally
            {
                Console.WriteLine("out");

                // await Navigation.PopAsync();
                OnNewMessages(this, new EventArgs());
                var result = await _todo.GetTodoItemsAsync(true);
                Console.WriteLine(result);
            }
           
          //  }
            Console.WriteLine("saved");
        }
        
        //private async void RefreshItems(bool syncItems)
        //{

        //    todoList.ItemsSource =  await _todo.GetTodoItemsAsync(syncItems);

        //}
        public List<TodoItem> returnMessages()
        {
            return MessageList;
        }

        void AddAnotherMessage(object sender, EventArgs e)
        {
            messageAdd = new TodoItem();           
            todoList.ItemsSource = null;
            if (!String.IsNullOrEmpty(newItemName.Text))
            {
                messageAdd.ToDo = newItemName.Text;
                messageAdd.GroupId = Convert.ToString(_chatGroup.Id);
                MessageList.Add(messageAdd);
                messageAdd = null;
            }
            todoList.ItemsSource = MessageList;
        }
    }
}
