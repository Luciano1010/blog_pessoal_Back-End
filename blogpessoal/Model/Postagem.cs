using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogpessoal.Model
{
    public class Postagem : Auditable
    {
        // modelo de dados da postagem 

        [Key] // chave primaria 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // identity (1,1) auto incremento
        public long id { get; set; }

        // titulo
        [Column(TypeName = "Varchar")]
        [StringLength(100)] // quantidade da digitação
        public string Titulo { get; set; } = string.Empty;

        // texto
        [Column(TypeName = "Varchar")] 
        [StringLength(1000)] // quantidade de digitação
        public string Texto { get; set; } = string.Empty;




    }
}


