using HandyControl.Controls;
using HandyControl.Data;
using MaSTK_Lite.Interface;
using MaSTK_Lite.Model;
using MaSTK_Lite.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;

namespace MaSTK_Lite.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly DBConnector Database;

        private WarehouseCollection _allWarehouse;
        public WarehouseCollection AllWarehouse
        {
            get => _allWarehouse;
            set
            {
                _allWarehouse = value;
                NotifyPropertyChanged();
            }
        }

        private Warehouse _currentWarehouse;
        public Warehouse CurrentWarehouse
        {
            get => _currentWarehouse;
            set
            {
                _currentWarehouse = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IsWarehouseSelected));

                if (value is not null)
                {
                    AllProducts = System.Windows.Data.CollectionViewSource.GetDefaultView(value.Products);
                    NotifyPropertyChanged(nameof(AllProducts));
                    AllProducts.Filter = SearchProduct;
                }
            }
        }

        public bool IsWarehouseSelected => CurrentWarehouse != null;

        private Warehouse _newWarehouse = new();
        public Warehouse NewWarehouse
        {
            get => _newWarehouse;
            set
            {
                _newWarehouse = value;
                NotifyPropertyChanged();
            }
        }

        public ICollectionView AllProducts { get; set; }

        private Product _currentProduct;
        public Product CurrentProduct
        {
            get => _currentProduct;
            set
            {
                _currentProduct = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IsProductSelected));
                NotifyPropertyChanged(nameof(CurrentProductIndex));
            }
        }

        public int CurrentProductIndex { get; set; }
        public bool IsProductSelected => CurrentProduct is not null;

        private CategoryCollection _allCategories;
        public CategoryCollection AllCategories
        {
            get => _allCategories;
            set
            {
                _allCategories = value;
                NotifyPropertyChanged();
            }
        }

        #region Controles
        private Mode _eMode;
        public Mode Mode
        {
            get => _eMode;
            set
            {
                _eMode = value;
                NotifyPropertyChanged();
            }
        }

        //  Barra de Búsqueda.
        private string _searchProductTXB = string.Empty;
        public string SearchProductTXB
        {
            get => _searchProductTXB;
            set
            {
                _searchProductTXB = value;
                NotifyPropertyChanged();
                AllProducts.Refresh();
            }
        }

        // Nuevo Warehouse
        private string _newWarehouseName = string.Empty;
        public string NewWarehouseNameTXB
        {
            get => _newWarehouseName;
            set
            {
                _newWarehouseName = value;
                NotifyPropertyChanged();
            }
        }

        private string _newWarehouseDesc = string.Empty;
        public string NewWarehouseDescTXB
        {
            get => _newWarehouseDesc;
            set
            {
                _newWarehouseDesc = value;
                NotifyPropertyChanged();
            }
        }

        // Nuevo Producto
        private DateTime _dateNewProductTXB = DateTime.Now;
        public DateTime DateNewProductTXB
        {
            get => _dateNewProductTXB;
            set
            {
                _dateNewProductTXB = value;
                NotifyPropertyChanged();
            }
        }

        private string _productSKUNewProductTXB = string.Empty;
        public string ProductSKUNewProductTXB
        {
            get => _productSKUNewProductTXB;
            set
            {
                _productSKUNewProductTXB = value;
                NotifyPropertyChanged();
            }
        }

        private Category _categoryNewProductTXB;
        public Category CategoryNewProductTXB
        {
            get => _categoryNewProductTXB;
            set
            {
                _categoryNewProductTXB = value;
                NotifyPropertyChanged();
            }
        }

        private string _brandNewProductTXB = string.Empty;
        public string BrandNewProductTXB
        {
            get => _brandNewProductTXB;
            set
            {
                _brandNewProductTXB = value;
                NotifyPropertyChanged();
            }
        }

        private string _modelNewProductTXB = string.Empty;
        public string ModelNewProductTXB
        {
            get => _modelNewProductTXB;
            set
            {
                _modelNewProductTXB = value;
                NotifyPropertyChanged();
            }
        }

        private string _descNewProductTXB = string.Empty;
        public string DescNewProductTXB
        {
            get => _descNewProductTXB;
            set
            {
                _descNewProductTXB = value;
                NotifyPropertyChanged();
            }
        }

        private float _priceNewProductTXB;
        public float PriceNewProductTXB
        {
            get => _priceNewProductTXB;
            set
            {
                _priceNewProductTXB = value;
                NotifyPropertyChanged();
            }
        }

        private int _stockNewProductTXB;
        public int StockNewProductTXB
        {
            get => _stockNewProductTXB;
            set
            {
                _stockNewProductTXB = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isAddWarehouseDialogOpen;
        public bool IsAddWarehouseDialogOpen
        {
            get => _isAddWarehouseDialogOpen;
            set
            {
                _isAddWarehouseDialogOpen = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isAddProductDialogOpen;
        public bool IsAddProductDialogOpen
        {
            get => _isAddProductDialogOpen;
            set
            {
                _isAddProductDialogOpen = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public MainViewModel(DBConnector db)
        {
            Database = db;

            AllWarehouse = LoadWarehouseDB(db);
            AllCategories = LoadCategories(db);
        }

        private WarehouseCollection LoadWarehouseDB(DBConnector dB)
        {
            //  Quedan establecidas las relaciones, por lo cual simplemente agregando datos a las listas, y ejecutando SaveChanges, se guardan en la DB.
            return new WarehouseCollection(dB.Warehouses.Include(inc => inc.Products));
        }

        private CategoryCollection LoadCategories(DBConnector dB)
        {
            return new CategoryCollection(dB.Categories.OrderBy(x => x.Name));
        }
    }

    /// <summary>
    ///     Sección donde se establecen los comandos a utilizar por la vista.
    /// </summary>
    public partial class MainViewModel
    {
        //  Añadir un nuevo Warehouse
        private IRelayCommand _addWarehouseBTN;
        public IRelayCommand AddWarehouseBTN => _addWarehouseBTN ??= new RelayCommand(AddWarehouse);

        //  Añadir producto
        private IRelayCommand _addProductMENU;
        public IRelayCommand AddProductMENU => _addProductMENU ??= new RelayCommand(AddProduct, () => IsWarehouseSelected);

        //  Menu contextual del data grid "Editar"
        private IRelayCommand _editProductMENU;
        public IRelayCommand EditProductMENU => _editProductMENU ??= new RelayCommand(EditProduct, () => IsProductSelected);

        //  Botón guardar del drawer que va a sincronizar los datos ingresados con la database.
        private IRelayCommand _addOrEditProductBTN;
        public IRelayCommand AddOrEditProductBTN => _addOrEditProductBTN ??= new RelayCommand(AddOrEdit, () => IsWarehouseSelected);

        //  Menu contextual del data grid "Borrar"
        private IRelayCommand _deleteProductMENU;
        public IRelayCommand DeleteProductMENU => _deleteProductMENU ??= new RelayCommand(DeleteProduct, () => IsProductSelected);

        private IRelayCommand _aboutMeMENU;

        public IRelayCommand AboutMeMENU => _aboutMeMENU ??= new RelayCommand(() =>
        {
            Growl.Info(new GrowlInfo()
            {
                Message = "Este programa esta hecho exclusivamente para exponer en mi repositorio: \"MatiasSchwinch\" de GitHub\n\n" +
                          "Autor: Matias Schwinch\n" +
                          "Contacto: matias.schwinch@outlook.com",
                StaysOpen = true,
                ShowDateTime = false
            });
        });

        private void AddWarehouse()
        {
            try
            {
                Warehouse warehouse = new()
                {
                    Name = NewWarehouseNameTXB,
                    Description = NewWarehouseDescTXB,
                    Products = new ProductsCollection()
                };

                _ = Database.Warehouses.Add(warehouse);
                _ = Database.SaveChanges();
                AllWarehouse.Add(warehouse);

                NewWarehouseNameTXB = string.Empty;
                NewWarehouseDescTXB = string.Empty;

                Growl.Success(new GrowlInfo()
                {
                    Message = $"Se ha añadido el nuevo almacén a la base de datos.",
                    ShowDateTime = false,
                    Type = InfoType.Success,
                    WaitTime = 10
                });
            }
            catch (Exception e)
            {
                Growl.Error(new GrowlInfo()
                {
                    Message = $"Se a producido un error al intentar guardar los datos en la base de datos:\n\n{e.Message}",
                    ShowDateTime = false,
                    Type = InfoType.Error,
                    WaitTime = 10
                });
            }
            finally
            {
                IsAddWarehouseDialogOpen = false;
            }
        }

        private void AddProduct()
        {
            if (CurrentWarehouse is null) { return; }

            Mode = Mode.Add;

            DrawerAddProductClear();

            IsAddProductDialogOpen = true;
        }

        private void EditProduct()
        {
            #region AntiguoDialogo
            //DialogResult = await Dialog.Show<ProductDialog>()
            //               .Initialize<ProductDialogViewModel>(vm =>
            //               {
            //                   vm.TriggerMode = Mode.Edit;
            //                   //vm.ActualProduct = CurrentProduct;
            //                   vm.Categories = AllCategories;
            //                   vm.NewProduct = CurrentProduct.Clone();
            //               })
            //               .GetResultAsync<Product>()
            //               .ContinueWith(fn => DialogResult = fn.Result);
            #endregion

            if (CurrentWarehouse is null) { return; }

            Mode = Mode.Edit;

            DrawerAddProductClear();

            DateNewProductTXB = CurrentProduct.Date;
            ProductSKUNewProductTXB = CurrentProduct.ProductSKU;
            CategoryNewProductTXB = CurrentProduct.Category;
            BrandNewProductTXB = CurrentProduct.Brand;
            ModelNewProductTXB = CurrentProduct.Model;
            DescNewProductTXB = CurrentProduct.Description;
            PriceNewProductTXB = CurrentProduct.Price;
            StockNewProductTXB = CurrentProduct.Stock;

            IsAddProductDialogOpen = true;
        }

        private void DeleteProduct()
        {
            Growl.Ask(new GrowlInfo()
            {
                Message = $"Esta seguro que desea eliminar el producto seleccionado?",
                ConfirmStr = "Si, estoy seguro.",
                CancelStr = "No",
                ShowDateTime = false,
                StaysOpen = true,
                ActionBeforeClose = isConfirmed =>
                {
                    if (isConfirmed)
                    {
                        try
                        {
                            _ = CurrentWarehouse.Products.Remove(CurrentProduct);
                            _ = Database.SaveChanges();
                            Growl.Success(new GrowlInfo()
                            {
                                Message = $"Se ha eliminado correctamente el producto a la base de datos.",
                                ShowDateTime = false,
                                Type = InfoType.Success,
                                WaitTime = 5
                            });
                        }
                        catch (Exception e)
                        {
                            Growl.Error(new GrowlInfo()
                            {
                                Message = $"Se ha producido un error al intentar borrar el producto a la base de datos:\n\n{e.Message}",
                                ShowDateTime = false,
                                Type = InfoType.Error,
                                WaitTime = 5
                            });
                        }
                    }
                    return true;
                },
            });
        }

        private void AddOrEdit()
        {
            switch (Mode)
            {
                case Mode.Add:
                    try
                    {
                        //var warehouse = Database.Warehouses.Include(p => p.Products).Single(X => X.WarehouseID == CurrentWarehouse.WarehouseID);
                        //warehouse.Products.Add(newproduct);
                        CurrentWarehouse.Products.Add(new Product()
                        {
                            Date = DateNewProductTXB,
                            ProductSKU = ProductSKUNewProductTXB,
                            Category = CategoryNewProductTXB,
                            Brand = BrandNewProductTXB,
                            Model = ModelNewProductTXB,
                            Description = DescNewProductTXB,
                            Price = PriceNewProductTXB,
                            Stock = StockNewProductTXB,
                        });

                        _ = Database.SaveChanges();

                        AllProducts.Refresh();

                        #region Restablece las propiedades de los controles del drawers correspondiente.
                        DrawerAddProductClear();
                        #endregion

                        Growl.Success(new GrowlInfo()
                        {
                            Message = $"Se ha añadido el nuevo producto a la base de datos.",
                            ShowDateTime = false,
                            Type = InfoType.Success,
                            WaitTime = 10
                        });
                    }
                    catch (Exception e)
                    {
                        Growl.Error(new GrowlInfo()
                        {
                            Message = $"Se ha producido un error al intentar agregar el producto a la base de datos:\n\n{e.Message}",
                            ShowDateTime = false,
                            Type = InfoType.Error,
                            WaitTime = 10
                        });
                    }
                    finally
                    {
                        IsAddProductDialogOpen = false;
                    }

                    break;

                case Mode.Edit:
                    try
                    {
                        CurrentProduct.Date = DateNewProductTXB;
                        CurrentProduct.ProductSKU = ProductSKUNewProductTXB;
                        CurrentProduct.Category = CategoryNewProductTXB;
                        CurrentProduct.Brand = BrandNewProductTXB;
                        CurrentProduct.Model = ModelNewProductTXB;
                        CurrentProduct.Description = DescNewProductTXB;
                        CurrentProduct.Price = PriceNewProductTXB;
                        CurrentProduct.Stock = StockNewProductTXB;

                        _ = Database.SaveChanges();

                        AllProducts.Refresh();

                        #region Restablece las propiedades de los controles del drawers correspondiente.
                        DrawerAddProductClear();
                        #endregion

                        Growl.Success(new GrowlInfo()
                        {
                            Message = $"Se ha editado el producto y se guardo en la base de datos correctamente.",
                            ShowDateTime = false,
                            Type = InfoType.Success,
                            WaitTime = 10
                        });
                    }
                    catch (Exception e)
                    {
                        Growl.Error(new GrowlInfo()
                        {
                            Message = $"Se ha producido un error al intentar agregar la edición del producto a la base de datos:\n\n{e.Message}",
                            ShowDateTime = false,
                            Type = InfoType.Error,
                            WaitTime = 10
                        });
                    }
                    finally
                    {
                        IsAddProductDialogOpen = false;
                    }

                    break;

                default:
                    break;
            }
        }

        private void DrawerAddProductClear()
        {
            #region Restablece las propiedades de los controles del drawers correspondiente.
            DateNewProductTXB = DateTime.Now;
            ProductSKUNewProductTXB = string.Empty;
            CategoryNewProductTXB = null;
            BrandNewProductTXB = string.Empty;
            ModelNewProductTXB = string.Empty;
            DescNewProductTXB = string.Empty;
            PriceNewProductTXB = 0f;
            StockNewProductTXB = 0;
            #endregion
        }

        private bool SearchProduct(object obj)
        {
            return obj is Product prt && prt.Description.Contains(SearchProductTXB, StringComparison.InvariantCultureIgnoreCase);
        }

    }

    public enum Mode
    {
        Add,
        Edit
    }
}