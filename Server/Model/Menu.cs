using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Menu
    {
        [Key]
        public long? Id { get; set; }

        public string Menu_Name { get; set; }
        public string Description { get; set; }
        public long Parent_Id { get; set; }

        public DateTime Created_Date { get; set; }

        public DateTime Updated_Date { get; set; }
        public String Action { get; set; }
        public String RoutePath { get; set; }
        public String Icon { get; set; }
        public bool Is_Active { get; set; }
        public DateTime ts { get; set; }
        
    }
}
