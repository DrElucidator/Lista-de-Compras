using ListaDeCompras.ConsoleApp.Compartilhado;

namespace ListaDeCompras.ConsoleApp.ModuloCategoria;

public class Categoria : EntidadeBase
{
    public string Nome { get; private set; }
    public string Cor { get; private set; }

    public Categoria(string nome, string cor)
    {
        Nome = nome;
        Cor = cor;
    }

    public override void AtualizarDados(EntidadeBase entidadeAtualizada)
    {
        var categoriaAtualizada = (Categoria)entidadeAtualizada;

        Nome = categoriaAtualizada.Nome;
        Cor = categoriaAtualizada.Cor;
    }

    public override string ToString() => $"{Nome}";
}