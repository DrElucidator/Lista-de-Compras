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

    public static bool Duplicado<T>(string valor, string nomeCampo, List<T> registros, string idAtual = null, string assunto = "Validação") where T : EntidadeBase
    {
        if (string.IsNullOrWhiteSpace(valor))
            return true;

        var propriedadeNome = typeof(T).GetProperty("Nome");
        if (propriedadeNome != null)
        {
            bool existe = registros.Any(reg => (idAtual == null || reg.Id != idAtual) &&
            string.Equals(propriedadeNome.GetValue(reg)?.ToString(), valor, StringComparison.OrdinalIgnoreCase));

            if (existe)
            {
                Erro($"Já existe um registro com o campo \"{nomeCampo}\" igual a \"{valor}\".", assunto);
                return false;
            }
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

    public static string LerCampoObrigatorio<T>(string mensagem, string nomeCampo, int min, int max, List<T>? registros = null) where T : EntidadeBase
    {
        string valor;
        do
        {
            Console.Write(mensagem);
            valor = Console.ReadLine() ?? string.Empty;

            if (CampoObrigatorio(valor, nomeCampo) && Tamanho(valor, nomeCampo, min, max))
            {
                if (registros != null && !Duplicado(valor, nomeCampo, registros))
                    continue;
                break;
            }
        } while (true);

        return valor;
    }

    public static string LerCampoOpcional(string mensagem, string valorAtual, string nomeCampo, int min, int max)
    {
        Console.Write(mensagem);
        string valor = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(valor))
            return valorAtual;

        if (CampoObrigatorio(valor, nomeCampo) && Tamanho(valor, nomeCampo, min, max))
            return valor;

        return valorAtual;
    }

    public static string LerId<T>(string mensagem, List<T> lista, string assunto) where T : EntidadeBase
    {
        string id;
        do
        {
            Console.Write(mensagem);
            id = Console.ReadLine()?.ToUpper() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(id))
            {
                Aviso("Operação cancelada.", assunto);
                MensagemContinuar();
                return string.Empty;
            }

            if (IdValido(id, lista, assunto))
                break;
        } while (true);

        return id;
    }

    public static int LerQuantidade(string mensagem, string nomeCampo)
    {
        int quantidade;
        do
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out quantidade) && quantidade > 0)
                break;

            Erro($"O campo \"{nomeCampo}\" deve ser um número positivo.", "Validação");
        } while (true);

        return quantidade;
    }

    public static string LerCor(string mensagem, string nomeCampo)
    {
        string cor;
        do
        {
            Console.Write(mensagem);
            cor = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(cor))
                return cor;

            if (CorValida(cor, nomeCampo))
                break;
        } while (true);

        return cor;
    }

    public static string LerCorOpcional(string mensagem, string corAtual)
    {
        Console.Write(mensagem);
        string cor = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(cor))
            return corAtual;

        if (CorValida(cor, "Cor"))
            return cor;

        return corAtual;
    }

    public static double LerPreco(string mensagem)
    {
        double preco;
        do
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();

            if (PrecoValido(entrada, out preco) && NumeroPositivo(preco, "Preço"))
                break;
        } while (true);

        return preco;
    }

    public static double LerPrecoOpcional(string mensagem, double precoAtual)
    {
        Console.Write(mensagem);
        string? entrada = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(entrada))
            return precoAtual;

        double preco;
        if (PrecoValido(entrada, out preco) && NumeroPositivo(preco, "Preço"))
            return preco;

        return precoAtual;
    }

    public static string LerStatus(string mensagem, string statusAtual)
    {
        string status;
        do
        {
            Console.WriteLine(mensagem);
            Console.Write("> ");
            string? opcao = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(opcao))
                return statusAtual;

            if (opcao == "1")
                return "Aberta";
            else if (opcao == "2")
                return "Concluída";

            Erro("Opção inválida. Digite 1 ou 2.", "Lista");
        } while (true);
    }
}