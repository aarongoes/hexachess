using System;
using System.Collections.Generic;
using DAL.Interfaces;
using Models;

namespace DAL.Memory.Contexts
{
    public abstract class MemoryContext<T> : IContext<T> where T : DatabaseItem
    {
        protected static List<T> Items { get; set; }

        public MemoryContext()
        {
            Items = new List<T>();
        }

        public T Add(T entity)
        {
            Items.Add(entity);
            entity.Id = Items.IndexOf(entity);
            return entity;
        }

        public T Read(KeyValuePair<string, Object> condition)
        {
            foreach (var obj in Items)
            {
                foreach (var property in obj.GetType().GetProperties())
                {
                    if (property.GetValue(obj) != null)
                    {
                        var s = property.Name;
                        var f = condition.Key;
                        var wq = property.GetValue(obj);
                        var q = condition.Value;
                        if (property.Name == condition.Key &&
                            property.GetValue(obj).ToString() == condition.Value.ToString())
                        {
                            return obj;
                        }
                    }
                }
            }
            return null;
        }

        public List<T> ReadAll(Dictionary<string, Object> conditions, string keyword = "OR")
        {
            List<T> returnvalue = new List<T>();
            foreach (var obj in Items)
            {
                foreach (var property in obj.GetType().GetProperties())
                {
                    if (property.GetValue(obj) != null)
                    {
                        foreach (var item in conditions)
                        {
                            if (property.Name == item.Key &&
                                property.GetValue(obj).ToString() == item.Value.ToString())
                            {
                                returnvalue.Add(obj);
                            }
                        }
                        
                    }
                }
            }
            return returnvalue;
        }

        public T Update(KeyValuePair<string, Object> condition, KeyValuePair<string, Object> value)
        {
            var model = Read(condition);
            typeof(Game).GetProperty(value.Key).SetValue(model, value.Value);
            return model;
        }

        public bool Delete(KeyValuePair<string, Object> condition)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(KeyValuePair<string, Object> condition)
        {
            foreach (var obj in Items)
            {
                foreach (var property in obj.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(T).GetProperty(condition.Key).PropertyType &&
                        property.GetValue(obj).ToString() == condition.Value.ToString())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}