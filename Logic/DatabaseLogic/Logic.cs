using System;
using System.Collections.Generic;
using Logic.Interfaces;
using Models;
using Repository.Interfaces;

namespace Logic
{
    public class Logic<T>: ILogic<T> where T: DatabaseItem
    {
        private readonly IRepository<T> repository;

        public Logic(IRepository<T> repository)
        {
            this.repository = repository;
        }
        
        public T Add(T entity)
        {
            return repository.Add(entity);
        }

        public T Read(KeyValuePair<string, Object> condition)
        {
            return repository.Read(condition);
        }
        
        public List<T> ReadAll(Dictionary<string, Object> conditions, string keyword = "OR")
        {
            return repository.ReadAll(conditions);
        }

        public T Update(KeyValuePair<string, Object> condition, KeyValuePair<string, Object> value)
        {
            return repository.Update(condition, value);
        }

        public bool Delete(KeyValuePair<string, Object> condition)
        {
            return repository.Delete(condition);
        }
        
        public bool Exists(KeyValuePair<string, Object> condition)
        {
            return repository.Exists(condition);
        }
    }
}