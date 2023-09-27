using System.Data.SqlClient;
using System.Data;
using SHUBHAM_PRODUCT.Models;
using Dapper;

namespace SHUBHAM_PRODUCT.Repository
{
    public class Product : IProduct
    {
        private readonly IDbConnection db;

        //// <summary>
        /// Define the costructor of the product class
        /// </summary>
        public Product(IConfiguration configuration)
        {
            // create a connection variable type is string
            this.db = new SqlConnection(configuration.GetConnectionString("DBConn"));
        }


        /// <summary>
        /// Insert the record into the database
        /// </summary>
        /// <returns>The Collection the type is ProductModel.</returns>
        public ProductModel Add(ProductModel product)
        {
            try
            {
                var sql = "SHUBHAM_Product1_Create";
                var parameters = new
                {
                    Name = product.Name,
                    CategoryId = Convert.ToInt32(product.CategoryId),
                    Description = product.Description,
                    Quantity = product.Quantity,
                    Type = product.Type,
                    Image = product.Image
                };
                var affectedRows = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    return product;
                }
                else
                {
                    return null; // or throw an exception if you prefer
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed, e.g., log it or return an error response.
                // You might also want to rethrow it if it's a critical error.
                // Example: throw ex;
                return null; // Returning null as an indication of an error
            }
        }


        /// <summary>
        /// Get all the records 
        /// </summary>
        /// <returns>Product.</returns>
        public List<ProductModel> GetAll()
        {
            return db.Query<ProductModel>("SHUBHAM_Product1_GetAll", commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// get the produt record
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns>Product</returns>
        public ProductModel GetProduct(int id)
        {
            var parameters = new { Id = id };

            var product = db.Query<ProductModel>("SHUBHAM_Product1_GetById", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return product;
        }

        /// <summary>
        /// Deletes a records
        /// </summary>
        /// <param name="id">Remove the record from the database</param>
        public void Remove(int id)
        {
            var parameters = new { Id = id };
            db.Execute("SHUBHAM_Product1_Delete", parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Update the records
        /// </summary>
        /// <param name="product">Update the record</param>
        public ProductModel Update(ProductModel product)
        {
            var sql = "SHUBHAM_Product1_Update";
            var parameters = new
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = Convert.ToInt32(product.CategoryId),
                Description = product.Description,
                Quantity = product.Quantity,
                Type = product.Type,
                Image = product.Image
            };
            var affectedRows = db.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
            if (affectedRows > 0)
            {
                return product;
            }
            else
            {
                return null;
            }
        }

    }
}
