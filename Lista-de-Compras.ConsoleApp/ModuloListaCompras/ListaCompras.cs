using ListaDeCompras.ConsoleApp.Compartilhado;

namespace ListaDeCompras.ConsoleApp.ModuloListaCompras;

public class ListaCompras : EntidadeBase
{
    public string Nome { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public string Status { get; set; }
    public int TotalItens { get; set; }
    public double TotalGastoEstimado { get; set; }

    public ListaCompras(string nome)
    {
        Nome = nome;
        DataCriacao = DateTime.Now;
        Status = "Aberta";
        TotalItens = 0;
        TotalGastoEstimado = 0;
    }

    public override void AtualizarDados(EntidadeBase entidadeAtualizada)
    {
        var listaAtualizada = (ListaCompras)entidadeAtualizada;

        Nome = listaAtualizada.Nome;
        Status = listaAtualizada.Status;
        TotalItens = listaAtualizada.TotalItens;
        TotalGastoEstimado = listaAtualizada.TotalGastoEstimado;
    }

    public override string ToString() => $"{Nome}";
}