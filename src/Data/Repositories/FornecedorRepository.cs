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
public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
{
    public FornecedorRepository(ApplicationDbContext db) : base(db) { }
    public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
    {
        return await _dbSet.AsNoTracking().Include(e => e.Endereco).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Fornecedor> ObterFornecedorProdutoEndereco(Guid id)
    {
        return await _dbSet.AsNoTracking().Include(e => e.Endereco).Include(p => p.Produtos).FirstOrDefaultAsync(x => x.Id == id);
    }
}
