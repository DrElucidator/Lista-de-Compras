using ListaDeCompras.ConsoleApp.Compartilhado;

namespace ListaDeCompras.ConsoleApp.ModuloItem;

public class ItemLista : EntidadeBase
{
    public string IdLista { get; set; }
    public string IdProduto { get; set; }
    public int Quantidade { get; private set; }

    public ItemLista(string idLista, string idProduto, int quantidade)
    {
        IdLista = idLista;
        IdProduto = idProduto;
        Quantidade = quantidade;
    }

    public override void AtualizarDados(EntidadeBase entidadeAtualizada)
    {
        var itemAtualizado = (ItemLista)entidadeAtualizada;

        IdLista = itemAtualizado.IdLista;
        IdProduto = itemAtualizado.IdProduto;
        Quantidade = itemAtualizado.Quantidade;
    }

    public override string ToString() => $"{IdProduto} x{Quantidade}";
}
