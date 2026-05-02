using System.Globalization;
namespace ListaDeCompras.ConsoleApp.Compartilhado.Validacao;

public static class Validar
{
    public static void MensagemContinuar()
    {
        Console.WriteLine("---------------------------------");
        Console.Write("Pressione ENTER para continuar...");
        Console.ReadLine();
    }

    public static void Erro(string mensagem, string assunto = "Sistema")
    {
        var registro = new ValidacaoErro(assunto, mensagem, "Erro");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(registro.ToString());
        Console.ResetColor();
    }

    public static void Aviso(string mensagem, string assunto = "Sistema")
    {
        var registro = new ValidacaoErro(assunto, mensagem, "Aviso");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(registro.ToString());
        Console.ResetColor();
    }

    public static void Sucesso(string mensagem, string assunto = "Sistema")
    {
        var registro = new ValidacaoErro(assunto, mensagem, "Sucesso");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(registro.ToString());
        Console.ResetColor();
    }

    public static void Confirmacao(string mensagem, string assunto = "Sistema")
    {
        var registro = new ValidacaoErro(assunto, mensagem, "Confirmação");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(registro.ToString());
        Console.ResetColor();
    }

    public static bool CampoObrigatorio(string? valor, string nomeCampo, string assunto = "Validação")
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            Erro($"O campo \"{nomeCampo}\" é obrigatório.", assunto);
            return false;
        }
        return true;
    }

    public static bool Tamanho(string valor, string nomeCampo, int min, int max, string assunto = "Validação")
    {
        if (valor.Length < min || valor.Length > max)
        {
            Erro($"O campo \"{nomeCampo}\" deve conter entre {min} e {max} caracteres.", assunto);
            return false;
        }
        return true;
    }

    public static bool NumeroPositivo(double valor, string nomeCampo, string assunto = "Validação")
    {
        if (valor <= 0)
        {
            Erro($"O campo \"{nomeCampo}\" deve ser maior que zero.", assunto);
            return false;
        }
        return true;
    }

    public static bool IdValido<T>(string? id, List<T> lista, string assunto = "Validação") where T : EntidadeBase
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            Aviso("Operação cancelada.", assunto);
            return false;
        }

        if (!lista.Any(entidade => entidade.Id == id))
        {
            Erro("ID inválido. Digite um ID válido ou pressione ENTER para cancelar.", assunto);
            return false;
        }

        return true;
    }

    public static bool CorValida(string? cor, string assunto = "Validação")
    {
        if (string.IsNullOrWhiteSpace(cor))
            return true;

        if (!Enum.TryParse<ConsoleColor>(cor, true, out _))
        {
            Erro($"A cor \"{cor}\" não é válida. Digite uma cor válida.", assunto);
            return false;
        }

        return true;
    }

    public static bool PrecoValido(string? entrada, out double preco, string assunto = "Preço")
    {
        preco = 0;

        if (string.IsNullOrWhiteSpace(entrada))
        {
            Erro("Preço não pode ser vazio.", assunto);
            return false;
        }

        // Converte entrada para moeda enquanto aceitando vírgula:
        if (!double.TryParse(entrada, NumberStyles.Number, CultureInfo.GetCultureInfo("pt-BR"), out preco))
        {
            Erro("Preço inválido. Digite um valor numérico double válido", assunto);
            return false;
        }

        if (preco <= 0)
        {
            Erro("Preço deve ser maior que zero.", assunto);
            return false;
        }

        return true;
    }
}