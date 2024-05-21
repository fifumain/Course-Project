using course_project_filip.Models;
using Microsoft.VisualBasic;
using ReactiveUI;
using System.Reactive;


namespace course_project_filip.ViewModels
{
    public class AddSupplierViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Supplier?> SaveSupplier { get; }
        public Supplier SupplierItem;
        public string mode = "insert";

        public AddSupplierViewModel()
        {
            SaveSupplier = ReactiveCommand.Create(() =>
            {
                SupplierItem = new Supplier();
                SupplierItem.Title= Title;
                SupplierItem.Info = Info;
   
                return SupplierItem;
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

        string _info = "";
        public string Info 
        {
            get { return _info; }
            set
            {
                if (_info != value)
                {
                    this.RaiseAndSetIfChanged(ref _info, value);
                }
            }
        }

    } }