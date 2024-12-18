using Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Magazine : IDocument
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int ReleaseNumber { get; set; }
        public DateTime PublishDate { get; set; }
        public string Number => $"{Title}-{ReleaseNumber}";

        public string GetCardInfo() =>
            $"Magazine: {Title}, Publisher: {Publisher}, Release #: {ReleaseNumber}, Published: {PublishDate:yyyy-MM-dd}";
    }
}
