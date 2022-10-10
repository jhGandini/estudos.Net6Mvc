using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories;
public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(ApplicationDbContext db) : base(db){}

    public async Task<Produto> ObterProdutoFornecedor(Guid id)
    {
        return await _dbSet.AsNoTracking().Include(f => f.Fornecedor).FirstOrDefaultAsync(x=>x.Id == id);
    }

    public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
    {
        return await _dbSet.AsNoTracking().Include(f => f.Fornecedor).ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
    {
        return await Buscar(x => x.FornecedorId == fornecedorId);
    }
}
