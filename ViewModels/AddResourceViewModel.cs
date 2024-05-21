using course_project_filip.Models;
using Microsoft.VisualBasic;
using ReactiveUI;
using System.Reactive;


namespace course_project_filip.ViewModels
{
	public class AddResourceViewModel : ViewModelBase
	{
		public ReactiveCommand<Unit, Resource?> SaveResource { get; }
		public Resource ResourceItem;
		public string mode = "insert";

		public AddResourceViewModel()
		{
			SaveResource = ReactiveCommand.Create(() =>
			{
				ResourceItem = new Resource();
				ResourceItem.Title= Title;
				ResourceItem.Quantity = Quantity;
	
				return ResourceItem;
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
		
		int _quantity = 0;
		public int Quantity
		{
			get { return _quantity; }
			set
			{
				if (_quantity!= value)
				{
					this.RaiseAndSetIfChanged(ref _quantity, value);
				}
			}
		}
	} }
