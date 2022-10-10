using Business.Notifications;

namespace Business.Interfaces;
public interface INotificador
{
    bool TemNotificacao();
    List<Notificacao> ObterNotificacoes();
    void Handler(Notificacao notificacao);
}
