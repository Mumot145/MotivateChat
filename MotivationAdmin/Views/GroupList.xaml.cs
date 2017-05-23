using Microsoft.WindowsAzure.MobileServices;
using MotivationAdmin.Models;
using MotivationAdmin.ViewModels;
using MotivationAdmin.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MotivationAdmin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupList : ContentPage
    {
        bool authenticated = false;
        User currentUser = new User();
        FacebookUser facebookUser = new FacebookUser();
        AzureDataService service;

        private string token;
        public GroupList(string _aToken)
        {           
            InitializeComponent();
            token = _aToken;
            //var tbi = new ToolbarItem("+", "plus.png", () =>
            //{                
            //    var todoPage = new NewUser(_chatGroup);
            //    Navigation.PushAsync(todoPage);
            //}, 0, 0);
            //tbi.Order = ToolbarItemOrder.Secondary;  // forces it to appear in menu on Android
            //ToolbarItems.Add(tbi);
            BindingContext = new List<ChatGroup>();
            service = AzureDataService.DefaultService;
            var tbi = new ToolbarItem("Add Group","", () =>
            {
                
                var newGroupPage = new NewGroup(currentUser);
                Navigation.PushAsync(newGroupPage);
            }, 0, 0);
            tbi.Order = ToolbarItemOrder.Secondary;  // forces it to appear in menu on Android
            ToolbarItems.Add(tbi);

        }
        void OnRefresh(object sender, RefreshEventArgs e)
        {
            refresh();
            groupList.IsRefreshing = false;
        }
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ChatGroup cg = (ChatGroup)e.SelectedItem;
            await Navigation.PushModalAsync(new NavigationPage(new GroupDetails(cg, currentUser)));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            groupList.ItemsSource = null;
            Debug.WriteLine("appearing checking token--" + MotiveApp.Token);
            // Refresh items only when authenticated.
            if (String.IsNullOrEmpty(token))
            {
                await Navigation.PushModalAsync(new LoginPage());
            } else
            {
                await service.GetFacebookProfileAsync(token);
                service.SetUser("fbId");
                currentUser = service.GetUser();
                if(currentUser == null)
                {
                    service.RegisterUser(facebookUser.Name, facebookUser.Id);
                    currentUser = service.GetUser();
                    if (currentUser == null)
                        return;                     
                }
                refresh();                                            
            }
        }
        void refresh()
        {
            var info = service.GetGroups(currentUser.Id);
            Debug.WriteLine("refreshing rn =="+ info.FirstOrDefault());
            if (info != null)
            {
                groupList.ItemsSource = info;
            }
        }
        public async Task GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.8/me/"
                             + "?fields=name,picture,cover,age_range,devices,email,gender,is_verified"
                             + "&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            facebookUser = JsonConvert.DeserializeObject<FacebookUser>(userJson);
        }
      
    }
}
