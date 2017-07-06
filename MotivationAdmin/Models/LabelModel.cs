using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotivationAdmin.Models
{
    public class MessageLabels
    {
        public List<Lbl> AllLabels = new List<Lbl>() {
            new Lbl() {Id = 0,  MsgLabel = "Morning", Clr="Yellow"},
            new Lbl() {Id = 1,  MsgLabel = "Afternoon", Clr="Green"},
            new Lbl() {Id = 2,  MsgLabel = "Night", Clr="Red"},
            new Lbl() {Id = 3,  MsgLabel = "Congratulations",  Clr="Orange"}
        };
        public Lbl getLabelWithId(int lblId)
        {
            return AllLabels.Where(al => al.Id == lblId).FirstOrDefault();
        }

    }
    public class Lbl
    {
        private string msg { get; set; }
        public int Id { get; set; }
        public string MsgLabel { get { return msg.Trim(); }  set { msg = value; } }
        public string Clr { get; set; }
    }
}
