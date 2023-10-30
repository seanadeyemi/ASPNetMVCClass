using SampleApplication.Data;
using SampleApplication.Entities;
using SampleApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SampleApplication.Controllers
{
    public class ProductsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var context = new SampleDbContext();
            var productsList = context.Products.ToList();

            var productsModelList = new List<ProductModel>();
            foreach (var product in productsList)
            {
                productsModelList.Add(new ProductModel
                {
                    Name = product.Name,
                    Color = product.Color,
                    Description = product.Description,
                    Quantity = product.Quantity,
                    UnitPrice = product.UnitPrice
                });
            }


            return View(productsModelList);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productModel);
            }



            //Save product to database

            var context = new SampleDbContext();

            var productEntity = new Product
            {
                Name = productModel.Name,
                Quantity = productModel.Quantity,
                Description = productModel.Description,
                UnitPrice = productModel.UnitPrice,
                Color = productModel.Color,
                Id = productModel.Id
            };

            context.Products.Add(productEntity);
            context.SaveChanges();



            //redirect or navigate to the index page
            return RedirectToAction("Index");

        }


    }
}