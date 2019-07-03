using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdventOne.Models {

    public class Task {

        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string Description { get; set; }

        [AllowHtml]
        [UIHint("tinymce_jquery_full")]
        public string FullText { get; set; }

        public virtual Project Project { get; set; }
    }

}