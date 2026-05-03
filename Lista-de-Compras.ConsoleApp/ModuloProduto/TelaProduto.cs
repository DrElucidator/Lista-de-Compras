using ListaDeCompras.ConsoleApp.Compartilhado;
using ListaDeCompras.ConsoleApp.Compartilhado.Validacao;
using ListaDeCompras.ConsoleApp.ModuloCategoria;
using ListaDeCompras.ConsoleApp.Utilidades;

namespace ListaDeCompras.ConsoleApp.ModuloProduto;

public class TelaProduto : TelaBase<Produto>
{
    private readonly RepositorioBase<Categoria> repositorioCategoria;

    public TelaProduto(RepositorioBase<Produto> repositorioProduto, RepositorioBase<Categoria> repositorioCategoria) : base("Produto", repositorioProduto)
    {
        this.repositorioCategoria = repositorioCategoria;
    }

    public override void VisualizarTodos(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho($"Visualização de {nomeEntidade}");

        var produtos = repositorio.SelecionarTodos();

        if (produtos.Count == 0)
        {
            Validar.Aviso("Nenhum produto cadastrado.");
            Validar.MensagemContinuar();
            return;
        }

        foreach (var produto in produtos)
        {
            var categoria = repositorioCategoria.SelecionarPorId(produto.IdCategoria);
            CustomText.TextoColorido(($"{produto.Id} : {produto.Nome} | {produto.Preco.ToString("C2")} | ", null), ($"{categoria?.Nome}", categoria?.Cor));
        }

        if (deveExibirCabecalho)
            Validar.MensagemContinuar();
    }

    protected override Produto ObterDadosCadastrais()
    {
        var telaCategoria = new TelaCategoria(repositorioCategoria, repositorio);
        telaCategoria.VisualizarTodos(false);

        string idCategoria = Validar.LerId("Digite o ID da categoria do produto: ", repositorioCategoria.SelecionarTodos(), "Categoria");

        string nome = Validar.LerCampoObrigatorio("Digite o nome do produto: ", "Nome", 2, 100, repositorio.SelecionarTodos());

        string medida = Validar.LerCampoObrigatorio("Digite a unidade de medida do produto: ", "Medida", 2, 50, repositorio.SelecionarTodos());

        double preco = Validar.LerPreco("Digite o preço do produto: ");

        return new Produto(nome, medida, preco, idCategoria);
    }

    protected override Produto ObterDadosEdicao(Produto atual)
    {
        string nome = Validar.LerCampoOpcional("Digite o nome do produto: ", atual.Nome, "Nome", 2, 100);
        string medida = Validar.LerCampoOpcional("Digite a unidade de medida do produto: ", atual.Medida, "Medida", 2, 50);
        double preco = Validar.LerPrecoOpcional("Digite o preço do produto: ", atual.Preco);

        return new Produto(nome, medida, preco, atual.IdCategoria);
    }

    protected override bool ValidarEntidade(Produto produto)
    {
        bool valido = true;

        valido &= Validar.CampoObrigatorio(produto.Nome, "Nome");
        valido &= Validar.Tamanho(produto.Nome, "Nome", 2, 100);
        valido &= Validar.CampoObrigatorio(produto.Medida, "Medida");
        valido &= Validar.Tamanho(produto.Medida, "Unidade de Medida", 2, 50);
        valido &= Validar.NumeroPositivo(produto.Preco, "Preço", "Preço");
        valido &= Validar.IdValido(produto.IdCategoria, repositorioCategoria.SelecionarTodos(), "Categoria");

        return valido;
    }
}