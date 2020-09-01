using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Department
    {
        [Key]
        public long? Id { get; set; }

        public string Name { get; set; }
        public bool Is_Active { get; set; }
        public DateTime ts { get; set; }
    }
}
