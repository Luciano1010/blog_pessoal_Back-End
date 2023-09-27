using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogpessoal.Model
{
    public class Postagem : Auditable
    {
        // modelo de dados da postagem 

        [Key] // key chave primaria  // key e databasegenerated estou definindo como uma coluna do banco de dados vai funcionar
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // identity (1,1) auto incremento
        public long Id { get; set; } // long armazena um grande numero de dados numericos e o id vai indemtificar as postagens get pegar o id e set pra modificar o valor do id

        // titulo
        [Column(TypeName = "Varchar")] // o tipo de dado que vai na coluna postagemé do tipo varchar
        [StringLength(100)] // o limite do titulo da posatgem vai ate 100 caracteres
        public string Titulo { get; set; } = string.Empty; // aqui estamos declarando a propriedade em si e ela inicia com o valor padrao vazio.

        // texto
        [Column(TypeName = "Varchar")] 
        [StringLength(1000)] // limite maximo de digitação é 1000
        public string Texto { get; set; } = string.Empty;

        public virtual Tema? Tema { get; set; } // chave estrangeira vem da tabela Tema, virtula porque pode vim vazio


    }
}


