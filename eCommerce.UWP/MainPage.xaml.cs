using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using eCommerce.UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace eCommerce.UWP
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Save();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Load();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Refresh();
        }

        private async void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                await vm.AddToCart();
            }
        }

        private async void AddToInvQuantity_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                await vm.Add(ProductType.ProductByQuantity);
            }
        }

        private async void AddToInvWeight_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                await vm.Add(ProductType.ProductByWeight);
            }
        }

        private void DelFromCart_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
                vm.DeleteFromCart();
        }

        private void DelFromInv_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
                vm.DeleteFromInv();
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                await vm.Update();
            }
        }

        private async void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                await vm.Checkout();
            }
        }
    }

    public enum ProductType
    {
        Product, ProductByWeight, ProductByQuantity
    }
}
