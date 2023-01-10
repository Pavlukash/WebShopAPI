namespace WebShop.Common.Auth
{
    public class PwdOptions
    {
        public int HashSize { get; set; }
        public int SaltSize { get; set; }
        public int Iterations { get; set; }
    }
}