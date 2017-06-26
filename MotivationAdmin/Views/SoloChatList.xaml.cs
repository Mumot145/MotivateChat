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
        List<ChatGroup> thischatGroupList = new List<ChatGroup>();

        public SoloChatList (List<ChatGroup> chatGroupList)
		{
            thischatGroupList = chatGroupList;
            InitializeComponent ();
		}
	}
}
