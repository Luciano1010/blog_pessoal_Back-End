using blogpessoal.Model;
using FluentValidation;

namespace blogpessoal.Validator
{
    public class TemaValidator : AbstractValidator<Tema> // isso vem de uma extensão de uma biblioteca
    {
        // metodo que verifica se as regras estao sendo seguidas e sim grava no banco de dados
        public TemaValidator()
        {
            RuleFor(t => t.Descricao)
                .NotEmpty() // ta vazio ? 
                .MinimumLength(5) // tem no minimo 5 caracteretes
                .MaximumLength(100); // tem no maximo 100 caracteres

        }
    }
           
}

