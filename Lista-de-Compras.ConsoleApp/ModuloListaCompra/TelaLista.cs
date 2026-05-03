using ListaDeCompras.ConsoleApp.Compartilhado;
using ListaDeCompras.ConsoleApp.Compartilhado.Validacao;
using ListaDeCompras.ConsoleApp.Utilidades;

namespace ListaDeCompras.ConsoleApp.ModuloLista;

public class TelaLista : TelaBase<ListaDeCompras>
{
    public TelaLista(RepositorioBase<ListaDeCompras> repositorioLista) : base("Lista de Compras", repositorioLista) { }

    public override void VisualizarTodos(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho($"Visualização de {nomeEntidade}");

        var listas = repositorio.SelecionarTodos();

        if (listas.Count == 0)
        {
            Validar.Aviso("Nenhuma lista cadastrada.");
            Validar.MensagemContinuar();
            return;
        }

        foreach (var lista in listas)
        {
            CustomText.TextoColorido(
                ($"{lista.Id} : {lista.Nome} | Status: {lista.Status} | Itens: {lista.TotalItens} | Total Estimado: {lista.TotalGastoEstimado.ToString("C2")}", null)
            );
        }

        if (deveExibirCabecalho)
            Validar.MensagemContinuar();
    }

    protected override ListaDeCompras ObterDadosCadastrais()
    {
        string nome;
        do
        {
            Write("Digite o nome da lista: ");
            nome = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nome))
            {
                Validar.Aviso("Operação cancelada.", "Lista");
                return null!;
            }

            if (Validar.CampoObrigatorio(nome, "Nome") &&
                Validar.Tamanho(nome, "Nome", 3, 100) &&
                Validar.Duplicado(nome, "Nome", repositorio.SelecionarTodos()))
                break;

        } while (true);

        return new ListaDeCompras(nome);
    }

    protected override ListaDeCompras ObterDadosEdicao(ListaDeCompras atual)
    {
        string nome;
        do
        {
            Write("Digite o nome da lista: ");
            nome = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nome))
            {
                nome = atual.Nome;
                break;
            }

            if (Validar.CampoObrigatorio(nome, "Nome") && Validar.Tamanho(nome, "Nome", 3, 100))
                break;
        } while (true);

        string status;
        do
        {
            WriteLine("Selecione o status da lista:");
            WriteLine("1 - Aberta");
            WriteLine("2 - Concluída");
            Write("> ");
            string? opcao = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(opcao))
            {
                status = atual.Status;
                break;
            }

            if (opcao == "1")
            {
                status = "Aberta";
                break;
            }
            else if (opcao == "2")
            {
                status = "Concluída";
                break;
            }
            else
            {
                Validar.Erro("Opção inválida. Digite 1 ou 2.", "Lista");
            }
        } while (true);

        return new ListaDeCompras(nome)
        {
            Status = status,
            TotalItens = atual.TotalItens,
            TotalGastoEstimado = atual.TotalGastoEstimado
        };
    }

    protected override bool ValidarEntidade(ListaDeCompras lista)
    {
        bool valido = true;

        valido &= Validar.CampoObrigatorio(lista.Nome, "Nome");
        valido &= Validar.Tamanho(lista.Nome, "Nome", 3, 100);

        if (lista.Status != "Aberta" && lista.Status != "Concluída")
        {
            Validar.Erro("Status inválido. Deve ser 'Aberta' ou 'Concluída'.", "Lista");
            valido = false;
        }

        return valido;
    }
}