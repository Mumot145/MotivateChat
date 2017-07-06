using MotivationAdmin.Controls;
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
	public partial class SoloChatList : ContentPage
	{
        public event EventHandler<EventArgs> OnAddSolo;
        public event EventHandler<GroupArgs> OnSelectSolo;
        AdminViewModel _thisAdmin = new AdminViewModel();
        public NewGroup newGroupPagesl;
        AzureDataService service;
        public SoloChatList (AdminViewModel thisAdmin)
		{
            //thischatGroupList = chatGroupList;
            InitializeComponent ();
            //var gl = chatGroupList.Where(cg => cg.SoloGroup == false).ToList();
            //adminViewModel = _thisAdmin;
            service = AzureDataService.DefaultService;
            _thisAdmin = thisAdmin;
            groupList.ItemsSource = _thisAdmin.UsersChatGroups.Where(cg => cg.SoloGroup == true).ToList();
        }
        
        public void updateChatGroups(AdminViewModel _adminVM)
        {
            groupList.ItemsSource = _adminVM.UsersChatGroups.Where(cg => cg.SoloGroup == true).ToList(); ;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            newGroupPagesl = new NewGroup(_thisAdmin.ThisUser);
            //    newGroupPage.OnNewGroupAdded += new EventHandler<GroupAddArgs>(AddGroups);
                await Navigation.PushAsync(newGroupPagesl);
        }

        private void groupList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ChatGroup cg = (ChatGroup)e.SelectedItem;
            OnSelectSolo?.Invoke(this, new GroupArgs(cg));
            //
            ((ListView)sender).SelectedItem = null;
        }
    }
}
