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
public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(ApplicationDbContext db) : base(db) { }
    public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.FornecedorId == fornecedorId);
    }
}
