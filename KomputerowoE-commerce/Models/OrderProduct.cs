using System.Text.Json.Serialization;

namespace KomputerowoE_commerce.Models
{
    public class OrderProduct
    {
        public int orderid { get; set; }
        [JsonIgnore]
        public Orders? Orders { get; set; }

        public int productid { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }

        public int quantity { get; set; }

    }
}
