using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.ViewModels;
using Business.Interfaces;
using AutoMapper;
using Business.Models;

namespace App.Controllers
{

    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        private readonly ILogger<FornecedoresController> _logger;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IMapper mapper, 
                                      IFornecedorService fornecedorService, INotificador notificador, 
                                      ILogger<FornecedoresController> logger) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
            _logger = logger;
        }

        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));
        }

        [Route("dados-do-fornecedor/{id:Guid}")]
        public async Task<IActionResult> Details(Guid id)
        {

            var fornecedorViewModel = await _fornecedorRepository.ObterFornecedorEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<FornecedorViewModel>(fornecedorViewModel));
        }

        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("novo-fornecedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return View(fornecedorViewModel);
            
            fornecedorViewModel.Id = Guid.NewGuid();
            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));
            if (!OperacaoValida()) return View(fornecedorViewModel);
            TempData["Sucesso"] = "Fornecedor cadastrado com sucesso";

            return RedirectToAction(nameof(Index));                       
        }

        [Route("editar-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await _fornecedorRepository.ObterFornecedorProdutoEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<FornecedorViewModel>(fornecedorViewModel));
        }

        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(fornecedorViewModel);

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));
            if (!OperacaoValida()) return View(fornecedorViewModel);

            TempData["Sucesso"] = "Fornecedor atualizado com sucesso";
            return RedirectToAction(nameof(Index));            
            
        }

        [Route("excluir-fornecedor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await _fornecedorRepository.ObterFornecedorEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<FornecedorViewModel>(fornecedorViewModel));
        }

        [Route("excluir-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = await _fornecedorRepository.ObterFornecedorEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            await _fornecedorService.Remover(id);
            if (!OperacaoValida()) return View(fornecedorViewModel);

            TempData["Sucesso"] = "Fornecedor removido com sucesso";
            return RedirectToAction(nameof(Index));
        }

        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
            if (fornecedor == null) return NotFound();
            return PartialView("_DetalhesEndereco", fornecedor);
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<ActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
            if (fornecedor == null) return NotFound();
            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });            
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));

            var url = Url.Action("ObterEndereco","Fornecedores",new {id = fornecedorViewModel.Endereco.FornecedorId});
            return Json(new {success = true, url });
        }

    }
}
