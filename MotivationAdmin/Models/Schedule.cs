using System;
using System.Collections.Generic;
using System.Text;

namespace MotivationAdmin.Models
{
    public class Schedule
    {
        public List<bool> Week = new List<bool>();
        public int Id { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public List<bool> GetFullScheduleList()
        {
            Week.Add(Monday);
            Week.Add(Tuesday);
            Week.Add(Wednesday);
            Week.Add(Thursday);
            Week.Add(Friday);
            Week.Add(Saturday);
            Week.Add(Sunday);
            return Week;
        }
    }
}
