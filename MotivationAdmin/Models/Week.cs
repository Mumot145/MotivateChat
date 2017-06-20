using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotivationAdmin.Models
{
    public class Week
    {
        public List<Day> aWeek = new List<Day> {
            new Day(){ Id = 0 , ToDo = "Sunday" },
            new Day(){ Id = 1 , ToDo = "Monday" },
            new Day(){ Id = 2 , ToDo = "Tuesday" },
            new Day(){ Id = 3 , ToDo = "Wednesday" },
            new Day(){ Id = 4 , ToDo = "Thursday" },
            new Day(){ Id = 5 , ToDo = "Friday" },
            new Day(){ Id = 6 , ToDo = "Saturday" }
        };

        public Day getDay(int id)
        {
            return aWeek.First(w => w.Id == id);
        }
    }
}
