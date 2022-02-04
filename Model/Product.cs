using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MaSTK_Lite.Model
{
    public class Product
    {
        public int ProductID { get; set; }
        public int WarehouseID { get; set; }
        public int? CategoryID { get; set; }

        public DateTime Date { get; set; }
        public string ProductSKU { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Stock { get; set; }
        public float Price { get; set; }

        public Category Category { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;

        public Product()
        {
            Date = DateTime.Now;
        }

        public Product Clone()
        {
            return (Product)MemberwiseClone();
        }

        public override string ToString()
        {
            return $"[{ProductID}] {Description} - {Stock} - {Price}";
        }
    }

    public class ProductsCollection : ObservableCollection<Product>
    {
        public ProductsCollection() { }
        public ProductsCollection(IEnumerable<Product> collection) : base(collection) { }
        public ProductsCollection(List<Product> list) : base(list) { }
    }
}
