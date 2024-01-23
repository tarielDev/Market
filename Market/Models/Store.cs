namespace Market.Models
{
    public class Store: BaseModel
    {
        public IList<ProductStore>? ProductStores { get; set; }
    }
}
