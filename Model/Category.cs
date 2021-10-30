using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaSTK_Lite.Model
{
    [Table("Categories")]
    public class Category
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        public string Name { get; set; }

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