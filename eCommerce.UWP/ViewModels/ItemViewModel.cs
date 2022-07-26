using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace eCommerce.UWP.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return BoundItem?.Name ?? string.Empty;
            }

            set
            {
                if (BoundItem == null)
                {
                    return;
                }

                BoundItem.Name = value;
                BoundInvByWeight.Name = value;
                BoundInvByQuan.Name = value;
            }
        }

        public string Description
        {
            get
            {
                return BoundItem?.Description ?? string.Empty;
            }

            set
            {
                if (BoundItem == null)
                {
                    return;
                }

                BoundItem.Description = value;
                BoundInvByWeight.Description = value;
                BoundInvByQuan.Description = value;
            }
        }

        public double Price
        {
            get
            {
                return BoundItem?.Price ?? 0;
            }

            set
            {
                if (BoundItem == null)
                {
                    return;
                }

                BoundItem.Price = value;
                BoundInvByWeight.Price = value;
                BoundInvByQuan.Price = value;
            }
        }

        public bool isBogo
        {
            get
            {
                return BoundItem?.isBoGo ?? false;
            }

            set
            {
                if (BoundItem == null)
                {
                    return;
                }

                BoundItem.isBoGo = value;
                BoundInvByWeight.isBoGo = value;
                BoundInvByQuan.isBoGo = value;
            }
        }

        public int Quantity
        {
            get
            {
                if (BoundInvByQuan != null)
                    return BoundInvByQuan?.Quantity ?? 0;
                else if (BoundCartByQuan != null)
                    return BoundCartByQuan?.Quantity ?? 0;
                else return 0;
            }

            set
            {
                if (BoundInvByQuan == null)
                {
                    return;
                }

                BoundInvByQuan.Quantity = value;
            }
        }

        public double Weight
        {
            get
            {
                if(BoundInvByWeight != null)
                    return BoundInvByWeight?.Weight ?? 0;
                else if(BoundCartByWeight != null)
                    return BoundCartByWeight?.Weight ?? 0;
                else return 0;
            }

            set
            {
                if (BoundInvByWeight == null)
                {
                    return;
                }

                BoundInvByWeight.Weight = value;
            }
        }

        public int Id
        {
            get
            {
                return BoundItem?.Id ?? 0;
            }
            set
            {
                if (BoundItem == null)
                {
                    return;
                }

                BoundItem.Id = value;
                BoundInvByWeight.Id = value;
                BoundInvByQuan.Id = value;
            }

        }

        public double AmountToAdd
        {
            get
            {
                return BoundItem?.AmountToAdd ?? 0;
            }

            set
            {
                if (BoundItem == null)
                {
                    return;
                }

                BoundItem.AmountToAdd = value;
                BoundInvByWeight.AmountToAdd = value;
                BoundInvByQuan.AmountToAdd = value;

            }
        }

        public double TotalPrice
        {
            get
            {
                if (BoundCartByWeight == null)
                {
                    if (BoundCartByQuan != null)
                    {
                        return BoundCartByQuan?.TotalPrice ?? 0;
                    }
                }
                else if (BoundCartByQuan == null)
                {
                    if (BoundCartByWeight != null)
                    {
                        return BoundCartByWeight?.TotalPrice ?? 0;
                    }
                }
                return 0;
            }
        }
        

        public static InventoryItemByWeight InventoryItemByWeight(ItemViewModel vm)
        {
            return new InventoryItemByWeight
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                isBoGo = vm.isBogo,
                Id = vm.Id,
                Weight = vm.Weight
            };
        }

        public static InventoryItemByQuantity InventoryItemByQuantity(ItemViewModel vm)
        {
            return new InventoryItemByQuantity
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                isBoGo = vm.isBogo,
                Id = vm.Id,
                Quantity = vm.Quantity
            };
        }

        public static ItemViewModel Ivm(InventoryItemByQuantity boundInvByQuan)
        {
            return new ItemViewModel
            {
                Name = boundInvByQuan.Name,
                Description = boundInvByQuan.Description,
                Price = boundInvByQuan.Price,
                isBogo = boundInvByQuan.isBoGo,
                Quantity = boundInvByQuan.Quantity,
                Id = boundInvByQuan.Id
            };
        }

        public static ItemViewModel Ivm(InventoryItemByWeight boundInvByWeight)
        {
            return new ItemViewModel
            {
                Name = boundInvByWeight.Name,
                Description = boundInvByWeight.Description,
                Price = boundInvByWeight.Price,
                isBogo = boundInvByWeight.isBoGo,
                Weight = boundInvByWeight.Weight,
                Id = boundInvByWeight.Id
            };
        }

        private CartItemByQuantity boundCartByQuan;
        public CartItemByQuantity BoundCartByQuan
        {
            get { return boundCartByQuan; }
        }

        private CartItemByWeight boundCartByWeight;
        public CartItemByWeight BoundCartByWeight
        {
            get { return boundCartByWeight; }
        }

        private InventoryItemByQuantity boundInvByQuan;
        public InventoryItemByQuantity BoundInvByQuan
        {
            get
            {
                return boundInvByQuan;
            }
        }

        private InventoryItemByWeight boundInvByWeight;
        public InventoryItemByWeight BoundInvByWeight
        {
            get
            {
                return boundInvByWeight;
            }
        }

        public Item BoundItem
        {
            get
            {
                if (BoundInvByQuan != null)
                {
                    return BoundInvByQuan;
                }
                else if(BoundInvByWeight != null)
                {
                    return BoundInvByWeight;
                }
                else if(BoundCartByQuan != null)
                {
                    return BoundCartByQuan;
                }
                else if(BoundCartByWeight != null)
                {
                    return BoundCartByWeight;
                }
                else return null;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

        public ItemViewModel(Item i)
        {
            if (i == null)
            {
                return;
            }

            if (i is InventoryItemByQuantity)
            {
                boundInvByQuan = i as InventoryItemByQuantity;
            }
            else if (i is InventoryItemByWeight)
            {
                boundInvByWeight = i as InventoryItemByWeight;
            }
            else if(i is CartItemByQuantity)
            {
                boundCartByQuan = i as CartItemByQuantity;
            }
            else if(i is CartItemByWeight)
            {
                boundCartByWeight = i as CartItemByWeight;
            }
        }

        public ItemViewModel()
        {
            boundInvByWeight = new InventoryItemByWeight();
            boundInvByQuan = new InventoryItemByQuantity();
        }

        public Visibility IsWeightVisible
        {
            get
            {
                return BoundInvByWeight == null && BoundInvByQuan != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility IsQuantityVisible
        {
            get
            {
                return BoundInvByQuan == null && BoundInvByWeight != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility IsWeightVisibleCart
        {
            get
            {
                return BoundCartByWeight == null && BoundCartByQuan != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility IsQuantityVisibleCart
        {
            get
            {
                return BoundCartByQuan == null && BoundCartByWeight != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }



        public bool IsInventoryItemByWeight
        {
            get
            {
                return BoundInvByWeight != null;
            }

            set
            {
                if (value)
                {
                    boundInvByWeight = new InventoryItemByWeight();
                    boundInvByQuan = null;
                }
            }
        }

        public bool IsInventoryItemByQuantity
        {
            get
            {
                return BoundInvByQuan != null;
            }

            set
            {
                if (value)
                {
                    boundInvByQuan = new InventoryItemByQuantity();
                    boundInvByWeight = null;
                }
            }
        }
    }
}
