using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using course_project_filip.Models;
using course_project_filip.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;

namespace course_project_filip;
	
public partial class AddProduct : ReactiveWindow<AddProductViewModel>
{
	public AddProduct()
	{
		InitializeComponent();
		this.WhenActivated(OnActivated);
		var bCancel = this.FindControl<Button>("bCancel");
		bCancel.Click += BCancel_Click;
	}

	private void OnActivated(CompositeDisposable disposables)
	{
		ViewModel!.SaveProduct.Subscribe(Close).DisposeWith(disposables);
		if (ViewModel!.mode == "edit")
		{
			ViewModel!.Title = ViewModel!.ProductItem.Title;
			ViewModel!.Category = ViewModel!.ProductItem.Category;
			ViewModel!.Price = ViewModel!.ProductItem.Price;
			ViewModel!.Quantity= ViewModel!.ProductItem.Quantity;
			
			
		}
	}
	private void BCancel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			Close(); // Закрываем текущее окно
		}
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var text = textBox.Text;

                if (!string.IsNullOrEmpty(text))
                {
                    // Используем регулярное выражение для фильтрации только цифр
                    if (!Regex.IsMatch(text, "^[0-9]*$"))
                    {
                        // Удаляем все нечисловые символы
                        textBox.Text = Regex.Replace(text, "[^0-9]", "");
                        
                        // Перемещаем курсор в конец текста
                        textBox.CaretIndex = textBox.Text.Length;
                    }
                }
            }
        }

}