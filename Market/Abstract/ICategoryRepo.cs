using Market.Models.Dtos;

namespace Market.Abstract
{
    public interface ICategoryRepo
    {
        public int AddCategory(CategoryDto categoryDto);
        public string GetCategoriesCSV();
        public string GetСacheStatCSV();

        public IEnumerable<CategoryDto> GetCategories();
    }
}
