using System.Collections;
using ListaDeCompras.ConsoleApp.Compartilhado;

namespace ListaDeCompras.ConsoleApp.ModuloCategoria;

public class TelaCategoria : TelaBase
{
    public TelaCategoria(RepositorioBase repositorio) : base("Categoria", repositorio)
    {
    }

    public override void VisualizarTodos(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Visualização de Categorias");


        ArrayList categorias = repositorio.SelecionarTodos();

        if (categorias.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Não existe nenhum registro.");
            Console.ResetColor();
            Console.WriteLine("---------------------------------");
            Console.Write("Digite ENTER para continuar...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine(
            "{0, -7} | {1, -20} | {2, -10}",
            "Id", "Nome", "Cor"
        );

        foreach (Categoria c in categorias)
        {
            string corSelecionada = c.Cor;

            if (corSelecionada == "Vermelho")
                Console.ForegroundColor = ConsoleColor.Red;

            else if (corSelecionada == "Verde")
                Console.ForegroundColor = ConsoleColor.Green;

            else if (corSelecionada == "Azul")
                Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine(
                "{0, -7} | {1, -20} | {2, -10}",
                c.Id, c.Nome, c.Cor
            );
        }

        Console.ResetColor();

        if (deveExibirCabecalho)
        {
            Console.WriteLine("---------------------------------");
            Console.Write("Digite ENTER para continuar...");
            Console.ReadLine();
        }
    }

    protected override EntidadeBase ObterDadosCadastrais()
    {
        Console.Write("Digite o nome da categoria: ");
        string nome = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("---------------------------------");
        Console.WriteLine("Selecione uma cor válida para a categoria");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1 - Vermelho");
        Console.WriteLine("2 - Azul");
        Console.WriteLine("3 - Verde");
        Console.WriteLine("4 - Branco (Padrão)");
        Console.WriteLine("---------------------------------");
        Console.Write("Digite a cor da categoria: ");
        string cor = Console.ReadLine() ?? string.Empty;

        string corPorExtenso = string.Empty;

        if (cor == "1")
            corPorExtenso = "Vermelho";
        else if (cor == "2")
            corPorExtenso = "Azul";
        else if (cor == "3")
            corPorExtenso = "Verde";
        else
            corPorExtenso = "Branco";

        return new Categoria(nome, corPorExtenso);
    }
}