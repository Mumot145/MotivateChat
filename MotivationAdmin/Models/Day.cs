using System;
using System.Collections.Generic;
using System.Text;

namespace MotivationAdmin.Models
{
    public class Day
    {
        private int _Id;
        private String _ToDo;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        public String ToDo
        {
            get { return _ToDo; }
            set { _ToDo = value; }
        }

    }
}
