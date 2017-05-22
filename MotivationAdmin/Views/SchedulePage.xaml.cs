using MotivationAdmin.Models;
using MotivationAdmin.ViewModels;
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
	public partial class SchedulePage : ContentPage
	{
        ScheduleViewModel thisGroupSchedule = new ScheduleViewModel();
        AzureDataService _azure;
		public SchedulePage (ScheduleViewModel svm)
		{
            thisGroupSchedule = svm;
            BindingContext = thisGroupSchedule;
            InitializeComponent ();
		}
        void updateSchedule(object sender, EventArgs e)
        {
            
            Schedule _schedule = new Schedule();
            _schedule.Id = thisGroupSchedule.schedule.Id;
            _schedule.Monday = Convert.ToBoolean(Monday.IsToggled);
            _schedule.Tuesday = Convert.ToBoolean(Tuesday.IsToggled);
            _schedule.Wednesday = Convert.ToBoolean(Wednesday.IsToggled);
            _schedule.Thursday = Convert.ToBoolean(Thursday.IsToggled);
            _schedule.Friday = Convert.ToBoolean(Friday.IsToggled);
            _schedule.Saturday = Convert.ToBoolean(Saturday.IsToggled);
            _schedule.Sunday = Convert.ToBoolean(Sunday.IsToggled);
            _azure.UpdateSchedule(_schedule);
        }
    }
}
