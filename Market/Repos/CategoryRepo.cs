using AutoMapper;
using Market.Abstract;
using Market.Models;
using Market.Models.Dtos;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.RegularExpressions;

namespace Market.Repos
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CategoryRepo(IMapper mapper, IMemoryCache cache)
        {
            _mapper = mapper;
            _cache = cache;
        }
        public int AddCategory(CategoryDto categoryDto)
        {
            using var context = new MarketContext();
            var entityCategory = context.Categories.FirstOrDefault(x => x.Name.ToLower() == categoryDto.Name.ToLower());
            if (entityCategory is null)
            {
                entityCategory = _mapper.Map<Category>(categoryDto);
                context.Categories.Add(entityCategory);
                context.SaveChanges();
                _cache.Remove("categories");
            }
            return entityCategory.Id;
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            using (var context = new MarketContext())
            {
                if (_cache.TryGetValue("products", out List<CategoryDto> categoriesDto))
                {
                    return categoriesDto;
                }

                _cache.Set("products", categoriesDto, TimeSpan.FromMinutes(30));
                var getCategories = context.Categories.Select(x => _mapper.Map<CategoryDto>(x)).ToList();

                return getCategories;
            }
        }

        public string GetCategoriesCSV()
        {
            var sb = new StringBuilder();
            var categories = GetCategories();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.Id},{category.Name}, {category.Description}");
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
