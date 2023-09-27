using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Orders")]
    public class Orders
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserID { get; set; }

        public string ReceiverAddress { get; set; }
        public string Note { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int UpdateBy { get; set; }
        [Required]
        public DateTime UpdateAt { get; set; }
        [Required]
        public int Status { get; set; }


    }
}
