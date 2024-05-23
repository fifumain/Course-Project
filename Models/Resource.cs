using System;
using System.Collections.Generic;
namespace course_project_filip.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string SupplierName { get; set; } // Список имен поставщиков

        // Свойство для отображения списка имен поставщиков как строки

    }
}
