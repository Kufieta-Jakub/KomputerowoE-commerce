namespace KomputerowoE_commerce.Models
{
    //Order Model
    public class Order
    {
        public int id { get; set; }
        public DateTime orderdate { get; set; }
        public string customername { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
