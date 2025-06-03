namespace KomputerowoE_commerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int[] Product_ID { get; set; }
        public string CustomerName { get; set; }
         
    }
}
