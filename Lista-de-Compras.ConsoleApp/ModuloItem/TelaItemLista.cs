using ListaDeCompras.ConsoleApp.Compartilhado;
using ListaDeCompras.ConsoleApp.Compartilhado.Validacao;
using ListaDeCompras.ConsoleApp.ModuloCategoria;
using ListaDeCompras.ConsoleApp.ModuloProduto;
using ListaDeCompras.ConsoleApp.ModuloListaCompras;
using ListaDeCompras.ConsoleApp.Utilidades;

namespace ListaDeCompras.ConsoleApp.ModuloItem;

public class TelaItemLista : TelaBase<ItemLista>
{
    private readonly RepositorioBase<Produto> repositorioProduto;
    private readonly RepositorioBase<Categoria> repositorioCategoria;
    private readonly RepositorioBase<ListaCompras> repositorioLista;
    private readonly RepositorioBase<ItemLista> repositorioItem;
    public TelaItemLista(RepositorioBase<ItemLista> repositorioItem, RepositorioBase<Produto> repositorioProduto, RepositorioBase<Categoria> repositorioCategoria, RepositorioBase<ListaCompras> repositorioLista) : base("Item da Lista", repositorioItem)
    {
        this.repositorioItem = repositorioItem;
        this.repositorioProduto = repositorioProduto;
        this.repositorioCategoria = repositorioCategoria;
        this.repositorioLista = repositorioLista;
    }

    public override void VisualizarTodos(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho($"Visualização de {nomeEntidade}");

        var telaLista = new TelaLista(repositorioLista, repositorioItem, repositorioProduto);
        telaLista.VisualizarTodos(false);

        string idLista = Validar.LerId("Digite o ID da lista para visualizar os itens: ", repositorioLista.SelecionarTodos(), "Lista");

        var itens = repositorio.SelecionarTodos().Where(item => item.IdLista == idLista).ToList();

        if (itens.Count == 0)
        {
            Validar.Aviso("Nenhum item cadastrado nesta lista.");
            Validar.MensagemContinuar();
            return;
        }

        foreach (var item in itens)
        {
            var produto = repositorioProduto.SelecionarPorId(item.IdProduto);
            var categoria = repositorioCategoria.SelecionarPorId(produto?.IdCategoria ?? "");

            CustomText.TextoColorido(
                ($"{item.Id} : {produto?.Nome} | Qtd: {item.Quantidade} | Categoria: ", null),
                ($"{categoria?.Nome}", categoria?.Cor)
            );
        }

        if (deveExibirCabecalho)
            Validar.MensagemContinuar();
    }

    protected override ItemLista ObterDadosCadastrais()
    {
        var telaLista = new TelaLista(repositorioLista, repositorioItem, repositorioProduto);
        telaLista.VisualizarTodos(false);

        string idLista = Validar.LerId("Digite o ID da lista: ", repositorioLista.SelecionarTodos(), "Lista");

        string idProduto;
        do
        {
            var telaProduto = new TelaProduto(repositorioProduto, repositorioCategoria);
            telaProduto.VisualizarTodos(false);

            idProduto = Validar.LerId("Digite o ID do produto: ", repositorioProduto.SelecionarTodos(), "Produto");

            if (string.IsNullOrWhiteSpace(idLista))
                return null;

            bool duplicado = repositorio.SelecionarTodos().Any(item => item.IdLista == idLista && item.IdProduto == idProduto);

            if (!duplicado) break;

            Validar.Erro("Este produto já está na lista.", "Item");
        } while (true);

        int quantidade = Validar.LerQuantidade("Digite a quantidade: ", "Quantidade");

        var novoItem = new ItemLista(idLista, idProduto, quantidade);

        AtualizarTotaisLista(idLista);

        return novoItem;
    }

    protected override ItemLista ObterDadosEdicao(ItemLista atual)
    {
        int quantidade = Validar.LerQuantidade("Digite a quantidade: ", "Quantidade");

        var atualizado = new ItemLista(atual.IdLista, atual.IdProduto, quantidade);

        AtualizarTotaisLista(atual.IdLista);

        return atualizado;
    }

    protected override bool ValidarEntidade(ItemLista item)
    {
        bool valido = true;

        valido &= Validar.IdValido(item.IdLista, repositorioLista.SelecionarTodos(), "Lista");
        valido &= Validar.IdValido(item.IdProduto, repositorioProduto.SelecionarTodos(), "Produto");
        valido &= Validar.NumeroPositivo(item.Quantidade, "Quantidade");

        return valido;
    }

    private void AtualizarTotaisLista(string idLista)
    {
        var lista = repositorioLista.SelecionarPorId(idLista);
        if (lista == null) return;

        var itens = repositorio.SelecionarTodos().Where(item => item.IdLista == idLista).ToList();

        lista.TotalItens = itens.Count;
        lista.TotalGastoEstimado = itens.Sum(item =>
        {
            var produto = repositorioProduto.SelecionarPorId(item.IdProduto);
            return (produto?.Preco ?? 0) * item.Quantidade;
        });
    }
}