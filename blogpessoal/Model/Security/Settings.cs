namespace blogpessoal.Model.Security
{ 
    public class Settings
    {
        private static string secret = "d05a1393c81fc71880feb1dd13500cd1776eefd84c4199856fed0bfca2854c4f";

        public static string  Secret { get => secret; set => secret = value; }

    } 
  
}
