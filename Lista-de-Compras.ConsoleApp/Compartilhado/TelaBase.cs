using System.Net;
using ListaDeCompras.ConsoleApp.Utilidades;
using ListaDeCompras.ConsoleApp.Compartilhado.Validacao;
using ListaDeCompras.ConsoleApp.ModuloCategoria;

namespace ListaDeCompras.ConsoleApp.Compartilhado;

public abstract class TelaBase<T> : ITela where T : EntidadeBase
{
    public string nomeEntidade = string.Empty;
    protected RepositorioBase<T> repositorio;

    protected TelaBase(string nomeEntidade, RepositorioBase<T> repositorio)
    {
        this.nomeEntidade = nomeEntidade;
        this.repositorio = repositorio;
    }

    public virtual string? ObterOpcaoMenu()
    {
        string nomeMinusculo = nomeEntidade.ToLower();

        Console.Clear();
        CustomText.TextoColorido(
                                ("---------------------------------\n", null),
                                ($"Gestão de {nomeEntidade}\n", null),
                                ("---------------------------------\n", null),
                                ($"1 - Cadastro de {nomeMinusculo}\n", null),
                                ($"2 - Edição de {nomeMinusculo}\n", null),
                                ($"3 - Exclusão de {nomeMinusculo}\n", null),
                                ($"4 - Visualização de {nomeMinusculo}\n", null),
                                ("S - Voltar para o início\n", null),
                                ("---------------------------------", null)
                                );
        Console.Write("> ");
        return Console.ReadLine()?.ToUpper();
    }

    public void Cadastrar()
    {
        ExibirCabecalho($"Cadastro de {nomeEntidade}");

        T novaEntidade;

        do
        {
            novaEntidade = ObterDadosCadastrais();

            if (novaEntidade == null)
                return;
        
            if (!ValidarEntidade(novaEntidade))
            {
                Validar.MensagemContinuar();
                continue;
            }

            break;
        } while (true);

        repositorio.Cadastrar(novaEntidade);
    }

    public void Editar()
    {
        ExibirCabecalho($"Edição de {nomeEntidade}");
        VisualizarTodos(deveExibirCabecalho: false);

        Console.WriteLine("---------------------------------");

        string? idSelecionado;

        do
        {
            Console.Write("Digite o ID do registro que deseja editar: ");
            idSelecionado = Console.ReadLine()?.ToUpper();

            if (string.IsNullOrWhiteSpace(idSelecionado))
            {
                Validar.Aviso("Operação cancelada.", nomeEntidade);
                Validar.MensagemContinuar();
                return;
            }

            if (Validar.IdValido(idSelecionado, repositorio.SelecionarTodos(), nomeEntidade))
                break;
        } while (true);

        Console.WriteLine("---------------------------------");

        var parametroAtual = repositorio.SelecionarPorId(idSelecionado!);

        T novaEntidade;

        do
        {
            novaEntidade = ObterDadosEdicao(parametroAtual!);

            if (!ValidarEntidade(novaEntidade))
            {
                Validar.MensagemContinuar();
                continue;
            }

            break;
        } while (true);

        bool conseguiuEditar = repositorio.Editar(idSelecionado!, novaEntidade);

        if (!conseguiuEditar)
            return;
    }

    public void Excluir()
    {
        ExibirCabecalho($"Exclusão de {nomeEntidade}");
        VisualizarTodos(deveExibirCabecalho: false);

        Console.WriteLine("---------------------------------");

        string? idSelecionado;

        do
        {
            Console.Write("Digite o ID do registro que deseja excluir: ");
            idSelecionado = Console.ReadLine()?.ToUpper();

            if (string.IsNullOrWhiteSpace(idSelecionado))
            {
                Validar.Aviso("Operação cancelada.", nomeEntidade);
                Validar.MensagemContinuar();
                return;
            }

            if (Validar.IdValido(idSelecionado, repositorio.SelecionarTodos(), nomeEntidade))
                break;
        } while (true);

        bool conseguiuExcluir = repositorio.Excluir(idSelecionado!, ValidarVinculos);

        if (!conseguiuExcluir)
        {
            Validar.Erro("Não foi possível excluir o registro requisitado.", nomeEntidade);
            Validar.MensagemContinuar();
            return;
        }
    }

    public abstract void VisualizarTodos(bool deveExibirCabecalho);

    protected void ExibirCabecalho(string titulo)
    {
        Console.Clear();
        CustomText.TextoColorido(
                                ("---------------------------------\n", null),
                                ($"Gestão de {nomeEntidade}\n", null),
                                ("---------------------------------\n", null),
                                ($"{titulo}\n", null),
                                ("---------------------------------", null)
                                );
    }

    protected abstract T ObterDadosCadastrais();
    protected abstract T ObterDadosEdicao(T Atual);
    protected virtual Func<T, bool>? ValidarVinculos => null;
    protected virtual bool ValidarEntidade(T entidade) => true;
}