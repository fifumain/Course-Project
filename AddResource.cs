using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using course_project_filip.Models;
using course_project_filip.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;



namespace course_project_filip
{
	public partial class AddResource : ReactiveWindow<AddResourceViewModel>
	{
		public AddResource()
		{
			InitializeComponent();
			this.WhenActivated(OnActivated);
			var bCancel = this.FindControl<Button>("bCancel");
			bCancel.Click += BCancel_Click;
		}

  private void OnActivated(CompositeDisposable disposables)
{
	

		ViewModel!.SaveResource.Subscribe(Close).DisposeWith(disposables);
		if (ViewModel!.mode == "edit")
		{
			ViewModel!.Title = ViewModel!.ResourceItem.Title;
			ViewModel!.Quantity = ViewModel!.ResourceItem.Quantity;
			ViewModel!.SupplierName = ViewModel!.ResourceItem.SupplierName;
		}
	
}
		private void BCancel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			Close(); // Закрываем текущее окно
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
