using AutoMapper;
using Market.Abstract;
using Market.Models;
using Market.Models.Dtos;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Market.Repos
{
    public class ProductRepo : IProductRepo
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public ProductRepo(IMapper mapper, IMemoryCache cache)
        {
            _mapper = mapper;
            _cache = cache;
        }

        public int AddProduct(ProductDto productDto)
        {
            using (var context = new MarketContext())
            {
                var entityProduct = context.Products.FirstOrDefault(x => x.Name.ToLower() == productDto.Name.ToLower());
                if (entityProduct is null)
                {
                    entityProduct = _mapper.Map<Product>(productDto);
                    context.Products.Add(entityProduct);
                    context.SaveChanges();
                    _cache.Remove("products");
                }
                return entityProduct.Id;
            }
        }

        public IEnumerable<ProductDto> GetProducts()
        {
            using (var context = new MarketContext())
            {
                if (_cache.TryGetValue("products", out List<ProductDto> products))
                {
                    return products;
                }

                _cache.Set("products", products, TimeSpan.FromMinutes(30));

                products = context.Categories.Select(x => _mapper.Map<ProductDto>(x)).ToList();

                return products;
            }
        }

        public string GetProductsCSV()
        {
            var sb = new StringBuilder();
            var products = GetProducts();

            foreach (var product in products)
            {
                sb.AppendLine($"{product.Id},{product.Name}, {product.Description}");
            }
            return sb.ToString();
        }

        public string GetСacheStatCSV()
        {
            var curCache = _cache.GetCurrentStatistics();
            var sb = new StringBuilder();
            sb.AppendLine($"CurrentEntryCount, {curCache.CurrentEntryCount.ToString()}")
              .AppendLine($"CurrentEstimatedSize, {curCache.CurrentEstimatedSize.ToString()}")
              .AppendLine($"TotalHits, {curCache.TotalHits.ToString()}")
              .AppendLine($"TotalMisses, {curCache.TotalMisses.ToString()}");
            return sb.ToString();
        }

    }
}
