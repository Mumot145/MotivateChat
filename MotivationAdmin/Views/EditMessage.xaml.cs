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
	public partial class EditMessage : ContentPage
	{
		public EditMessage (TodoFullItem editTodo)
		{
			InitializeComponent ();
            todo.Text = editTodo.AttachedToDo.ToDo;
            pkr.ItemsSource = new MessageLabels().AllLabels;
            pkr.SelectedIndex = editTodo.AttachedToDo.MessageLabel;
        }
	}
}
