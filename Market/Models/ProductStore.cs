using Microsoft.Identity.Client.Extensions.Msal;

namespace Market.Models
{
    public class ProductStore
    {
        public virtual Product? Product { get; set; }=null;
        public virtual Store? Store { get; set; } = null;
        public int? ProductId { get; set; }
        public int? StoreId { get; set; }
    }
}
