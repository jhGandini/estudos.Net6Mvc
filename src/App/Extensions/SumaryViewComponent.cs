using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Extensions;

public class SumaryViewComponent : ViewComponent
{
    private readonly INotificador _notificador;

    public SumaryViewComponent(INotificador notificador)
    {
        _notificador = notificador;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var notificacaoes = await Task.FromResult(_notificador.ObterNotificacoes());
        notificacaoes.ForEach(c => ViewData.ModelState.AddModelError(String.Empty, c.Mensagem));

        return View();
    }
}
