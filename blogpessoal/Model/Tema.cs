    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using blogpessoal.Model;


    public class Tema 
    {
        [Key] // key chave primaria  // key e databasegenerated estou definindo como uma coluna do banco de dados vai funcionar
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // identity (1,1) auto incremento
        public long Id { get; set; } // long armazena um grande numero de dados numericos e o id vai indemtificar as postagens get pegar o id e set pra modificar o valor do id

        // Temas
        [Column(TypeName = "Varchar")] // o tipo de dado que vai na coluna postagemé do tipo varchar
        [StringLength(100)] // o limite do titulo da posatgem vai ate 100 caracteres
        public string Descricao { get; set; } = string.Empty; // aqui estamos declarando a propriedade em si e ela inicia com o valor padrao vazio.

        [InverseProperty("Tema")]
        public virtual ICollection<Postagem>? Postagem { get; set; } // Icollection(generica) traz todos objetos de postagem, porque posategm podem ter varios temas

    }
        


