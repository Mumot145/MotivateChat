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
	public partial class LoadingPage : ContentPage
	{
        AzureDataService service;
        TodoItemManager manager;
        string token = "";
        User currentUser = new User();
        public LoadingPage (string _aToken)
		{
            service = AzureDataService.DefaultService;
            manager = TodoItemManager.DefaultManager;
            token = _aToken;
            InitializeComponent ();
        }
        protected override async void OnAppearing()
        {
            if (String.IsNullOrEmpty(token))
            {
                await Navigation.PushModalAsync(new LoginPage());
            }
            else
            {
                try
                {
                    await service.GetFacebookProfileAsync(token);
                    service.SetUser("fbId");
                    currentUser = service.GetUser();
                    manager.SetUser(currentUser);
                    if (currentUser == null)
                    {
                        service.RegisterUser();
                        currentUser = service.GetUser();
                        if (currentUser == null)
                            await Navigation.PushModalAsync(new LoginPage());
                        else
                        {
                            var allTodo = await manager.GetTodoItemsAsync(true);
                            var info = service.GetAdminViewModel(currentUser.Id, allTodo.ToList());
                            info.ThisUser = currentUser;
                            info.UsersAllMessages = allTodo;
                            await Navigation.PushModalAsync(new NavigationPage(new MainPage(info)));
                        }
                            
                    }
                    else
                    {
                        var allTodo = await manager.GetTodoItemsAsync(true);
                        var info = service.GetAdminViewModel(currentUser.Id, allTodo.ToList());
                        info.ThisUser = currentUser;
                        info.UsersAllMessages = allTodo;
                        await Navigation.PushModalAsync(new NavigationPage(new MainPage(info)));
                    }
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine("ERROR IN FB LOGIN ="+e.Message);
                    return;
                }
                       
            }
        }
	}
}
