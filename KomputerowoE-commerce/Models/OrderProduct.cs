namespace KomputerowoE_commerce.Models
{
    public class OrderProduct
    {
        public int orderid { get; set; }
        public Orders Orders { get; set; }

        public int productid { get; set; }
        public Product Product { get; set; }

    }
}
