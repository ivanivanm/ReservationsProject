using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDb<T,K>
    {
        void Create(T entity,bool useNavigationalProperties);
        T Read(K key, bool useNavigationalProperties, bool isReadOnlyTrue);
        List<T> ReadAll(bool useNavigationalProperties, bool isReadOnlyTrue);

        void Update(K key,T entity, bool useNavigationalProperties);
        void Delete(K key);
    }
}
