namespace Market.Models
{
    public class Product: BaseModel
    {
        public int Cost { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public IList<ProductStore>? ProductStores { get; set; }
    }
}
