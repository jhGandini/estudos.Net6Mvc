using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories;
public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly ApplicationDbContext _applicationDbContext;
    protected readonly DbSet<TEntity> _dbSet;

    protected Repository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _dbSet = applicationDbContext.Set<TEntity>();
    }

    public virtual async Task<TEntity> ObterPorId(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<List<TEntity>> ObterTodos()
    {
        return await _dbSet.ToListAsync();
    }
    public  virtual async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public virtual async Task Adicionar(TEntity entity)
    {
        _dbSet.Add(entity);
        await SaveChages();
    }

    public virtual async Task Atualizar(TEntity entity)
    {
        _dbSet.Update(entity);
        await SaveChages();
    }

    public virtual async Task Remover(Guid id)
    {
        //_dbSet.Remove(await _dbSet.FindAsync(id));
        _dbSet.Remove(new TEntity { Id = id});
        await SaveChages();
    }

    public async Task<int> SaveChages()
    {
        return await _applicationDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _applicationDbContext?.Dispose();
    }
}
