namespace Market.Models
{
    public class Product: BaseModel
    {
        public int Cost { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; } = null;
        public IList<ProductStore>? ProductStores { get; set; } = null;
    }
}
