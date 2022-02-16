using EGallery.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EGallery.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public EGalleryApplicationUser Owner { get; set; }
        public virtual ICollection<ProductInShoppingCart> ProductsInCart { get; set; }
    }
}
