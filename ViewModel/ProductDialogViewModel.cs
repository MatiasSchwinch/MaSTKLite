using HandyControl.Tools.Extension;
using MaSTK_Lite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSTK_Lite.ViewModel
{
    public class ProductDialogViewModel : ViewModelBase, IDialogResultable<Product>
    {
        public Product Result { get; set; }
        public Action CloseAction { get; set; }
        public Mode TriggerMode { get; set; }

        private CategoryCollection _categories;

        public CategoryCollection Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                NotifyPropertyChanged();
            }
        }


        private Product _newProduct;
        public Product NewProduct
        {
            get => _newProduct;
            set
            {
                _newProduct = value;
                Result = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand CloseBTN => new(() => CloseAction?.Invoke());
        public RelayCommand SaveBTN => new(Save);
        public RelayCommand UndoBTN => new(Undo);

        private void Save()
        {

        }

        private void Undo()
        {
            HandyControl.Controls.MessageBox.Show(new HandyControl.Data.MessageBoxInfo
            {
                Message = "Estoy Undeando?",
                Caption = "Attention",
                Button = System.Windows.MessageBoxButton.YesNo
            });
            /*, , , System.Windows.MessageBoxImage.Question*/
            //ActualProduct.Date = OriginalProduct.Date;
            //ActualProduct.ProductID = OriginalProduct.ProductID;
            //ActualProduct.Category = OriginalProduct.Category;
            //ActualProduct.Brand = OriginalProduct.Brand;
            //ActualProduct.Model = OriginalProduct.Model;
            //ActualProduct.Description = OriginalProduct.Description;
            //ActualProduct.Stock = OriginalProduct.Stock;
            //ActualProduct.Price = OriginalProduct.Price;
        }

    }
}
