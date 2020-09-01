using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class User
    {
        [Key]
        public long? Id { get; set; }
        public string User_Name { get; set; }

        public string Password { get; set; }
        
        public bool isActive { get; set; }
        public DateTime Created_Date { get; set; }

        public DateTime Updated_Date { get; set; }
        public DateTime ts { get; set; }


        public long Role_ID { get; set; }
        [ForeignKey("Role_ID")]
        public virtual Role Role { get; set; }
    }
}
