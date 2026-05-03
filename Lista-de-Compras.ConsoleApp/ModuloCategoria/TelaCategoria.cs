using ListaDeCompras.ConsoleApp.Compartilhado;
using ListaDeCompras.ConsoleApp.Compartilhado.Validacao;
using ListaDeCompras.ConsoleApp.ModuloProduto;
using ListaDeCompras.ConsoleApp.Utilidades;

namespace ListaDeCompras.ConsoleApp.ModuloCategoria;

public class TelaCategoria : TelaBase<Categoria>
{
    private readonly RepositorioBase<Produto> repositorioProduto;

    public TelaCategoria(RepositorioBase<Categoria> repositorio, RepositorioBase<Produto> repositorioProduto) : base("Categoria", repositorio)
    {
        this.repositorioProduto = repositorioProduto;
    }

    public override void VisualizarTodos(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho($"Visualização de {nomeEntidade}");

        var categorias = repositorio.SelecionarTodos();

        if (categorias.Count == 0)
        {
            Validar.Aviso("Nenhuma categoria cadastrada.");
            Validar.MensagemContinuar();
            return;
        }

        foreach (var categoria in categorias)
            CustomText.TextoColorido($"{categoria.Id}: {categoria.Nome}", categoria.Cor);

        if (deveExibirCabecalho)
            Validar.MensagemContinuar();
    }

    protected override Categoria ObterDadosCadastrais()
    {
        string nome = Validar.LerCampoObrigatorio("Digite o nome da categoria: ", "Nome", 3, 50, repositorio.SelecionarTodos());

        string cor = Validar.LerCor("Digite a cor da categoria: ", "Cor");

        return new Categoria(nome, cor);
    }

    protected override Categoria ObterDadosEdicao(Categoria atual)
    {
        string nome = Validar.LerCampoOpcional("Digite o nome da categoria: ", atual.Nome, "Nome", 3, 50);
        string cor = Validar.LerCorOpcional("Digite a cor da categoria: ", atual.Cor);

        return new Categoria(nome, cor);
    }

    protected override Func<Categoria, bool>? ValidarVinculos => categoria => repositorioProduto.SelecionarTodos().Any(product => product.IdCategoria == categoria.Id);

    protected override bool ValidarEntidade(Categoria categoria)
    {
        bool valido = true;

        valido &= Validar.CampoObrigatorio(categoria.Nome, "Nome");
        valido &= Validar.Tamanho(categoria.Nome, "Nome", 3, 50);
        valido &= Validar.CorValida(categoria.Cor, "Cor");

        return valido;
    }
}