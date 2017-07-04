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
        public string GroupShareId { get; set; }
        private List<User> users = new List<User>();
        private List<TodoFullItem> readyToDos = new List<TodoFullItem>();
        public event PropertyChangedEventHandler PropertyChanged;

        public List<User> UserList
        {
            get { return users; }
            set { users = value; }
        }
        public List<TodoFullItem> ReadyToDoList
        {
            get { return readyToDos; }
            set { readyToDos = value; }
        }   
        public string GroupNameUserTitle
        {
            get
            {
                return "Users for " + GroupName;
            }
        }

    }
}
