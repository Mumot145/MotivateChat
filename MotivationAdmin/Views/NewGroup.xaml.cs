using MotivationAdmin.Controls;
using MotivationAdmin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MotivationAdmin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewGroup : ContentPage
	{
        //public event EventHandler<GroupAddArgs> OnNewGroupAdded;
        AzureDataService _azure;
        User _thisUser = new User();
        ChatGroup chatGroup = new ChatGroup();
        
        public NewGroup(User admin)
		{
			InitializeComponent();
            _azure = AzureDataService.DefaultService;
            _thisUser = admin;
        }
        async void AddGroup(object sender, EventArgs e)
        {
            string ngChat = newGroupChat.Text;
            var isSolo = solo.IsToggled;
            if (!String.IsNullOrEmpty(ngChat))
            {              
                ChatGroup cg = _azure.AddNewGroup(ngChat, _thisUser, isSolo);
                await Navigation.PopAsync();
             //   OnNewGroupTwo(this, new GroupAddArgs(cg));
            }
            else
            {
                notFound.IsVisible = true;
                return;
            }
        }
    }
}
