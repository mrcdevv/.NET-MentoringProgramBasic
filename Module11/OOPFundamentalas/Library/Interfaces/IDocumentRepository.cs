using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface IDocumentRepository
    {
        Task<List<IDocument>> SearchByNumberAsync(string number);
    }
}
