using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MaSTK_Lite.Model
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; } = null!;

        public ProductsCollection Product { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public class CategoryCollection : ObservableCollection<Category>
    {
        public CategoryCollection() { }
        public CategoryCollection(IEnumerable<Category> collection) : base(collection) { }
        public CategoryCollection(List<Category> list) : base(list) { }
    }
}