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
    public class GroupAddArgs : EventArgs
    {
        private ChatGroup m_Data;
        public GroupAddArgs(ChatGroup data)
        {
            m_Data = data;
        }
        public ChatGroup Data
        { get { return m_Data; } }
    }
}

