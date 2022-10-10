using App.ViewModels;
using AutoMapper;
using Business.Models;

namespace App.Automapper;

public class AutomapperConfig : Profile
{
	public AutomapperConfig()
	{
		CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
        CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
        CreateMap<Produto, ProdutoViewModel>().ReverseMap();
    }
}
