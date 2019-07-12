using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdventOne.Models {

    public enum RevenueType {
        COS = 0,
        REV
    }

    public class Task {

        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
        public RevenueType RevenueType { get; set; }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [AllowHtml]
        [UIHint("tinymce_jquery_full")]
        public string FullText { get; set; }

        public virtual Project Project { get; set; }
    }

}