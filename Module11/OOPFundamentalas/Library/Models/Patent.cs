using Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Patent : IDocument
    {
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string UniqueId { get; set; }
        public string Number => UniqueId;

        public string GetCardInfo() =>
            $"Patent: {Title}, Authors: {string.Join(", ", Authors)}, Published: {DatePublished.ToShortDateString()}, Expires: {ExpirationDate.ToShortDateString()}";
    }
}
