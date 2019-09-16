using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdventOne.Models {

    public class InvLineTest {

        public InvLineTest() {

        }

        [Key]
        [Required]
        public int InvoiceLineId { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Item")]
        public string ItemName { get; set; }

        [DisplayName("Description")]
        public string ItemDescription { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal Value { get; set; }

        [ForeignKey("ParentInvoice")]
        public int InvoiceID { get; set; }

        public InvTest ParentInvoice { get; set; }

    }
}