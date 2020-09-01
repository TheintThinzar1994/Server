using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class ThankCard
    {
        public long? Id { get; set; }
        public long From_Employee_Id { get; set; }
        public long To_Employee_Id { get; set; }
        public string Title { get; set; }
        public string SendText { get; set; }
        public DateTime SendDate { get; set; }

        public string ReplyText { get; set; }
        public DateTime ReplyDate { get; set; }
        public string Status { get; set; }

        public bool isActive { get; set; }
        public DateTime ts{ get; set; }
    }
}
