using Avalonia.Controls;
using Avalonia.ReactiveUI;
using course_project_filip.Models;
using course_project_filip.ViewModels;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace course_project_filip.Views
{
	public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
	{
		public static String mPath = AppDomain.CurrentDomain.BaseDirectory;
		public static String mDBPath = mPath + "Course_Project.db";

		private async Task DoShowDialogAsync(InteractionContext<AddSupplierViewModel, Supplier?> interaction)
		{
			var dialog = new AddSupplier();
			dialog.DataContext = interaction.Input;

			var result = await dialog.ShowDialog<Supplier?>(this);
			interaction.SetOutput(result);
		}
		private async Task DoShowDialogAsyncP(InteractionContext<AddProductViewModel, Product?> interaction)
		{
			var dialog = new AddProduct();
			dialog.DataContext = interaction.Input;

			var result = await dialog.ShowDialog<Product?>(this);
			interaction.SetOutput(result);
		}
		private async Task DoShowDialogAsyncR(InteractionContext<AddResourceViewModel, Resource?> interaction)
		{
			var dialog = new AddResource();
			dialog.DataContext = interaction.Input;

			var result = await dialog.ShowDialog<Resource?>(this);
			interaction.SetOutput(result);
		}

		public MainWindow()
		{
			InitializeComponent();
			this.WhenActivated(action =>
				action(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
			this.WhenActivated(action =>
				action(ViewModel!.ShowDialogp.RegisterHandler(DoShowDialogAsyncP)));
			this.WhenActivated(action =>
				action(ViewModel!.ShowDialogr.RegisterHandler(DoShowDialogAsyncR)));
		}


	}
}