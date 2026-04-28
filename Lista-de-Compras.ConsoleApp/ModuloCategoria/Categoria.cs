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

    public override string[] Validar()
    {
        string erros = string.Empty;

        if (Nome.Length == 0 || Nome.Length > 50)
            erros += "O campo \"Nome\" deve conter entre 0 e 50 caracteres;";

        if (string.IsNullOrWhiteSpace(Cor))
            erros += "O campo \"Cor\" deve ser preenchido;";

        else if (Cor != "Vermelho" && Cor != "Azul" && Cor != "Verde" && Cor != "Branco")
            erros += "O campo \"Cor\" deve conter uma seleção permitida (Vermelho, Azul, Verde, Branco);";

        return erros.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }

    public override void AtualizarDados(EntidadeBase entidadeAtualizada)
    {
        Categoria categoriaAtualizada = (Categoria)entidadeAtualizada;

        Nome = categoriaAtualizada.Nome;
        Cor = categoriaAtualizada.Cor;
    }
}