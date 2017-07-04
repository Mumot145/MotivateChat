using Microsoft.WindowsAzure.MobileServices;
using MotivationAdmin.Controls;
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
        public event EventHandler<EventArgs> OnNewGroup;
        bool authenticated = false;
        User currentUser = new User();
        public NewGroup newGroupPagegl;
        FacebookUser facebookUser = new FacebookUser();
        AzureDataService service;
       // List<ChatGroup> thisGroupList = new List<ChatGroup>();
        AdminViewModel adminViewModel = new AdminViewModel();
        private string token;
        public GroupList(AdminViewModel _thisAdmin)
        {            
            InitializeComponent();
            currentUser = _thisAdmin.ThisUser;
            var gl = _thisAdmin.UsersChatGroups.Where(cg => cg.SoloGroup == false).ToList();
            adminViewModel = _thisAdmin;
            service = AzureDataService.DefaultService;
            
            groupList.ItemsSource = gl;
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
            await Navigation.PushAsync(new GroupDetails(cg, adminViewModel));
            ((ListView)sender).SelectedItem = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            OnNewGroup?.Invoke(this, new EventArgs());
        }

        public void updateVM(AdminViewModel avm)
        {
            groupList.ItemsSource = null;
            adminViewModel = avm;
            groupList.ItemsSource = adminViewModel.UsersChatGroups.Where(cg => cg.SoloGroup == false).ToList();
        }
    }
}
