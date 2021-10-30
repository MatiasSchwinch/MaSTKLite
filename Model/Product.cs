using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSTK_Lite.Model
{
    [Table("Products")]
    public class Product
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        public DateTime Date { get; set; }
        public string ProductSKU { get; set; }

        public int? CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }

        public int Stock { get; set; }
        public float Price { get; set; }

        public int WarehouseID { get; set; }
        [ForeignKey("WarehouseID")]
        public virtual Warehouse Warehouse { get; set; }

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
