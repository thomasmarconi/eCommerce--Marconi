using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using eCommerce.UWP.Dialogs;

namespace eCommerce.UWP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Query { get; set; }
        public ItemViewModel SelectedItem { get; set; }
        private InventoryService _inventoryService;
        private CartService _cartService;

        public String savedCartName { get; set; }
        public ObservableCollection<String> loadCartNames  
        {
            get
            {
                return _cartService.SavedCartNames;
            }
        }

        public String SelectedLoad { get; set; }






        public ObservableCollection<ItemViewModel> Inventory
        {
            get
            {
                if (_inventoryService == null)
                {
                    return new ObservableCollection<ItemViewModel>();
                }

                if (string.IsNullOrEmpty(Query))
                {
                    return new ObservableCollection<ItemViewModel>(_inventoryService.Inventory.Select(i => new ItemViewModel(i)));
                }
                else
                {
                    return new ObservableCollection<ItemViewModel>(
                        _inventoryService.Inventory.Where(i => i.Name.ToUpper().Contains(Query.ToUpper())
                            || i.Description.ToUpper().Contains(Query.ToUpper()))
                        .Select(i => new ItemViewModel(i)));
                }

            }
        }

        public ObservableCollection<ItemViewModel> Cart
        {
            get
            {
                if (_cartService == null)
                {
                    return new ObservableCollection<ItemViewModel>();
                }

                if (string.IsNullOrEmpty(Query))
                {
                    return new ObservableCollection<ItemViewModel>(_cartService.Cart.Select(i => new ItemViewModel(i)));
                }
                else
                {
                    return new ObservableCollection<ItemViewModel>(
                        _cartService.Cart.Where(i => i.Name.ToUpper().Contains(Query.ToUpper())
                            || i.Description.ToUpper().Contains(Query.ToUpper()))
                        .Select(i => new ItemViewModel(i)));
                }

            }
        }

        public MainViewModel()
        {
            _inventoryService = InventoryService.Current;
            _cartService = CartService.Current;
        }

        public void Refresh()
        {
            NotifyPropertyChanged("Inventory");
        }

        public async void Save()
        {
            ContentDialog diag = new SaveDialog();
            await diag.ShowAsync();
        }

        public async void Load()
        {
            ContentDialog diag = new LoadDialog();
            await diag.ShowAsync();
            NotifyPropertyChanged("Inventory");
            NotifyPropertyChanged("Cart");  
        }

        public async Task Add(ProductType pType)
        {
            ContentDialog diag = null;
            if (pType == ProductType.ProductByQuantity)
            {
                diag = new InventoryByQuantityDialog();
            }
            else if(pType == ProductType.ProductByWeight)
            {
                diag = new InventoryByWeightDialog();
            }
            else
            {
                throw new NotImplementedException();
            }

            await diag.ShowAsync();
            NotifyPropertyChanged("Inventory");
            NotifyPropertyChanged("Cart");

        }

        public void DeleteFromInv()
        {
            var id = SelectedItem?.Id ?? -1;
            if (id >= 1)
            {
                _inventoryService.Delete(SelectedItem.Id);
            }
            NotifyPropertyChanged("Inventory");
        }

        public void DeleteFromCart()
        {
            var id = SelectedItem?.Id ?? -1;
            if (id >= 1)
            {
                _cartService.Delete(SelectedItem.Id);
            }
            NotifyPropertyChanged("Cart");
            NotifyPropertyChanged("Subtotal");
            NotifyPropertyChanged("Tax");
            NotifyPropertyChanged("Total");
        }


        public async Task Checkout()
        {
            ContentDialog diag = new PaymentDialog();
            await diag.ShowAsync();

            NotifyPropertyChanged("Cart");
            NotifyPropertyChanged("Subtotal");
            NotifyPropertyChanged("Tax");
            NotifyPropertyChanged("Total");
        }

        public async Task Update()
        {
            if (SelectedItem != null)
            {
                ContentDialog diag = null;
                if(SelectedItem.IsInventoryItemByQuantity)
                {
                    diag = new InventoryByQuantityDialog(ItemViewModel.Ivm(SelectedItem.BoundInvByQuan));
                } else if(SelectedItem.IsInventoryItemByWeight)
                {
                    diag = new InventoryByWeightDialog(ItemViewModel.Ivm(SelectedItem.BoundInvByWeight));
                }

                await diag.ShowAsync();
                NotifyPropertyChanged("Inventory");
            }

        }

        public async Task AddToCart()
        {
            if (SelectedItem != null)
            {
                ContentDialog diag = null;
                if(SelectedItem.IsInventoryItemByQuantity)
                {
                    diag = new QuantityAddToCartDialog(ItemViewModel.Ivm(SelectedItem.BoundInvByQuan));
                }
                else if(SelectedItem.IsInventoryItemByWeight)
                {
                    diag = new WeightAddToCartDialog(ItemViewModel.Ivm(SelectedItem.BoundInvByWeight));
                }
                 

                await diag.ShowAsync();
                NotifyPropertyChanged("Inventory");
                NotifyPropertyChanged("Cart");
                NotifyPropertyChanged("Subtotal");
                NotifyPropertyChanged("Tax");
                NotifyPropertyChanged("Total");
            }
        }

        public double Subtotal
        {
            get
            {
                return _cartService?.Subtotal() ?? 0;
            }
        }

        public double Tax
        {
            get
            {
                return _cartService?.Tax() ?? 0;
            }
        }

        public double Total
        {
            get
            {
                return _cartService?.Total() ?? 0;
            }
        }
    }
}
