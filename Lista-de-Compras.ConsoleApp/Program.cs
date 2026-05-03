﻿using ListaDeCompras.ConsoleApp.Compartilhado;
using ListaDeCompras.ConsoleApp.Utilidades;

TelaPrincipal telaPrincipal = new TelaPrincipal();

while (true)
{
    ITela? telaSelecionada = telaPrincipal.ApresentarMenuOpcoesPrincipal();

    if (telaSelecionada == null)
    {
        Console.Clear();
        break;
    }

    while (true)
    {
        string? opcaoSubMenu = telaSelecionada.ObterOpcaoMenu();

        if (opcaoSubMenu == "S")
        {
            Console.Clear();
            break;
        }

        if (string.IsNullOrWhiteSpace(opcaoSubMenu))
            return;

        if (opcaoSubMenu == "1")
            telaSelecionada.Cadastrar();

        else if (opcaoSubMenu == "2")
            telaSelecionada.Editar();

        else if (opcaoSubMenu == "3")
            telaSelecionada.Excluir();

        else if (opcaoSubMenu == "4")
            telaSelecionada.VisualizarTodos(deveExibirCabecalho: true);
    }
}