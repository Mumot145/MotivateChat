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
	public partial class MainPage : TabbedPage
	{
        List<ChatGroup> thisChatGroups = new List<ChatGroup>();
		public MainPage (List<ChatGroup> allInfo)
		{
            thisChatGroups = allInfo;
			InitializeComponent ();
            UserPage userpage = (new UserPage(thisChatGroups));
            GroupList groupPage = new GroupList(thisChatGroups.Where(cg => cg.SoloGroup == false).ToList());
            SoloChatList soloChatPage = new SoloChatList(thisChatGroups.Where(cg=>cg.SoloGroup == true).ToList());
            Children.Add(new NavigationPage(userpage));
            Children.Add(new NavigationPage(groupPage));
		}
	}
}
