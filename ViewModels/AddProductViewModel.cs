using course_project_filip.Models;
using course_project_filip.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;


namespace course_project_filip.ViewModels
{
	   public class AddProductViewModel : ViewModelBase
	{
		public ReactiveCommand<Unit, Product?> SaveProduct { get; }
		public Product ProductItem;
		public string mode = "insert";

		public AddProductViewModel()
		{
			SaveProduct = ReactiveCommand.Create(() =>
			{
				ProductItem = new Product();
				ProductItem.Title= Title;
				ProductItem.Capacity = Capacity;
				ProductItem.Price = Price;
				ProductItem.Quantity = Quantity;
				return ProductItem;
			});
		}
		

		string _title= "";
		public string Title
		{
			get { return _title; }
			set
			{
				if (_title != value)
				{
					this.RaiseAndSetIfChanged(ref _title, value);
				}
			}
		}

		int _capacity = 0;
		public int Capacity
		{
			get { return _capacity; }
			set
			{
				if (_capacity != value)
				{
					this.RaiseAndSetIfChanged(ref _capacity, value);
				}
			}
		}

		decimal _price = 0;
		public decimal Price
		{
			get { return _price; }
			set
			{
				if (_price != value)
				{
					this.RaiseAndSetIfChanged(ref _price, value);
				}
			}
		}

		int _quantity = 0;
		public int Quantity
		{
			get { return _quantity; }
			set
			{
				if (_quantity != value)
				{
					this.RaiseAndSetIfChanged(ref _quantity, value);
				}
			}
		}

	}
	
}
