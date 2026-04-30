namespace ListaDeCompras.ConsoleApp.Compartilhado.Validacao;

public class ValidacaoErro
{
    public string Assunto { get; }
    public string Mensagem { get; }
    public string GrauSeveridade { get; }

    public ValidacaoErro(string assunto, string mensagem, string grauSeveridade = "Erro")
    {
        Assunto = assunto;
        Mensagem = mensagem;
        GrauSeveridade = grauSeveridade;
    }

    public override string ToString() => $"[{GrauSeveridade}] {Assunto} : {Mensagem}";
}