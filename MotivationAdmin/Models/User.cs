using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotivationAdmin.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FacebookId { get; set; }
        public bool Admin { get; set; }
        public string Complete
        {
            get
            {
                if (((DateTime.Now.Day - CompleteDate.Day) > 0) || CompleteDate == null)
                {
                    return "Incomplete";
                }
                else
                    return "Complete";
            }
            
        }
        public DateTime CompleteDate { get; set; }
    }
}
