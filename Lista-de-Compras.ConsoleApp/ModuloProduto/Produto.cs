using ListaDeCompras.ConsoleApp.Compartilhado;

namespace ListaDeCompras.ConsoleApp.ModuloCategoria;

public class Produto : EntidadeBase
{
    public string Nome { get; private set; }
    public string Medida { get; private set; }
    public double Preco { get; private set; }
    public string IdCategoria { get; private set; }

    public Produto(string nome, string medida, double preco, string idCategoria)
    {
        Nome = nome;
        Medida = medida;
        Preco = preco;
        IdCategoria = idCategoria;
    }

    public override void AtualizarDados(EntidadeBase entidadeAtualizada)
    {
        var produtoAtualizado = (Produto)entidadeAtualizada;

        Nome = produtoAtualizado.Nome;
        Medida = produtoAtualizado.Medida;
        Preco = produtoAtualizado.Preco;
        IdCategoria = produtoAtualizado.IdCategoria;
    }

    public override string ToString() => $"{Nome}";
}