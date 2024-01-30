using Market.Models.Dtos;

namespace Market.Abstract
{
    public interface IProductRepo
    {
        public int AddProduct(ProductDto productDto);
        public string GetProductsCSV();
        public string GetСacheStatCSV();

        public IEnumerable<ProductDto> GetProducts();
    }
}
