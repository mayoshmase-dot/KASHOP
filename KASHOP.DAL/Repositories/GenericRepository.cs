using KASHOP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.DAL.Repositories
{
    public class GenericRepository <T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

public GenericRepository (ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create (T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
        public List<T> Get()
        {
            var entities = _context.Set<T>().AsNoTracking().ToList();
            return entities;
        }
    }
}
