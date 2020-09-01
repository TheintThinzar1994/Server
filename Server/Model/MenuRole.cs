using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class MenuRole
    {
        [Key]
        public long? Id { get; set; }
        
        public DateTime Created_Date { get; set; }

        public DateTime Updated_Date { get; set; }
        public bool isActive { get; set; }
        public DateTime ts { get; set; }

      
        public long Menu_Id { get; set; }
        [ForeignKey("Menu_Id")]
        public virtual Menu Menu { get; set; }

        public long Role_Id { get; set; }
       
        [ForeignKey("Role_Id")]
        public virtual Role Role { get; set; }
    }
}
