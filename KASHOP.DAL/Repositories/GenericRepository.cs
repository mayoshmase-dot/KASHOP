using KASHOP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.DAL.Repositories
{
    public class GenericRepository <T> where T : class
    {
        ApplicationDbContext context = new ApplicationDbContext(); 
        public void Create (T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }
        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }
        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }
        public List<T> Get()
        {
            var entities = context.Set<T>().AsNoTracking().ToList();
            return entities;
        }
    }
}
