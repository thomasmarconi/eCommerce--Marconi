using eCommerce.UWP.ViewModels;
using Library.eCommerce.Services;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace eCommerce.UWP.Dialogs
{
    public sealed partial class InventoryByWeightDialog : ContentDialog
    {
        public InventoryByWeightDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ItemViewModel();
        }

        public InventoryByWeightDialog(ItemViewModel ivm)
        {
            this.InitializeComponent();
            this.DataContext = ivm;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //step 1: coerce datacontext into view model
            var viewModel = DataContext as ItemViewModel;

            //step 2: use a conversion constructor from view model -> todo
            var prodByWeight = ItemViewModel.InventoryItemByWeight(viewModel);

            //step 3: interact with the service using models;
            InventoryService.Current.AddOrUpdate(prodByWeight);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
