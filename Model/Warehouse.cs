using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MaSTK_Lite.Model
{
    public class Warehouse
    {
        public int WarehouseID { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
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