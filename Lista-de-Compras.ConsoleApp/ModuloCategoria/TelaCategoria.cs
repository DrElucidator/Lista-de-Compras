using System.Collections;
using ListaDeCompras.ConsoleApp.Compartilhado;
using ListaDeCompras.ConsoleApp.Compartilhado.Validacao;
using ListaDeCompras.ConsoleApp.ModuloProduto;
using ListaDeCompras.ConsoleApp.Utilidades;

namespace ListaDeCompras.ConsoleApp.ModuloCategoria;

public class TelaCategoria : TelaBase<Categoria>
{
    private readonly RepositorioBase<Produto> repositorioProduto;
    public TelaCategoria(RepositorioBase<Categoria> repositorio, RepositorioBase<Produto> repositorioProduto) : base("Categoria", repositorio) { this.repositorioProduto = repositorioProduto; }
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
        string nome;
        do
        {
            Write("Digite o nome da categoria: ");
            nome = Console.ReadLine() ?? string.Empty;

            if (Validar.CampoObrigatorio(nome, "Nome") && Validar.Tamanho(nome, "Nome", 3, 50))
                break;

        } while (true);

        string cor;
        do
        {
            Write("Digite a cor da categoria: ");
            cor = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(cor))
                break;

            if (Validar.CorValida(cor, "Cor"))
                break;

        } while (true);

        return new Categoria(nome, cor);
    }

    protected override Categoria ObterDadosEdicao(Categoria atual)
    {
        string nome;
        do
        {
            Write("Digite o nome da categoria: ");
            nome = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nome))
            {
                nome = atual.Nome;
                break;
            }

            if (Validar.CampoObrigatorio(nome, "Nome") && Validar.Tamanho(nome, "Nome", 3, 50))
                break;
        } while (true);

        string cor;
        do
        {
            Write("Digite a cor da categoria: ");
            cor = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(cor))
            {
                cor = atual.Cor;
                break;
            }

            if (Validar.CorValida(cor, "Cor"))
                break;
        } while (true);

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