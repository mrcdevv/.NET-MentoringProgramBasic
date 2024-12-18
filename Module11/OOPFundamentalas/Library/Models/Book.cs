using Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Book : IDocument
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public int NumberOfPages { get; set; }
        public string Publisher { get; set; }
        public DateTime DatePublished { get; set; }
        public string Number => ISBN;

        public string GetCardInfo() =>
            $"Book: {Title}, Authors: {string.Join(", ", Authors)}, Pages: {NumberOfPages}, Publisher: {Publisher}, Published: {DatePublished.ToShortDateString()}";
    }
}
