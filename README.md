## Visão Geral
Este projeto é uma aplicação de console em C# para **gestão de listas de compras**.  
Ele foi desenvolvido com foco em **modularidade** e **reuso**, utilizando um modelo genérico que permite implementar novas funcionalidades e módulos de forma simples e consistente.

---

## Funcionalidades
- **Gerenciar Categorias**  
  Criar, editar, visualizar e excluir categorias de produtos, com suporte a cores para destaque visual.
  
- **Gerenciar Produtos**  
  Associar produtos a categorias, definir unidade de medida e preço, e manter controle centralizado.

- **Gerenciar Listas de Compras**  
  Criar listas, definir status (Aberta/Concluída), visualizar totais de itens e valores estimados.

- **Gerenciar Itens da Lista**  
  Adicionar produtos às listas, definir quantidades, recalcular automaticamente totais e valores.

---

## Arquitetura Genérica
O projeto segue um padrão **CRUD genérico** (Create, Read, Update, Delete), implementado através de classes base reutilizáveis:

### 1. **EntidadeBase**
- Define propriedades comuns (`Id`) e método `AtualizarDados`.
- Todas as entidades (`Categoria`, `Produto`, `ListaDeCompras`, `ItemLista`) herdam dela.

### 2. **RepositorioBase<T>**
- Implementa operações genéricas de persistência em memória:
  - `Cadastrar`, `Editar`, `Excluir`, `SelecionarTodos`, `SelecionarPorId`.
- Cada módulo possui seu repositório específico, mas todos usam a mesma lógica base.

### 3. **TelaBase<T>**
- Estrutura genérica para telas de console:
  - Exibir cabeçalhos.
  - Fluxo CRUD padronizado.
- Telas específicas (`TelaCategoria`, `TelaProduto`, `TelaLista`, `TelaItemLista`) herdam e personalizam apenas o necessário.

### 4. **Validar.cs**
- Centraliza toda a lógica de validação e input:
  - `LerCampoObrigatorio`, `LerCampoOpcional`, `LerId`, `LerQuantidade`, `LerCor`, `LerPreco`, `LerStatus`.
  - Feedback ao usuário: `Erro`, `Aviso`, `Sucesso`, `Confirmação`, `MensagemContinuar`.

### 5. **CustomText**
- Utilitário para exibição formatada no console:
  - `TextoColorido` para destacar categorias, status e mensagens.

---

## Benefícios do Modelo Genérico
- **Facilidade de expansão**: adicionar uma nova entidade (ex.: Fornecedor, Cliente) exige apenas criar a classe e herdar de `EntidadeBase`, sem reescrever lógica CRUD.  
- **Consistência**: todas as telas seguem o mesmo padrão de interação.  
- **Reuso**: validações e exibição são centralizadas, evitando duplicação de código.  
- **Manutenção simplificada**: alterações em regras de validação ou persistência refletem em todos os módulos automaticamente.

---

## Fluxo de Uso
1. Usuário acessa o menu principal (`TelaPrincipal`).  
2. Seleciona o módulo desejado (Categoria, Produto, Lista, Item).  
3. A tela correspondente captura dados → chama `Validar` → monta entidade.  
4. Entidade é persistida via `RepositorioBase<T>`.  
5. Resultados são exibidos com `CustomText`.

---

## Comentários finais
Graças ao modelo genérico, torna-se simples implementar novos módulos e funcionalidades, assim o programa segue um padrão facilmente reconhecível e adaptável que é fundamento para os demais segmentos, centralizando a estruturura e simplificando a lógica
