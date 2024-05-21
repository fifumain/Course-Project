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
		
		        // Властивості для продуктів
        public ICommand AddResourceommand { get; }
        public ICommand EditResourceCommand { get; }
        public ICommand DeleteResourceCommand { get; }
        public Resource TheResource { get; } = new Resource();
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

		private string _searchProductTitle;
		public string SearchProductTitle
		{
			get => _searchProductTitle;
			set => this.RaiseAndSetIfChanged(ref _searchProductTitle, value);
		}

		private string _searchProductCategory;
		public string SearchProductCategory
		{
			get => _searchProductCategory;
			set => this.RaiseAndSetIfChanged(ref _searchProductCategory, value);
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
					string logSql = string.Format("INSERT INTO Logs (action, timestamp) VALUES ('Added Supplier: {0} {1}', '{2}');",
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
					string insertProductSql = string.Format("INSERT INTO Products (Title, Category, Price, Quantity) " +
						"VALUES ('{0}', '{1}', '{2}', '{3}');",
						result.Title, result.Category, result.Price, result.Quantity);

					// Логіка для логування операції додавання продукту
					string logSql = string.Format("INSERT INTO Logs (action, timestamp) VALUES ('Added product: {0}', '{1}');",
						result.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

					Database.Exec_SQL(insertProductSql);
					Database.Exec_SQL(logSql);
					TheProduct.Fill_Product();
					TheLog.Fill_Logs();
				}
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
						Database.Exec_SQL(sql);
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
							"Category = '{1}', " +
							"Price = '{2}', " +
							"Quantity = '{3}' " +
							"WHERE ProductId = '{4}';",
							result.Title, result.Category, result.Price, result.Quantity, SelectItemp.ProductId);

						// Логіка для логування операції редагування продукту
						string logSql = string.Format("INSERT INTO Logs (Action, Timestamp) VALUES ('Edited product: {0}', '{1}');",
							result.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

						Database.Exec_SQL(updateProductSql);
						Database.Exec_SQL(logSql);
						TheProduct.Fill_Product();
						TheLog.Fill_Logs();
					}
				}
			});

			DeleteSupplierCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				if (SelectItem != null)
				{
					string sql = string.Format("DELETE FROM Supplier WHERE Title = '{0}';", SelectItem.Title);
					Database.Exec_SQL(sql);
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
					string logSql = string.Format("INSERT INTO Logs (action, timestamp) VALUES ('Deleted product: {0}', '{1}');",
						SelectItemp.Title, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

					Database.Exec_SQL(deleteProductSql);
					Database.Exec_SQL(logSql);
					TheProduct.Fill_Product();
					TheLog.Fill_Logs();
				}
			});

			SelectRowCommand = ReactiveCommand.Create<Supplier>(Supplier =>
			{
				SelectItem = Supplier;
			});

			SelectRowCommand = ReactiveCommand.Create<Product>(product =>
			{
				SelectItemp = product;
			});

			ClearDataCommand = ReactiveCommand.Create(() =>
			{
				SearchProductTitle = string.Empty;
				SearchProductCategory = string.Empty;
				SearchMinPrice = null;
				SearchMaxPrice = null;
				FilteredProducts = new ObservableCollection<Product>(TheProduct);
			});
			SearchProductCommand = ReactiveCommand.Create(FilterProducts);

			FilterProducts();
		}

		// Метод для фільтрації продуктів
		private void FilterProducts()
		{
			var filtered = TheProduct.Where(p =>
			{
				bool matchesName = string.IsNullOrEmpty(SearchProductTitle) || p.Title.Contains(SearchProductTitle, StringComparison.OrdinalIgnoreCase);
				bool matchesCategory = string.IsNullOrEmpty(SearchProductCategory) || p.Category.Contains(SearchProductCategory, StringComparison.OrdinalIgnoreCase);
				bool matchesMinPrice = string.IsNullOrEmpty(SearchMinPrice) || p.Price >= decimal.Parse(SearchMinPrice);
				bool matchesMaxPrice = string.IsNullOrEmpty(SearchMaxPrice) || p.Price <= decimal.Parse(SearchMaxPrice);

				return matchesName && matchesCategory && matchesMinPrice && matchesMaxPrice;
			}).ToList();

			FilteredProducts = new ObservableCollection<Product>(filtered);
		}

		// Властивості для обраного постачальника та продукту
		Supplier _selectItem = null;
		Product _selectItemp = null;
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
	}
}
