namespace ListaDeCompras.ConsoleApp.Compartilhado;

public interface ITela
{
    string? ObterOpcaoMenu();
    void Cadastrar();
    void Editar();
    void Excluir();
    void VisualizarTodos(bool deveExibirCabecalho);
}