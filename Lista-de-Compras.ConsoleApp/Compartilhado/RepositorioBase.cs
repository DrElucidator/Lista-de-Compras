using ListaDeCompras.ConsoleApp.Compartilhado.Validacao;

namespace ListaDeCompras.ConsoleApp.Compartilhado;

public abstract class RepositorioBase<T> where T : EntidadeBase
{
    protected List<T> registros = new List<T>();

    public void Cadastrar(T entidade)
    {
        registros.Add(entidade);
        Validar.Sucesso($"\"{entidade}\" cadastrado com sucesso.", "Registro");
        Validar.MensagemContinuar();
    }

    public virtual bool Editar(string idSelecionado, T entidadeAtualizada)
    {
        T? registroSelecionado = SelecionarPorId(idSelecionado);

        if (registroSelecionado == null)
        {
            Validar.Erro($"\"{entidadeAtualizada}\" não encontrado para edição.", "Registro");
            Validar.MensagemContinuar();
            return false;
        }

        registroSelecionado.AtualizarDados(entidadeAtualizada);
        Validar.Sucesso($"\"{entidadeAtualizada}\" editado com sucesso.", "Registro");
        Validar.MensagemContinuar();
        return true;
    }

    public virtual bool Excluir(string idSelecionado, Func<T, bool>? possuiVinculos = null)
    {
        var entidade = SelecionarPorId(idSelecionado);
        if (entidade == null)
        {
            Validar.Erro($"\"{entidade}\" não encontrado para exclusão.", "Registro");
            Validar.MensagemContinuar();
            return false;
        }

        if (possuiVinculos != null && possuiVinculos(entidade))
        {
            Validar.Erro($"\"{entidade}\" não pode ser excluído pois possui vínculos.", "Registro");
            Validar.MensagemContinuar();
            return false;
        }

        registros.Remove(entidade);
        Validar.Sucesso($"\"{entidade}\" excluído com sucesso.", "Registro");
        Validar.MensagemContinuar();
        return true;
    }

    protected ValidacaoErro? ValidarDuplicado(string valor, string nomeCampo)
    {
        var propriedadeNome = typeof(T).GetProperty("Nome");
        if (propriedadeNome != null)
        {
            bool existe = registros.Any(reg =>
                propriedadeNome.GetValue(reg)?.ToString()!
                .Equals(valor, StringComparison.OrdinalIgnoreCase) == true);

            if (existe)
                return new ValidacaoErro(nomeCampo, $"Já existe um registro com o campo \"{nomeCampo}\" igual a \"{valor}\"", "Nome");
        }
        return null;
    }

    public virtual T? SelecionarPorId(string idSelecionado) =>
        registros.FirstOrDefault(reg => reg.Id == idSelecionado);

    public List<T> SelecionarTodos() => registros;
}