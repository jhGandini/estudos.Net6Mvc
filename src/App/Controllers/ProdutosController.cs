using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.ViewModels;
using AutoMapper;
using Business.Models;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using App.Extensions;

namespace App.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoRepository _repository;
        private readonly IProdutoService _produtoService;
        public readonly IMapper _mapper;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutoRepository repository, IMapper mapper, 
                                  IFornecedorRepository fornecedorRepository, IProdutoService produtoService, 
                                  INotificador notificador, ILogger<ProdutosController> logger) : base(notificador)
        {
            _repository = repository;
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
            _produtoService = produtoService;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            var a = await _repository.ObterProdutosFornecedores();

            var meuContext = _mapper.Map<IEnumerable<ProdutoViewModel>>(a);
            return View(meuContext);
        }

        [AllowAnonymous]
        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await _repository.ObterProdutoFornecedor(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProdutoViewModel>(produtoViewModel));
        }

        [ClaimsAuthorize("Produto","Adicionar")]
        [Route("novo-produto")]
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["FornecedorId"] = await ListaFornecedoresAsync();
            return View();
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            ViewData["FornecedorId"] = await ListaFornecedoresAsync(produtoViewModel.FornecedorId);
            if (!ModelState.IsValid) return View(produtoViewModel);
            
            produtoViewModel.Id = Guid.NewGuid();

            var imgPrefixo = Guid.NewGuid() + "_";
            if(!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return View(produtoViewModel);
            }

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));     
            if(!OperacaoValida()) return View(produtoViewModel);
            TempData["Sucesso"] = "Produto cadastrado com sucesso";

            return RedirectToAction(nameof(Index));                                    
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {            
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _repository.ObterPorId(id));
            if (produtoViewModel == null)
            {
                return NotFound();
            }            

            ViewData["FornecedorId"] = await ListaFornecedoresAsync(produtoViewModel.FornecedorId);
            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
            {
                return NotFound();
            }

            ViewData["FornecedorId"] = await ListaFornecedoresAsync(produtoViewModel.FornecedorId);            
            if (!ModelState.IsValid) return View(produtoViewModel);
            
            if(produtoViewModel.ImagemUpload != null) { 
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(produtoViewModel);
                }
                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }
            
            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoViewModel));
            if (!OperacaoValida()) return View(produtoViewModel);
            TempData["Sucesso"] = "Produto alterado com sucesso";

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            
            var produtoViewModel = await _repository.ObterProdutoFornecedor(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProdutoViewModel>(produtoViewModel));
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await _repository.ObterProdutoFornecedor(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            await _produtoService.Remover(id);
            if (!OperacaoValida()) return View(produtoViewModel);
            TempData["Sucesso"] = "Produto removido com sucesso";

            return RedirectToAction(nameof(Index));            
        }

        private async Task<SelectList> ListaFornecedoresAsync(Guid? id = null)
        {
            return new SelectList(await _fornecedorRepository.ObterTodos(), "Id", "Nome", id);
        }

        private bool ProdutoViewModelExists(Guid id)
        {
          return _repository.ObterPorId(id) != null;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string ImgPrefixo)
        {
            if (arquivo.Length <= 0) return false;
            var patc = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", ImgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(patc))
            {
                ModelState.AddModelError(string.Empty, "Ja existe um arquivo com esse nome!");
                return false;
            }

            using(var stream = new FileStream(patc, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
