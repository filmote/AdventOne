﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdventOne.Models {

    public enum InvoiceStatus {
        Open,
        Paid
    }

    public class Invoice {

        public int ID { get; set; }
        public int ProjectID { get; set; }

        [EnumDataType(typeof(InvoiceStatus))]
        public InvoiceStatus Status { get; set; }

        public String InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime InvoiceDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime ExpectedPaymentDate { get; set; }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<InvoiceNote> InvoiceNotes { get; set; }

    }

}