using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdventOne.Models {
    public class InvTest {
        public InvTest() {

        }

        [Required]
        [Key]
        public int InvoiceID { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Invoice Number")]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayName("Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        public List<InvLineTest> InvoiceLines { get; set; }


    }
}