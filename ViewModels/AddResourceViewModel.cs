using course_project_filip.Models;
using ReactiveUI;
using System;
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
                ResourceItem.Title = Title;
                ResourceItem.Quantity = Quantity;
                ResourceItem.SupplierName = SupplierName;

                return ResourceItem;
            });
        }

        private string _title = "";
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set => this.RaiseAndSetIfChanged(ref _quantity, value);
        }

        private string _supplierName = "";
        public string SupplierName
        {
            get => _supplierName;
            set => this.RaiseAndSetIfChanged(ref _supplierName, value);
        }
    }
}
