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
	public partial class GroupUsers : ContentPage
	{
		public GroupUsers (ChatGroup _chatgroup)
		{
			InitializeComponent ();
            BindingContext = _chatgroup;

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            User selectedUser = (User)e.SelectedItem;
            UserInfo ui = new UserInfo(selectedUser);
            Navigation.PushAsync(ui);
            ((ListView)sender).SelectedItem = null;
        }
    }
}
