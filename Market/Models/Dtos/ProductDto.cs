namespace Market.Models.Dtos
{
    public class ProductDto: BaseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
