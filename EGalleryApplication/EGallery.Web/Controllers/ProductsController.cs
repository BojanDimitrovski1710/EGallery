using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

using EGallery.Domain.Identity;
using EGallery.Repository;
using EGallery.Domain.DomainModels;
using EGallery.Domain.DTO;
using EGallery.Services.Interface;

namespace EGallery.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<EGalleryApplicationUser> _userManager;

        public ProductsController(IProductService productService, UserManager<EGalleryApplicationUser> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var allProducts = _productService.GetAllProducts();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            ProductDTO prodDT = new ProductDTO
            {
                loggedInUser = user,
                Products = allProducts
            };
            return View(prodDT);
        }

        public IActionResult View(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Image,Description,Price,Rating")] Product product)
        {
            if(ModelState.IsValid)
            {
                this._productService.CreateNewProduct(product);
                return RedirectToAction("Index", "Products");
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(Guid? p)
        {
            if (p == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(p);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult AddToShoppingCart(Guid? id)
        {
            var model = this._productService.GetShoppingCartInfo(id);
            
            ViewBag.image = model.SelectedProduct.Image;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddToShoppingCart([Bind("ProductId", "Quantity", "SelectedProduct")]AddToShoppingCartDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._productService.AddToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Products");
            }

            return View(item);
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Image,Description,Price,Rating")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._productService.UpdeteExistingProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Products");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

       

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._productService.DeleteProduct(id);
            return RedirectToAction("Index" , "Products");
        }

        private bool ProductExists(Guid id)
        {
            return this._productService.GetDetailsForProduct(id) != null;
        }
    }
}
