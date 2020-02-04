using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto.GestaoProjetos.Models
{
    public class UsuarioView
    {

        [Required]
        public string Nome { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required]
        [Compare("Senha")]
        [DataType(DataType.Password)]
        public string Confirma { get; set; }

        public string Nivel { get; set; }
    }
}