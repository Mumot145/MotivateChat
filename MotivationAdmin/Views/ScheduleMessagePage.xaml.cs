﻿using MotivationAdmin.Models;
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
	public partial class ScheduleMessagePage : ContentPage
	{
        private SchedulePage selectSchedule;
        AzureDataService _azure;
        int _chatId = 0;
        private List<TodoFullItem> _todoList = new List<TodoFullItem>();
        public ScheduleMessagePage (List<TodoFullItem> toDoList, int chatId)
		{
            _chatId = chatId;
            _azure = AzureDataService.DefaultService;
            InitializeComponent ();
            _todoList = toDoList;
            foreach (var td in toDoList)
            {
                Console.WriteLine("td - " + td.AttachedToDo.ToDo);
                Console.WriteLine("td - " + td.DayStr);
            }
            //BindingContext = toDoList;
            selectedItemsList.ItemsSource = toDoList;
        }

        private async void selectedItemsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            // selectedItemsList.SelectedItem = false;
            TodoFullItem selectedFullItem = new TodoFullItem();
           // selectedFullItem.AttachedToDo = new TodoItem();
            selectedFullItem = e.SelectedItem as TodoFullItem;
            Console.WriteLine("clicking here -"+ selectedFullItem.getTime);
            selectSchedule = new SchedulePage(selectedFullItem);
            await Navigation.PushModalAsync(selectSchedule);
            selectSchedule.OnAddSchedule += new EventHandler(AddedSchedule);
            
        }
        async void AddedSchedule(object sender, EventArgs e)
        {
            TodoFullItem info = selectSchedule.ProvideSelected();

            TodoFullItem select = _todoList.First(td => td.AttachedToDo.Id == info.AttachedToDo.Id);
            var index = _todoList.IndexOf(select);

            if (index != -1)
                _todoList[index] = info;

            selectedItemsList.ItemsSource = null;
            selectedItemsList.ItemsSource = _todoList;
            await Navigation.PopModalAsync();
        }
        public List<TodoFullItem> provideDates()
        {
            return _todoList;
        }
        void AddToSchedule(object sender, EventArgs e)
        {
            var submitList = _todoList;
            int i = 0;
            int[] scheduleIds = _azure.AddScheduleToMessage(submitList);
            if(scheduleIds != null)
            {
                foreach (var si in scheduleIds)
                {
                    if (si == 0)
                        break;

                    Console.WriteLine("looking at =>" + si);
                }
                foreach (var sl in submitList)
                {
                    Console.WriteLine(scheduleIds[i] + " IS SCHEDULE ID FOR = DAY => " + sl.DayStr + "    msg => "+ sl.AttachedToDo.ToDo + "    for time =>"+sl.getTime);
                    _azure.AddDaysToSchedule(scheduleIds[i], sl.toDoDays);
                    i++;
                }
                Navigation.PopModalAsync();
            } 
            
        }
    }
}
