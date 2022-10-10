using Business.Interfaces;

namespace Business.Notifications;

public class Notificador : INotificador
{
    private List<Notificacao> _notificacoes;

    public Notificador() => _notificacoes = new List<Notificacao>();

    public void Handler(Notificacao notificacao) => _notificacoes.Add(notificacao);

    public List<Notificacao> ObterNotificacoes() => _notificacoes;
    
    public bool TemNotificacao() => _notificacoes.Any();    
}
