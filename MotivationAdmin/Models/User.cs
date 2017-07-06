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
        public int Location { get; set; }
        public int Age { get; set; }

        public string Name { get; set; }
        public string FacebookId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        private string Password { get; set; }
               
        public bool Gender { get; set; }       
        public bool Admin { get; set; }
        public bool AttachedGroup { get; set; }
        public string filterName {
            get {
                if (Name == "") { return Email; }
                else { return Name; }
            }
        }
        public bool GuessPass(string passGuess)
        {
            if(passGuess == Password)            
                return true;
             else            
                return false;           
        }

        public void setPass(string pass)
        {
            Password = pass; 
        }

        public string getGroupType
        {
            get
            {
                if (AttachedGroup == true)
                {
                    return "solo.png";
                }
                else
                {
                    return "group.png";
                }
            }
        }

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
