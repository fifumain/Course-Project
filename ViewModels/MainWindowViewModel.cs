using course_project_filip.Models;
using course_project_filip.ViewModels;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Linq;

namespace course_project_filip.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		// Властивості для постачальників
		public Suppliers TheSupplier { get; } = new Suppliers();
		public Interaction<AddSupplierViewModel, Supplier?> ShowDialog { get; }
		public ICommand AddSupplierCommand { get; }
		public ICommand EditSupplierCommand { get; }
		public ICommand DeleteSupplierCommand { get; }
		public ICommand SelectRowCommand { get; }

		// Властивості для продуктів
		public ICommand AddProductCommand { get; }
		public ICommand EditProductCommand { get; }
		public ICommand DeleteProductCommand { get; }
		public ICommand SearchProductCommand { get; }
		public ICommand ClearDataCommand { get; }
		public Products TheProduct { get; } = new Products();
		public Interaction<AddProductViewModel, Product?> ShowDialogp { get; }
		
				// Властивості для ресурсів
		public ICommand AddResourceCommand { get; }
		public ICommand EditResourceCommand { get; }
		public ICommand DeleteResourceCommand { get; }
		public ICommand SearchResourceCommand { get; }
		public ICommand ClearDataCommandResource { get; }
		public Resources TheResource { get; } = new Resources();
		public Interaction<AddResourceViewModel, Resource?> ShowDialogr { get; }

		// Властивості для журналу
		public Logs TheLog { get; } = new Logs();

		// Властивості для фільтрації продуктів
		private ObservableCollection<Product> _filteredProducts;
		public ObservableCollection<Product> FilteredProducts
		{
			get => _filteredProducts;
			set => this.RaiseAndSetIfChanged(ref _filteredProducts, value);
		}
		
		private ObservableCollection<Resource> _filteredResources;
		public ObservableCollection<Resource> FilteredResources
		{
			get => _filteredResources;
			set => this.RaiseAndSetIfChanged(ref _filteredResources, value);
		}


		private string _searchResourceTitle;
		public string SearchResourceTitle
		{
			get => _searchResourceTitle;
			set => this.RaiseAndSetIfChanged(ref _searchResourceTitle, value);
		}

		private string _searchResourceQuantity;
		public string SearchResourceQuantity
		{
			get => _searchResourceQuantity;
			set => this.RaiseAndSetIfChanged(ref _searchResourceQuantity, value);
		}
		/// 
		/// 
		///

		private string _searchProductTitle;
		public string SearchProductTitle
		{
			get => _searchProductTitle;
			set => this.RaiseAndSetIfChanged(ref _searchProductTitle, value);
		}

		private string _searchProductCapacity;
		public string SearchProductCapacity
		{
			get => _searchProductCapacity;
			set => this.RaiseAndSetIfChanged(ref _searchProductCapacity, value);
		}

		private string _searchMinPrice;
		public string SearchMinPrice
		{
			get => _searchMinPrice;
			set => this.RaiseAndSetIfChanged(ref _searchMinPrice, value);
		}

		private string _searchMaxPrice;
		public string SearchMaxPrice
		{
			get => _searchMaxPrice;
			set => this.RaiseAndSetIfChanged(ref _searchMaxPrice, value);
		}

		public MainWindowViewModel()
		{
			// Ініціалізація взаємодії з діалогами
			ShowDialog = new Interaction<AddSupplierViewModel, Supplier?>();
			ShowDialogp = new Interaction<AddProductViewModel, Product?>();
			ShowDialogr = new Interaction<AddResourceViewModel, Resource?>();
			// Ініціалізація команд для додавання постачальників, продуктів та фільтрації продуктів
			AddSupplierCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				var Supplier = new AddSupplierViewModel();
				Supplier.mode = "insert";
				var result = await ShowDialog.Handle(Supplier);
				if (result != null)
				{
					string sql = string.Format("INSERT INTO Supplier (title, info) " +
							"VALUES ('{0}', '{1}');",
							 result.Title, result.Info);
					string logSql = string.Format("INSERT INTO Logs (text, timestamp) VALUES ('Додано постачальника: {0} {1}', '{2}');",
							result.Title, result.Info, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

					Database.Exec_SQL(sql);
					Database.Exec_SQL(logSql);
					TheSupplier.FillSupplier();
					TheLog.Fill_Logs();
				}
			});

			AddProductCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				var product = new AddProductViewModel();
				product.mode = "insert";
				var result = await ShowDialogp.Handle(product);
				if (result != null)
				{
					string insertProductSql = string.Format("INSERT INTO Products (Title, Capacity, Price, Quantity) " +
						"VALUES ('{0}', '{1}', '{2}', '{3}');",
						result.Title, result.Capacity, result.Price, result.Quantity);

					// Логіка для логування операції додавання продукту
					string logSql = string.Format("INSERT INTO Logs (text, timestamp) VALUES ('Додано продукт: {0}', '{1}');",
						result.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

					Database.Exec_SQL(insertProductSql);
					Database.Exec_SQL(logSql);
					TheProduct.Fill_Product();
					TheLog.Fill_Logs();
				}
				FilterProducts();
			});
			
		AddResourceCommand = ReactiveCommand.CreateFromTask(async () =>
{
	var resource = new AddResourceViewModel();
	resource.mode = "insert";
	
	// Отримання списку імен постачальників
  
	// Передача списку імен постачальників в модель представлення додавання ресурсу

	var result = await ShowDialogr.Handle(resource);
	if (result != null)
	{
		string insertResourceSql = string.Format("INSERT INTO resources (Title, Quantity, Supplier_title) " +
			"VALUES ('{0}', '{1}', '{2}');",
			result.Title,  result.Quantity, result.SupplierName);

		// Логіка для логування операції додавання ресурсу
		string logSql = string.Format("INSERT INTO Logs (text, timestamp) VALUES ('Додано матеріал: {0}', '{1}');",
			result.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

		Database.Exec_SQL(insertResourceSql);
		Database.Exec_SQL(logSql);
		TheResource.Fill_Resource();
		TheLog.Fill_Logs();
	}
	FilterResources();
});


			EditSupplierCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				if (SelectItem != null)
				{
					var Supplier = new AddSupplierViewModel();
					Supplier.mode = "edit";
					Supplier.SupplierItem = SelectItem;
					var result = await ShowDialog.Handle(Supplier);
					if (result != null)
					{
						string sql = string.Format("UPDATE Supplier SET Title = '{0}', " +
							"Info = '{1}' WHERE Id = '{2}';",
							result.Title, result.Info, SelectItem.Id);
						string logSql = string.Format("INSERT INTO Logs (text, Timestamp) VALUES ('Змінено постачальника: {0}', '{1}');",
							result.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
						Database.Exec_SQL(sql);
						Database.Exec_SQL(logSql);
						TheSupplier.FillSupplier();
						TheLog.Fill_Logs();
					}
				}
			});

			EditProductCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				if (SelectItemp != null)
				{
					var product = new AddProductViewModel();
					product.mode = "edit";
					product.ProductItem = SelectItemp;
					var result = await ShowDialogp.Handle(product);
					if (result != null)
					{
						string updateProductSql = string.Format("UPDATE Products SET " +
							"Title = '{0}', " +
							"Capacity= '{1}', " +
							"Price = '{2}', " +
							"Quantity = '{3}' " +
							"WHERE ProductId = '{4}';",
							result.Title, result.Capacity, result.Price, result.Quantity, SelectItemp.ProductId);

						// Логіка для логування операції редагування продукту
						string logSql = string.Format("INSERT INTO Logs (text, Timestamp) VALUES ('Змінено продукт: {0}', '{1}');",
							result.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

						Database.Exec_SQL(updateProductSql);
						Database.Exec_SQL(logSql);
						TheProduct.Fill_Product();
						TheLog.Fill_Logs();
					}
				}
				FilterProducts();
			});
			
			EditResourceCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				if (SelectItemr != null)
				{
					var resource = new AddResourceViewModel();
					resource.mode = "edit";
					resource.ResourceItem = SelectItemr;
					var result = await ShowDialogr.Handle(resource);
					if (result != null)
					{
						string updateResourceSql = string.Format("UPDATE resources SET " +
							"Title = '{0}', " +
							"Quantity = '{1}', " + 
							"supplier_title = '{2}' "+
							"WHERE ResourceId = '{3}';",
							result.Title, result.Quantity, result.SupplierName, SelectItemr.ResourceId);
						
						// Логіка для логування операції редагування продукту
						string logSql = string.Format("INSERT INTO Logs (text, Timestamp) VALUES ('Змінено ресурс: {0}', '{1}');",
							result.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

						Database.Exec_SQL(updateResourceSql);
						Database.Exec_SQL(logSql);
						TheResource.Fill_Resource();
						TheLog.Fill_Logs();
					}
				}
				FilterResources();
			});

			DeleteSupplierCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				if (SelectItem != null)
				{
					string sql = string.Format("DELETE FROM Supplier WHERE Title = '{0}';", SelectItem.Title);
					string logSql = string.Format("INSERT INTO Logs (text, timestamp) VALUES ('Видалено постачальника: {0}', '{1}');",
						SelectItemp.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					Database.Exec_SQL(sql);
					Database.Exec_SQL(logSql);
					TheSupplier.FillSupplier();
					TheLog.Fill_Logs();
				}
				
			});

			DeleteProductCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				if (SelectItemp != null)
				{
					string deleteProductSql = string.Format("DELETE FROM Products WHERE Title = '{0}';", SelectItemp.Title);

					// Логіка для логування операції видалення продукту
					string logSql = string.Format("INSERT INTO Logs (text, timestamp) VALUES ('Видалено продукт: {0}', '{1}');",
						SelectItemp.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

					Database.Exec_SQL(deleteProductSql);
					Database.Exec_SQL(logSql);
					TheProduct.Fill_Product();
					TheLog.Fill_Logs();
				}
				FilterProducts();
			});
			
			DeleteResourceCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				if (SelectItemr != null)
				{
					string deleteResourceSql = string.Format("DELETE FROM resources WHERE Title = '{0}';", SelectItemr.Title);

					// Логіка для логування операції видалення продукту
					string logSql = string.Format("INSERT INTO Logs (text, timestamp) VALUES ('Видалено матеріал: {0}', '{1}');",
						SelectItemr.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

					Database.Exec_SQL(deleteResourceSql);
					Database.Exec_SQL(logSql);
					TheResource.Fill_Resource();
					TheLog.Fill_Logs();
				}
				FilterResources();
			});

			SelectRowCommand = ReactiveCommand.Create<Supplier>(Supplier =>
			{
				SelectItem = Supplier;
			});

			SelectRowCommand = ReactiveCommand.Create<Product>(product =>
			{
				SelectItemp = product;
			});
			
			SelectRowCommand = ReactiveCommand.Create<Resource>(Resource =>
			{
				SelectItemr = Resource;
			});

			ClearDataCommand = ReactiveCommand.Create(() =>
			{
				SearchProductTitle = string.Empty;
				SearchProductCapacity = string.Empty;
				SearchMinPrice = null;
				SearchMaxPrice = null;
				FilteredProducts = new ObservableCollection<Product>(TheProduct);
			});
			SearchProductCommand = ReactiveCommand.Create(FilterProducts);
			
			FilterProducts();
			
			ClearDataCommandResource = ReactiveCommand.Create(() =>
			{
				SearchResourceTitle = string.Empty;
				SearchResourceQuantity = null;
				FilteredResources = new ObservableCollection<Resource>(TheResource);
			});
			SearchResourceCommand = ReactiveCommand.Create(FilterResources);

			FilterResources();

		}

		// Метод для фільтрації продуктів
		private void FilterProducts()
		{
			var filtered = TheProduct.Where(p =>
			{
				bool matchesName = string.IsNullOrEmpty(SearchProductTitle) || p.Title.Contains(SearchProductTitle, StringComparison.OrdinalIgnoreCase);
				bool matchesCapacity = string.IsNullOrEmpty(SearchProductCapacity) || int.TryParse(SearchProductCapacity, out int capacity) && p.Capacity == capacity;
				bool matchesMinPrice = string.IsNullOrEmpty(SearchMinPrice) || p.Price >= decimal.Parse(SearchMinPrice);
				bool matchesMaxPrice = string.IsNullOrEmpty(SearchMaxPrice) || p.Price <= decimal.Parse(SearchMaxPrice);

				return matchesName && matchesCapacity && matchesMinPrice && matchesMaxPrice;
			}).ToList();

			FilteredProducts = new ObservableCollection<Product>(filtered);
		}
		
			private void FilterResources()
		{
			var filtered = TheResource.Where(p =>
			{
				bool matchesTitle = string.IsNullOrEmpty(SearchResourceTitle) || p.Title.Contains(SearchResourceTitle, StringComparison.OrdinalIgnoreCase);
				bool matchesQuantity = string.IsNullOrEmpty(SearchResourceQuantity) || p.Quantity >=decimal.Parse(SearchResourceQuantity);
				

				return matchesTitle && matchesQuantity;
			}).ToList();

			FilteredResources= new ObservableCollection<Resource>(filtered);
		}

		// Властивості для обраного постачальника та продукту
		Supplier _selectItem = null;
		Product _selectItemp = null;
		Resource _selectItemr = null;
		public Supplier SelectItem
		{
			get { return _selectItem; }
			set
			{
				if (_selectItem != value)
				{
					this.RaiseAndSetIfChanged(ref _selectItem, value);
				}
			}
		}
		public Product SelectItemp
		{
			get { return _selectItemp; }
			set
			{
				if (_selectItemp != value)
				{
					this.RaiseAndSetIfChanged(ref _selectItemp, value);
				}
			}
		}
		public Resource SelectItemr
		{
			get { return _selectItemr; }
			set
			{
				if (_selectItemr != value)
				{
				 this.RaiseAndSetIfChanged(ref _selectItemr, value);
				}
			}
		}
	}
}
