using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PRN211_Grocery_store.Data.Entity
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime createdDate { get; set; }

        [Required]
        [StringLength(100)]
        public String username { get; set; }

        [Required]
        [StringLength(100)]
        public string status { get; set; }
        public virtual ICollection<Order> products { get; set; }
    }
}
