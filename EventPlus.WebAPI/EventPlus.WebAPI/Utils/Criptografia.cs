namespace EventPlus.WebAPI.Utils
{
    /// <summary>
    /// Ultilitário estático responsável pelas operações de criptografia e hashing de senhas na API
    /// </summary>
    public static class Criptografia
    {

        public static string GerarHash (string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public static bool CompararHash(string senhaInformada, string senhaBanco)
        {
            if (string.IsNullOrEmpty(senhaInformada) || string.IsNullOrEmpty(senhaBanco))
            {
                return false;
            }

            try
            {
               return BCrypt.Net.BCrypt.Verify(senhaInformada, senhaBanco);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }

        }
    }
}
