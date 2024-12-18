using Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class LocalizedBook : IDocument
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public int NumberOfPages { get; set; }
        public string OriginalPublisher { get; set; }
        public string Country { get; set; }
        public string LocalPublisher { get; set; }
        public DateTime DatePublished { get; set; }
        public string Number => ISBN;

        public string GetCardInfo() =>
            $"Localized Book: {Title}, Authors: {string.Join(", ", Authors)}, Pages: {NumberOfPages}, Original Publisher: {OriginalPublisher}, Local Publisher: {LocalPublisher}, Country: {Country}, Published: {DatePublished.ToShortDateString()}";
    }
}
