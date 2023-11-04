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
                    UnitPrice = product.UnitPrice,
                    Id = product.Id
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
        public ActionResult AddProduct([Bind(Exclude = "Id")] ProductModel productModel)
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var context = new SampleDbContext();
            var product = context.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            var productModel = new ProductModel
            {
                Name = product.Name,
                Id = product.Id,
                Quantity = product.Quantity,
                Color = product.Color,
                Description = product.Description,
                UnitPrice = product.UnitPrice

            };

            return View(productModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productModel);

            }

            var context = new SampleDbContext();
            var productEntity = context.Products.Find(productModel.Id);

            if (productEntity == null)
            {
                return HttpNotFound();
            }

            //Lets update it
            productEntity.Name = productModel.Name;
            productEntity.Quantity = productModel.Quantity;
            productEntity.Color = productModel.Color;
            productEntity.Description = productModel.Description;
            productEntity.UnitPrice = productModel.UnitPrice;

            context.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var context = new SampleDbContext();
            var productEntity = context.Products.Find(id);


            if (productEntity == null)
            {
                return HttpNotFound();
            }


            //Remove the product entity from the database
            context.Products.Remove(productEntity);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}