using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ListaDeCompras.ConsoleApp.ModuloCategoria;
using ListaDeCompras.ConsoleApp.Compartilhado;
using ListaDeCompras.ConsoleApp.Compartilhado.Validacao;
using System.Data.Common;
using ListaDeCompras.ConsoleApp.Utilidades;

namespace ListaDeCompras.ConsoleApp.ModuloProduto;

public class TelaProduto : TelaBase<Produto>
{
    private readonly RepositorioBase<Categoria> repositorioCategoria;
    public TelaProduto(RepositorioBase<Produto> repositorioProduto, RepositorioBase<Categoria> repositorioCategoria) : base("Produto", repositorioProduto) { this.repositorioCategoria = repositorioCategoria; }
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

        string idCategoria;
        do
        {
            Console.Write("Digite o ID da categoria do produto: ");
            idCategoria = Console.ReadLine()?.ToUpper() ?? string.Empty;

            if (Validar.IdValido(idCategoria, repositorioCategoria.SelecionarTodos(), "Categoria"))
                break;
        } while (true);

        string nome;
        do
        {
            Write("Digite o nome do produto: ");
            nome = Console.ReadLine() ?? string.Empty;

            if (Validar.CampoObrigatorio(nome, "Nome") && Validar.Tamanho(nome, "Nome", 3, 50))
                break;

        } while (true);

        string medida;
        do
        {
            Write("Digite a unidade de medida do produto: ");
            medida = Console.ReadLine()?.ToUpper() ?? string.Empty;

            if (Validar.CampoObrigatorio(medida, "Medida") && Validar.Tamanho(medida, "Medida", 2, 50))
                break;

        } while (true);

        double preco;
        do
        {
            Write("Digite o preço do produto: ");
            string? precoInformado = Console.ReadLine();

            if (Validar.PrecoValido(precoInformado, out preco) && Validar.NumeroPositivo(preco, "Preço", "Preço"))
                break;
        } while (true);

        return new Produto(nome, medida, preco, idCategoria);
    }

    protected override Produto ObterDadosEdicao(Produto atual)
    {
        string nome;
        do
        {
            Write("Digite o nome do produto: ");
            nome = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nome))
            {
                nome = atual.Nome;
                break;
            }

            if (Validar.CampoObrigatorio(nome, "Nome") && Validar.Tamanho(nome, "Nome", 2, 50))
                break;
        } while (true);

        string medida;
        do
        {
            Write("Digite a unidade de medida do produto: ");
            medida = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(medida))
            {
                medida = atual.Medida;
                break;
            }

            if (Validar.CampoObrigatorio(medida, "Medida") && Validar.Tamanho(medida, "Medida", 2, 50))
                break;
        } while (true);

        double preco;
        do
        {
            Write("Digite o preço do produto: ");
            string? precoInformado = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(precoInformado))
            {
                preco = atual.Preco;
                break;
            }

            if (Validar.PrecoValido(precoInformado, out preco) && Validar.NumeroPositivo(preco, "Preço", "Preço"))
                break;
        } while (true);

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