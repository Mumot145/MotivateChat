using MotivationAdmin.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MotivationAdmin
{
    public partial class TodoList : ContentPage
    {
        TodoItemManager manager;
        ChatGroup thisChatGroup;
        
        [System.Obsolete("Use RuntimePlatform instead.")]
        public TodoList(ChatGroup _chatGroup)
        {
            InitializeComponent();
            thisChatGroup = _chatGroup;
            todoList.ItemsSource = _chatGroup.ToDoList;
            BindingContext = _chatGroup.ToDoList;

            manager = TodoItemManager.DefaultManager;
            manager.SetGroup(_chatGroup);
            Console.WriteLine("starting todo");
            // OnPlatform<T> doesn't currently support the "Windows" target platform, so we have this check here.
            if (manager.IsOfflineEnabled &&
                (Xamarin.Forms.Device.OS == TargetPlatform.Windows || Xamarin.Forms.Device.OS == TargetPlatform.WinPhone))
            {
                var syncButton = new Button
                {
                    Text = "Sync items",
                    HeightRequest = 30
                };
                syncButton.Clicked += OnSyncItems;

                buttonsPanel.Children.Add(syncButton);
            }
            // Track whether the user has authenticated.

        }
        
       

        // Data methods
        async Task AddItem(TodoItem item)
        {
            await manager.SaveTaskAsync(item);
            todoList.ItemsSource = await manager.GetTodoItemsAsync();
        }

        async Task DeleteItem(TodoItem item)
        {
            item.Deleted = true;
            await manager.SaveTaskAsync(item);
            todoList.ItemsSource = await manager.GetTodoItemsAsync();
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            var todo = new TodoItem { ToDo = newItemName.Text, GroupId = thisChatGroup.Id.ToString(), SendTime = newItemTime.Time.ToString()};
            await AddItem(todo);
           // Console.WriteLine("we need to add this time "+newItemTime.Time);
            newItemName.Text = string.Empty;
            newItemName.Unfocus();
        }

        // Event handlers
        [System.Obsolete("Use RuntimePlatform instead.")]
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var todo = e.SelectedItem as TodoItem;
            
            if (Xamarin.Forms.Device.OS != TargetPlatform.iOS && todo != null)
            {
                // Not iOS - the swipe-to-delete is discoverable there
                if (Xamarin.Forms.Device.OS == TargetPlatform.Android)
                {
                    await DisplayAlert(todo.ToDo, "Press-and-hold to delete task " + todo.ToDo, "Got it!");
                }
                else
                {
                    // Windows, not all platforms support the Context Actions yet
                    if (await DisplayAlert("Delete Message?", "Do you wish to delete " + todo.ToDo + " at " + todo.SendTime+"?", "Delete", "Cancel"))
                    {
                        await DeleteItem(todo);
                    }
                }
            }

            // prevents background getting highlighted
            todoList.SelectedItem = null;
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#context
        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var todo = mi.CommandParameter as TodoItem;
            await DeleteItem(todo);
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true, true);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
               
                todoList.ItemsSource = await manager.GetTodoItemsAsync(syncItems);
            }
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }
    }
}

