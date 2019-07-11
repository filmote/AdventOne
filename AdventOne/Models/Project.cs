using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace AdventOne.Models {

    public enum Status {
        Lost,
        [Display(Name = "Pipe Dream")]
        PipeDream,
        [Display(Name = "Looking Good")]
        LookingGood,
        Sold
    }

    public class Project : BaseEntity {

        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string ProjectName { get; set; }
        public int? EmployeeID { get; set; }

        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        public String StatusDisplayName() {

            return base.GetEnumDisplayName(this.Status);

        }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }

    }

}