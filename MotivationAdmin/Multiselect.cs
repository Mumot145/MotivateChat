using System;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using MotivationAdmin.Views;
using MotivationAdmin.Models;
using System.Threading.Tasks;

namespace MotivationAdmin
{
    public class SelectMultipleBasePage<T> : ContentView
    {
        public class WrappedSelection<T> : INotifyPropertyChanged
        {
            public T Item { get; set; }

            bool isSelected = false;
            public bool IsSelected
            {
                get
                {
                    return isSelected;
                }
                set
                {
                    if (isSelected != value)
                    {
                        isSelected = value;
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                    }
                }
            }
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }

        public class WrappedItemSelectionTemplate : ViewCell
        {
            public WrappedItemSelectionTemplate() : base()
            {
                Label name = new Label();         
                name.SetBinding(Label.TextProperty, new Binding("Item.ToDo"));

                Label todoLbl = new Label();
                todoLbl.SetBinding(Label.TextProperty, new Binding("Item.getLabelInfo.MsgLabel"));

                BoxView boxLbl = new BoxView();
                boxLbl.SetBinding(BoxView.ColorProperty, new Binding("Item.getLabelInfo.Clr"));

                Switch mainSwitch = new Switch();
                mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));

                RelativeLayout layout = new RelativeLayout();
                layout.Children.Add(name,
                    Constraint.Constant(5),
                    Constraint.Constant(5),
                    Constraint.RelativeToParent(p => p.Width - 60),
                    Constraint.RelativeToParent(p => p.Height - 10)
                );

                layout.Children.Add(boxLbl,
                    Constraint.RelativeToView(todoLbl, (l, v) => v.X - 1),
                    Constraint.RelativeToView(todoLbl, (l, v) => v.Y - 1),
                    Constraint.RelativeToView(todoLbl, (l, v) => v.Width + 2),
                    Constraint.RelativeToView(todoLbl, (l, v) => v.Height + 2)
                );
                layout.Children.Add(todoLbl,
                    Constraint.Constant(15),
                    Constraint.RelativeToView(name, (l, v) => v.Height - 10),
                    null,
                    Constraint.RelativeToParent(p => p.Height - 5)
                );
                layout.Children.Add(mainSwitch,
                    Constraint.RelativeToParent(p => p.Width - 55),
                    Constraint.Constant(5),
                    Constraint.Constant(50),
                    Constraint.RelativeToParent(p => p.Height - 20)
                );
                //todoLbl.Margin = new Thickness(0,0,0,10);

                View = layout;
                
            }
        }

        public List<WrappedSelection<T>> WrappedItems = new List<WrappedSelection<T>>();
        ListView mainList = new ListView();
        public SelectMultipleBasePage(List<T> items)
        {

            //T items = (T)Convert.ChangeType(_chatGroup.ToDoList, typeof(T));
            WrappedItems = items.Select(item => new WrappedSelection<T>() { Item = item, IsSelected = false }).ToList();
            mainList = new ListView()
            {
                ItemsSource = WrappedItems,
                ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate))
            };

            Content = mainList;

            //ToolbarItems.Add(new ToolbarItem("All", null, SelectAll, ToolbarItemOrder.Primary));
           // ToolbarItems.Add(new ToolbarItem("None", null, SelectNone, ToolbarItemOrder.Primary));
        }
 
        //public async void RefreshItems(bool syncItems)
        //{      
        //    var items = await manager.GetTodoItemsAsync(syncItems);
        //    WrappedItems = items.Select(item => new WrappedSelection<TodoItem>() { Item = item, IsSelected = false }).ToList();
        //    ListView mainList2 = new ListView()
        //    {
        //        ItemsSource = WrappedItems,
        //        ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate))
        //    };
        //    stack.Children.Remove(mainList);           
        //    stack.Children.Add(mainList2);
        //    mainList = mainList2;
        //}

        void SelectAll()
        {
            foreach (var wi in WrappedItems)
                wi.IsSelected = true;
        }

        void SelectNone()
        {
            foreach (var wi in WrappedItems)
                wi.IsSelected = false;
        }

        public List<T> GetSelection()
        {
            foreach(var w in WrappedItems)
            {
                Console.WriteLine(w.Item+"----"+w.IsSelected);
            }
            
            return WrappedItems.Where(item => item.IsSelected).Select(wrappedItem => wrappedItem.Item).ToList();
        }
    }
}
