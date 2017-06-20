using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotivationAdmin.Models
{
    public class TodoFullItem
    {
        TimeSpan time;
        string dayStr = "EMPTY";
        List<Day> todoDays = new List<Day>();

        public int ScheduleId { get; set; }
        public TodoItem AttachedToDo { get; set; }

        public List<Day> toDoDays
        {
            get { return todoDays; }
            set { todoDays = value; }
        }
        public String getTime
        {
            get { return Convert.ToString(time); }
        }
        public TimeSpan SendTimeSpan
        {
            get { return time; }
            set { time = value; }
        }
        public String DayStr
        {
            get
            {
                if (todoDays.Count > 0)
                {
                    dayStr = "";
                    foreach (var day in todoDays)
                    {
                        dayStr = dayStr + day.ToDo + ", ";
                    }
                    dayStr = dayStr.Remove(dayStr.Length - 2);
                }
                return dayStr;
            }

        }
        public Day getDay(int id)
        {           
            return todoDays.First(td => td.Id == id);
        }
    }
}
