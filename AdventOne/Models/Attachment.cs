using System.ComponentModel.DataAnnotations;

namespace AdventOne.Models {

    public enum FileType {
        Quotation = 1,
        PurchaseOrder
    }

    public class Attachment {

        public int ID { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(255)]
        [Required]
        public string Description { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

    }
}
