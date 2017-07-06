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
        private List<User> _pendingUsers = new List<User>();
        private ObservableCollection<TodoItem> _usersAllMessages = new ObservableCollection<TodoItem>();

        public List<ChatGroup> UsersChatGroups
        {
            get { return _usersChatGroups; }
            set { _usersChatGroups = value; }
        }
        public List<User> PendingUsers
        {
            get { return _pendingUsers; }
            set { _pendingUsers = value; }
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
