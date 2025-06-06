﻿using System.Text.Json.Serialization;

namespace KomputerowoE_commerce.Models
{
    //Product Model
    public class Product
    {
        public int id { get; set; }
        public string? name { get; set; }
        public decimal? price { get; set; }
        public string? type { get; set; }
        public string? description { get; set; }
        [JsonIgnore]
        public List<OrderProduct> orderproduct { get; set; } = new();
    }
}
