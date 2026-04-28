using System.Collections;
namespace ListaDeCompras.ConsoleApp.Compartilhado;

public abstract class RepositorioBase
{
    protected ArrayList registros = new ArrayList();

    public void Cadastrar(EntidadeBase entidade)
    {
        registros.Add(entidade);
    }

    public bool Editar(string idSelecionado, EntidadeBase entidadeAtualizada)
    {
        EntidadeBase? registroSelecionado = SelecionarPorId(idSelecionado);

        if (registroSelecionado == null)
            return false;

        registroSelecionado.AtualizarDados(entidadeAtualizada);

        return true;
    }

    public bool Excluir(string idSelecionado)
    {
        EntidadeBase? registroSelecionado = SelecionarPorId(idSelecionado);

        if (registroSelecionado == null)
            return false;

        registros.Remove(registroSelecionado);

        return true;
    }

    public EntidadeBase? SelecionarPorId(string idSelecionado)
    {
        foreach (EntidadeBase registro in registros)
        {
            if (registro.Id == idSelecionado)
                return registro;
        }

        return null;
    }

    public ArrayList SelecionarTodos()
    {
        return registros;
    }
}