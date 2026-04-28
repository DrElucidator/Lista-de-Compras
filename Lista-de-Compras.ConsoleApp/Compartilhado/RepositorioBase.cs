namespace ListaDeCompras.ConsoleApp.Compartilhado;

public abstract class RepositorioBase
{
    protected EntidadeBase?[] registros = new EntidadeBase[100];

    public void Cadastrar(EntidadeBase entidade)
    {
        for (int i = 0; i < registros.Length; i++)
        {
            if (registros[i] == null)
            {
                registros[i] = entidade;
                break;
            }
        }
    }

    public bool Editar(string idSelecionado, EntidadeBase entidade)
    {
        EntidadeBase? entidadeSelecionada = SelecionarPorId(idSelecionado);

        if (entidadeSelecionada == null)
            return false;

        entidadeSelecionada.AtualizarRegistro(entidade);

        return true;
    }

    public bool Excluir(string idSelecionado)
    {
        for (int i = 0; i < registros.Length; i++)
        {
            EntidadeBase? c = registros[i];

            if (c == null)
                continue;

            if (c.Id == idSelecionado)
            {
                registros[i] = null;
                return true;
            }
        }

        return false;
    }

    public EntidadeBase? SelecionarPorId(string idSelecionado)
    {
        EntidadeBase? entidadeSelecionada = null;

        for (int i = 0; i < registros.Length; i++)
        {
            EntidadeBase? c = registros[i];

            if (c == null)
                continue;

            if (c.Id == idSelecionado)
            {
                entidadeSelecionada = c;
                break;
            }
        }

        return entidadeSelecionada;
    }

    public EntidadeBase?[] SelecionarTodos()
    {
        return registros;
    }
}