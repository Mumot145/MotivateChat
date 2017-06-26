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
        AzureDataService _azure;
        User _thisUser = new User();
        ChatGroup chatGroup = new ChatGroup();
        public NewGroup(User admin)
		{
			InitializeComponent();
            _thisUser = admin;
        }
        async void AddGroup(object sender, EventArgs e)
        {
            string ngChat = newGroupChat.Text;
            if (!String.IsNullOrEmpty(ngChat))
            {
                _azure.AddNewGroup(ngChat, _thisUser);
            }
            else
            {
                notFound.IsVisible = true;
                return;
            }
        }
    }
}
