using MotivationAdmin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MotivationAdmin.Controls
{
    class SwitchText : Switch
    {

    }
    public class GroupArgs : EventArgs
    {
        private ChatGroup m_Data;
        public GroupArgs(ChatGroup data)
        {
            m_Data = data;
        }
        public ChatGroup Data
        { get { return m_Data; } }
    }
    public class UserArgs : EventArgs
    {
        private User m_Data;
        public UserArgs(User data)
        {
            m_Data = data;
        }
        public User Data
        { get { return m_Data; } }
    }
}

