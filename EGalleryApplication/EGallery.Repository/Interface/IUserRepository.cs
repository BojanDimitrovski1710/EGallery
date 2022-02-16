using EGallery.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGallery.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<EGalleryApplicationUser> GetAll();
        EGalleryApplicationUser Get(string id);
        void Insert(EGalleryApplicationUser entity);
        void Update(EGalleryApplicationUser entity);
        void Delete(EGalleryApplicationUser entity);
    }
}
