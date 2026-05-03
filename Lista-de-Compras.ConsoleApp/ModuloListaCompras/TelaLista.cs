using ListaDeCompras.ConsoleApp.Compartilhado;
using ListaDeCompras.ConsoleApp.Compartilhado.Validacao;
using ListaDeCompras.ConsoleApp.ModuloProduto;
using ListaDeCompras.ConsoleApp.ModuloItem;
using ListaDeCompras.ConsoleApp.Utilidades;

namespace ListaDeCompras.ConsoleApp.ModuloListaCompras;

public class TelaLista : TelaBase<ListaCompras>
{
    private readonly RepositorioBase<ItemLista> repositorioItens;
    private readonly RepositorioBase<Produto> repositorioProduto;

    public TelaLista(RepositorioBase<ListaCompras> repositorioLista, RepositorioBase<ItemLista> repositorioItens, RepositorioBase<Produto> repositorioProduto) : base("Lista de Compras", repositorioLista)
    {
        this.repositorioItens = repositorioItens;
        this.repositorioProduto = repositorioProduto;
    }

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
            var itens = repositorioItens.SelecionarTodos().Where(item => item.IdLista == lista.Id).ToList();

            lista.TotalItens = itens.Count;

            double total = 0.0;
            foreach (var item in itens)
            {
                var produto = repositorioProduto.SelecionarPorId(item.IdProduto);
                if (produto == null)
                    continue;

                total += produto.Preco * item.Quantidade;
            }

            lista.TotalGastoEstimado = total;

            CustomText.TextoColorido(
                ($"{lista.Id} : {lista.Nome} | Status: {lista.Status} | Itens: {lista.TotalItens} | Total Estimado: {lista.TotalGastoEstimado.ToString("C2")}", null)
            );
        }

        if (deveExibirCabecalho)
            Validar.MensagemContinuar();
    }

    protected override ListaCompras ObterDadosCadastrais()
    {
        string nome = Validar.LerCampoObrigatorio("Digite o nome da lista: ", "Nome", 3, 100, repositorio.SelecionarTodos());

        return new ListaCompras(nome);
    }

    protected override ListaCompras ObterDadosEdicao(ListaCompras atual)
    {
        string nome = Validar.LerCampoOpcional("Digite o nome da lista: ", atual.Nome, "Nome", 3, 100);

        string status = Validar.LerStatus("Selecione o status da lista (1 - Aberta, 2 - Concluída): ", atual.Status);

        return new ListaCompras(nome)
        {
            Status = status,
            TotalItens = atual.TotalItens,
            TotalGastoEstimado = atual.TotalGastoEstimado
        };
    }

    protected override Func<ListaCompras, bool>? ValidarVinculos => lista => repositorioItens.SelecionarTodos().Any(item => item.IdLista == lista.Id);

    protected override bool ValidarEntidade(ListaCompras lista)
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