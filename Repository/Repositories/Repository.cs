using System;
using System.Collections.Generic;
using DAL.Interfaces;
using Repository.Interfaces;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IContext<T> _context;

        protected Repository(IContext<T> context)
        {
            _context = context;
        }
        
        public T Add(T entity)
        {
            return _context.Add(entity);
        }

        public T Read(KeyValuePair<string, Object> condition)
        {
            return _context.Read(condition);
        }
        
        public List<T> ReadAll(Dictionary<string, Object> conditions, string keyword = "OR")
        {
            return _context.ReadAll(conditions);
        }

        public T Update(KeyValuePair<string, Object> condition, KeyValuePair<string, Object> value)
        {
            return _context.Update(condition, value);
        }

        public bool Delete(KeyValuePair<string, Object> condition)
        {
            return _context.Delete(condition);
        }
        
        public bool Exists(KeyValuePair<string, Object> keyValuePair)
        {
            return _context.Exists(keyValuePair);
        }
    }
}