using eCommerce.UWP.ViewModels;
using Library.eCommerce.Models;
using Library.eCommerce.Services;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace eCommerce.UWP.Dialogs
{
    public sealed partial class WeightAddToCartDialog : ContentDialog
    {
        public WeightAddToCartDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ItemViewModel();
        }

        public WeightAddToCartDialog(ItemViewModel ivm)
        {
            this.InitializeComponent();
            this.DataContext = ivm;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //step 1: coerce datacontext into view model
            var viewModel = DataContext as ItemViewModel;

            if (viewModel.BoundInvByWeight.Weight < viewModel.AmountToAdd)
                viewModel.AmountToAdd = viewModel.BoundInvByWeight.Weight;

            var newInvByWeight = new InventoryItemByWeight(viewModel.BoundItem.Name, viewModel.BoundItem.Description, viewModel.BoundItem.Price, viewModel.BoundInvByWeight.Weight, viewModel.BoundItem.isBoGo, viewModel.BoundInvByWeight.Id);

            //step 3: interact with the service using models;
            InventoryService.Current.AddToCart(newInvByWeight, 0, viewModel.AmountToAdd);
            InventoryService.Current.AddOrUpdate(newInvByWeight);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
