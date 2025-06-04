using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KomputerowoE_commerce.Models
{
    //Order Model
    public class Orders
    {
        public int id { get; set; }
        public DateTime orderdate { get; set; } =DateTime.UtcNow;
        public string customername { get; set; }
        public List<OrderProduct> OrderProducts { get; set; } = new();
    }
}
