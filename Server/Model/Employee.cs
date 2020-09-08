using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Employee
    {
        [Key]
        public long? Id { get; set; }
        public string User_Name { get; set; }

        public long Sub_Dept_Id { get; set; }
        [ForeignKey("Sub_Dept_Id")]
        public virtual SubDepartment SubDepartment { get; set; }

        public long Dept_Id { get; set; }
        [ForeignKey("Dept_Id")]
        public virtual Department Department { get; set; }

        public long User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual User User { get; set; }


        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string PhotoName { get; set; }

        public DateTime Created_Date { get; set; }

        public DateTime End_Date { get; set; }
        public bool isActive { get; set; }
        public DateTime ts { get; set; }

}
}
