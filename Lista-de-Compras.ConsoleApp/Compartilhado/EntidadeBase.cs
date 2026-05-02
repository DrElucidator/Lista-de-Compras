using System.Security.Cryptography;

namespace ListaDeCompras.ConsoleApp.Compartilhado
{
    public abstract class EntidadeBase
    {
        public string Id { get; private set; } = string.Empty;

        public EntidadeBase()
        {
            Id = Convert
                    .ToHexString(RandomNumberGenerator.GetBytes(3))
                    .ToUpper()
                    .Substring(0, 5);
        }

        public abstract void AtualizarDados(EntidadeBase entidadeAtualizada);
    }
}