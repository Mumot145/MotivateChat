using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MotivationAdmin.Models
{
    public class ChatGroup : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public bool SoloGroup { get; set; }
        private List<User> users = new List<User>();
        private List<TodoFullItem> allToDos = new List<TodoFullItem>();
        
        public event PropertyChangedEventHandler PropertyChanged;

        public List<User> UserList
        {
            get { return users; }
            set { users = value; }
        }
        public List<TodoFullItem> ToDoList
        {
            get { return allToDos; }
            set { allToDos = value; }
        }   
    }
}
