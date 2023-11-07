using SampleApplication.Data;
using SampleApplication.Entities;
using SampleApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            var context = new SampleDbContext();
            var categories = context.Categories.ToList(); // Retrieve the list of categories from the database
            var productModel = new ProductModel
            {


                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            };

            return View(productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct([Bind(Exclude = "Id", Include = "Name, Description, Color, Images, Quantity, UnitPrice, SelectedCategoryId")] ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productModel);
            }

            // Create a list to store the file paths associated with the product
            List<string> imagePaths = new List<string>();

            var context = new SampleDbContext();
            // Handle the uploaded images
            if (productModel.Images != null && productModel.Images.Count > 0)
            {
                foreach (var imageFile in productModel.Images)
                {
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        // Save the image to a location of your choice, e.g., a folder on the server
                        // You can generate a unique file name to avoid overwriting existing images
                        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/Images"), fileName);
                        imageFile.SaveAs(filePath);

                        // Store the file path or other information in your database for reference
                        // You can associate the file with the product being added
                        // Store the file path in the list
                        imagePaths.Add("Images/" + fileName);
                    }
                }
            }



            //Save product to database

            // Create a new Product entity and set its properties
            var productEntity = new Product
            {
                Name = productModel.Name,
                Quantity = productModel.Quantity,
                Description = productModel.Description,
                UnitPrice = productModel.UnitPrice,
                Color = productModel.Color,
                Category = context.Categories.Find(productModel.SelectedCategoryId) // Set the Category property based on the selected category ID
            };


            context.Products.Add(productEntity);
            context.SaveChanges();


            // Associate the uploaded image file paths with the product
            if (imagePaths.Count > 0)
            {
                foreach (var imagePath in imagePaths)
                {
                    var imageEntity = new ProductImage
                    {
                        ProductId = productEntity.Id,
                        ImagePath = imagePath
                    };

                    context.ProductImages.Add(imageEntity);
                }

                context.SaveChanges();
            }


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
                return HttpNotFound();// Return a 404 Not Found response if the product is not found.

            }




            var categories = context.Categories.ToList(); // Retrieve the list of categories from the database




            var productModel = new ProductModel
            {
                Name = product.Name,
                Id = product.Id,
                Quantity = product.Quantity,
                Color = product.Color,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                SelectedCategoryId = product.Category == null ? 0 : product.Category.Id, // Set the selected category ID
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
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

            var categoryEntity = context.Categories.Find(productModel.SelectedCategoryId);

            if (categoryEntity == null)
            {
                // Add a custom validation error to ModelState
                ModelState.AddModelError("SelectedCategoryId", "The selected category is not valid. Please select a valid category.");
                return View(productModel);
            }


            //Lets update it
            productEntity.Name = productModel.Name;
            productEntity.Quantity = productModel.Quantity;
            productEntity.Color = productModel.Color;
            productEntity.Description = productModel.Description;
            productEntity.UnitPrice = productModel.UnitPrice;
            productEntity.Category = categoryEntity;

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


        [HttpGet]
        public ActionResult ProductDetails(int id)
        {
            var context = new SampleDbContext();
            var product = context.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound(); // Return a 404 Not Found response if the product is not found.
            }

            // Create a ProductModel to pass the product details to the view
            var productModel = new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Color = product.Color,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                SelectedCategoryId = product.Category.Id // Set the selected category ID
            };

            // You also need to retrieve the associated images and add them to the Images property
            // You can do this by querying the database for images associated with the product

            // Example: Retrieve image paths from the ProductImages table for the product
            var imagePaths = context.ProductImages.Where(pi => pi.ProductId == id).Select(pi => pi.ImagePath).ToList();

            // Set the image paths in the ProductModel
            productModel.ImagePaths = imagePaths;

            return View(productModel);
        }

    }
}