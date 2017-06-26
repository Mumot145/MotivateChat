using Microsoft.WindowsAzure.MobileServices;
using MotivationAdmin.Models;
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
        List<ChatGroup> thisGroupList = new List<ChatGroup>();
        private string token;
        public GroupList(List<ChatGroup> _thisGroupList)
        {           
            InitializeComponent();
            thisGroupList = _thisGroupList;
            service = AzureDataService.DefaultService;
            var tbi = new ToolbarItem("Add Group","", () =>
            {            
                var newGroupPage = new NewGroup(currentUser);
                Navigation.PushAsync(newGroupPage);
            }, 0, 0);
            tbi.Order = ToolbarItemOrder.Primary;  // forces it to appear in menu on Android
            ToolbarItems.Add(tbi);
            groupList.ItemsSource = thisGroupList;
        }
        void OnRefresh(object sender, RefreshEventArgs e)
        {
            groupList.IsRefreshing = false;
        }
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ChatGroup cg = (ChatGroup)e.SelectedItem;
            await Navigation.PushAsync(new GroupDetails(cg, currentUser));
            ((ListView)sender).SelectedItem = null;
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
