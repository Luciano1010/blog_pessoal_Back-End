using blogpessoal.Model;
using FluentValidation;

namespace blogpessoal.Validator
{
    public class TemaValidator : AbstractValidator<Tema> 
    {
       
        public TemaValidator()
        {
            RuleFor(t => t.Descricao)
                .NotEmpty()  
                .MinimumLength(5) 
                .MaximumLength(100); 

        }
    }
           
}

