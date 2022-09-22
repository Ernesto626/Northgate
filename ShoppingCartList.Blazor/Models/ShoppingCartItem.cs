﻿namespace ShoppingCartList.Blazor.Models
{
    public class ShoppingCartItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; } = DateTime.Now;
        public string ItemName { get; set; }
        public bool Collected { get; set; }
    }
}
