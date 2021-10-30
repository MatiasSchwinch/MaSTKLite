using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSTK_Lite.Model
{
    [Table("Warehouses")]
    public class Warehouse
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WarehouseID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductsCollection Products { get; set; }

        public Warehouse Clone()
        {
            return (Warehouse)MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public class WarehouseCollection : ObservableCollection<Warehouse>
    {
        public WarehouseCollection() { }
        public WarehouseCollection(IEnumerable<Warehouse> collection) : base(collection) { }
        public WarehouseCollection(List<Warehouse> list) : base(list) { }
    }
}