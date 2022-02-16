using EGallery.Domain.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGallery.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        
        public string UserId { get; set; }
        public EGalleryApplicationUser User { get; set; }

        public virtual ICollection<ProductInOrder> Products { get; set; }
    }
}
