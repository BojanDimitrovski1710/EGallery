using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EGallery.Domain.DomainModels
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        
        public int Rating { get; set; }

        public virtual ICollection<ProductInShoppingCart> ProductinShoppingCarts { get; set; }
        public virtual ICollection<ProductInOrder> Orders { get; set; }
    }
}
