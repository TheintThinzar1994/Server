using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class SubDepartment
    {
        public long? Id { get; set; }

        public string Name { get; set; }
       
        public long Dept_Id { get; set; }
        
        [ForeignKey("Dept_Id")]
        public virtual Department Department { get; set; }

        public long Is_Active { get; set; }
        public DateTime ts { get; set; }
    }
}
