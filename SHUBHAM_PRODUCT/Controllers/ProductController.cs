using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHUBHAM_PRODUCT.Models;
using SHUBHAM_PRODUCT.Repository;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using Dapper;

namespace SHUBHAM_PRODUCT.Controllers
{


    /// <summary>
    /// ProductController for perform actions
    /// </summary>
    public class ProductController : Controller
    {

        private readonly IDbConnection db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProduct _product;

        /// <summary>
        /// Constructor of ProductController.
        /// </summary>
        /// <param name="product">The repository for product data.</param>
        public ProductController(IProduct product, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _product = product;
            _webHostEnvironment = webHostEnvironment;
            this.db = new SqlConnection(configuration.GetConnectionString("DBConn"));
        }

        /// <summary>
        /// Display the Home page Which contains all the records
        /// </summary>
        /// <returns>The Index view.</returns>
        public IActionResult Index()
        {
            //string query = "select CatId, PName from SHUBHAM_Category;";
            //IEnumerable<Item> items = db.Query<Item>(query);
            //ViewBag.Items = new SelectList(items, "CatId", "PName");

            List<ProductModel> list = _product.GetAll();

            return View(list);
        }


        /// <summary>
        /// Show the details of product
        /// </summary>
        /// <returns>The Index view.</returns>
        [HttpGet]
        public IActionResult Details(int id)
        {
            ProductModel product = _product.GetProduct(id);
            if (product == null)
            {
                return View("Product not found");
            }
            return View(product);
        }


        /// <summary>
        /// Show the view for create a new record 
        /// </summary>
        /// <returns>The create view.</returns>
        public IActionResult Create()
        {
            string query1 = "select CatId, PName from SHUBHAM_Category;";
            IEnumerable<Item> items = db.Query<Item>(query1);
            ViewBag.Items = new SelectList(items, "CatId", "PName");

            string query2 = "select Pid, Ptype from SHUBHAM_Type;";
            IEnumerable<TypeItem> typeItems = db.Query<TypeItem>(query2);
            ViewBag.TypeItems = new SelectList(typeItems, "Pid", "Ptype");
            return View();
        }







        /// <summary>
        /// To save the record inside the database by using stored procedure
        /// </summary>
        /// <param name="product">The product to be insert</param>
        /// <param name="image">The image for the the product.</param>
        /// <returns>Redirects to the main page on success, or returns to the create view on failure.</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductModel product, IFormFile ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageFile.CopyTo(stream);
                        }
                        product.Image = "~/Images/" + uniqueFileName;
                    }
                    else
                    {
                        product.Image = "~/Images/NoImage.png";
                    }
                    _product.Add(product);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(product);
        }

        /// <summary>
        /// show the view for Edit the product.
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>The edit view.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            string query = "select CatId, PName from SHUBHAM_Category;";
            IEnumerable<Item> items = db.Query<Item>(query);
            ViewBag.Items = new SelectList(items, "CatId", "PName");

            string query2 = "select Pid, Ptype from SHUBHAM_Type;";
            IEnumerable<TypeItem> typeItems = db.Query<TypeItem>(query2);
            ViewBag.TypeItems = new SelectList(typeItems, "Pid", "Ptype");

            ProductModel product = _product.GetProduct(id);

            if (product == null)
            {
                return View("Product not found");
            }
            return View(product);
        }

        /// <summary>
        /// Handles the submission of edited product data to update the database.
        /// </summary>
        /// <param name="id">The ID of the product</param>
        /// <param name="product">The updated product</param>
        /// <param name="imageFile">The updated image file</param>
        /// <returns>Redirects to the index page</returns>

        [HttpPost]
        public IActionResult Edit(ProductModel product, IFormFile ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageFile.CopyTo(stream);
                        }
                        product.Image = "~/Images/" + uniqueFileName;
                    }
                    else
                    {

                    }
                    _product.Update(product);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(product);
        }

        /// <summary>
        /// Show the view for deleting the record.
        /// </summary>
        /// <param name="id">Id of the product.</param>
        /// <returns>The delete view.</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            ProductModel product = _product.GetProduct(id);
            if (product == null)
            {
                return NotFound(); // Return a 404 response when the product is not found.
            }
            return View(product);
        }

        /// <summary>
        /// Delete comfirm 
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns> Redirect to the index page</returns>
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _product.Remove(id);
            return RedirectToAction("Index"); // Redirect to the action that displays the list of products.
        }




    }
}
