namespace ListaDeCompras.ConsoleApp.Utilidades;

public class TextoColorido
{
    public string Texto { get; set; }
    public string? Cor { get; set; }

    public TextoColorido(string texto, string? cor = null)
    {
        Texto = texto;
        Cor = cor;
    }
}

public static class CustomText
{
    public static void ExibirTextoColorido(List<TextoColorido> partes)
    {
        foreach (var parte in partes)
        {
            if (!string.IsNullOrWhiteSpace(parte.Cor) &&
                Enum.TryParse(parte.Cor, true, out ConsoleColor corConsole))
            {
                Console.ForegroundColor = corConsole;
                Console.Write(parte.Texto);
                Console.ResetColor();
            }
            else
            {
                Console.Write(parte.Texto);
            }
        }

        Console.WriteLine();
    }
    
    public static void TextoColorido(string texto, string? cor = null)
    {
        var partes = new List<TextoColorido> { new TextoColorido(texto, cor) };
        ExibirTextoColorido(partes);
    }

    public static void TextoColorido(params (string texto, string? cor)[] partes)
    {
        var lista = partes
            .Select(part => new TextoColorido(part.texto, part.cor))
            .ToList();

        ExibirTextoColorido(lista);
    }
}