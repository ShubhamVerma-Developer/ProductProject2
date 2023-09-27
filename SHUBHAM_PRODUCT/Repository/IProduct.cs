using SHUBHAM_PRODUCT.Models;

namespace SHUBHAM_PRODUCT.Repository
{
    public interface IProduct
    {
        List<ProductModel> GetAll();
        ProductModel GetProduct(int id);
        ProductModel Add(ProductModel Product);
        ProductModel Update(ProductModel Product);
        void Remove(int id);
      
    }
}
