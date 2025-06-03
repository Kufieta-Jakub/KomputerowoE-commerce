using System.ComponentModel.DataAnnotations.Schema;

namespace KomputerowoE_commerce.Models
{
    //Order Model
    public class Orders
    {
        public int id { get; set; }
        public DateTime orderdate { get; set; }
        public string customername { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
