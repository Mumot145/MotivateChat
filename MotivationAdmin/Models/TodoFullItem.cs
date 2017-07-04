using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotivationAdmin.Models
{
    public class TodoFullItem
    {
        DateTime dtime;
        TimeSpan tspan;
        //List<Day> todoDays = new List<Day>();
        public int groupId { get; set; }
        public int ScheduleId { get; set; }
        public TodoItem AttachedToDo { get; set; }
        
        //public List<Day> toDoDays
        //{
        //    get { return todoDays; }
        //    set { todoDays = value; }
        //}
        public String getdTime
        {
            get { return Convert.ToString(dtime); }
        }
        public DateTime SendDateTime
        {
            get { return dtime; }
            set { dtime = value; tspan = value.TimeOfDay; }
        }
        public TimeSpan SendTimeSpan
        {
            get { return tspan;  }
            set { tspan = value; }
        }
        //public String DayStr
        //{
        //    get
        //    {
        //        if (todoDays.Count > 0)
        //        {
        //            dayStr = "";
        //            foreach (var day in todoDays)
        //            {
        //                dayStr = dayStr + day.ToDo + ", ";
        //            }
        //            dayStr = dayStr.Remove(dayStr.Length - 2);
        //        }
        //        return dayStr;
        //    }

        //}
        //public Day getDay(int id)
        //{           
        //    return todoDays.First(td => td.Id == id);
        //}
    }
    
}
