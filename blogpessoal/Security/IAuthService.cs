namespace blogpessoal.Model.Security
{
    public interface IAuthService
    {
        Task <UserLogin?> Autenticar(UserLogin userLogin);
    }
}
