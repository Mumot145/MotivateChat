using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MotivationAdmin.Models
{
    public class AdminViewModel
    {
        private List<ChatGroup> _usersChatGroups = new List<ChatGroup>();
        private User _thisUser = new User();
        private ObservableCollection<TodoItem> _usersAllMessages = new ObservableCollection<TodoItem>();

        public List<ChatGroup> UsersChatGroups
        {
            get { return _usersChatGroups; }
            set { _usersChatGroups = value; }
        }
        public User ThisUser
        {
            get { return _thisUser; }
            set { _thisUser = value; }
        }
        public ObservableCollection<TodoItem> UsersAllMessages
        {
            get { return _usersAllMessages; }
            set { _usersAllMessages = value; }
        }
    }
}
