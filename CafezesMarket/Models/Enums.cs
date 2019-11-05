namespace CafezesMarket.Models
{
    public static class Enums
    {
        public enum CadastroCliente : int
        {
            Sucesso = 0,
            CpfInvalido = 1,
            DataNascimentoInvalida = 2,
            EmailJaExiste = 3,
            CpfJaExiste = 4,
            SenhaInvalida = 5,
            ErroSistema = 6
        }
    }
}
