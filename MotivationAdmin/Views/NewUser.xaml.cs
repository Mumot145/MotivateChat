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
	public partial class NewUser : ContentPage
	{
        AzureDataService _azure;
        ChatGroup chatGroup = new ChatGroup();
		public NewUser (ChatGroup _chatGroup)
		{
			InitializeComponent ();
            chatGroup = _chatGroup;

        }
        async void AddMember(object sender, EventArgs e)
        {
            string ngMember = newGroupMember.Text;
            if (!String.IsNullOrEmpty(ngMember))
            {
                var user = _azure.GetUser();
                Debug.WriteLine(chatGroup.GroupName);

                if(user != null )
                {
                    //Debug.WriteLine("ID found=" + id);
                    _azure.AddUserToGroup(user, chatGroup.Id);
                }
                
            } else
            {
                Debug.WriteLine("user not found");
                notFound.IsVisible = true;
                return;
            }
            
              
        }


    }
}
