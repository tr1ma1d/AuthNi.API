using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthNi.Core.Abstractions
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(int id, T entity);
        Task<bool> RemoveAsync(int id);
    }
}
