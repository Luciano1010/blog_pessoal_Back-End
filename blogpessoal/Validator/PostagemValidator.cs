using blogpessoal.Model;
using FluentValidation;
using FluentValidation.Validators;

namespace blogpessoal.Validator
{
    public class PostagemValidator : AbstractValidator<Postagem>
    {
        // metodo que verifica se as regras estao sendo seguidas e sim grava no banco de dados
        public PostagemValidator() 
        {
            RuleFor(p => p.Titulo)
                .NotEmpty() // ta vazio ? 
                .MinimumLength(5) // tem no minimo 5 caracteretes
                .MaximumLength(100); // tem no maximo 100 caracteres

            RuleFor(p => p.Texto)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(1000);
        
        }    
    
    }
}
