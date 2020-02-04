using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto.GestaoProjetos.ViewModel
{
    public class ProjetoValoresViewModel
    {
        public string RazaoSocial { get; set; }
        [DataType(DataType.Time)]
        [Display(Name ="Horas trab.:")]
        public int HorasTrabalhadas { get; set; }
        [DataType(DataType.Time)]
        [Display(Name ="Horas rest.:")]
        public int HorasRestantes { get; set; }
        [DataType(DataType.Currency)]
        public double ValorTotal { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Pago pela Mttechne:")]
        public double ValorPagoColaboradores { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Recebido do Projeto:")]
        public double ValorPedidoColaboradores { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Valor Colaborador:")]
        public double ValorColaborador { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Valor Despesas:")]
        public double ValorDespesas { get; set; } 
        [DataType(DataType.Currency)]
        [Display(Name = "Valor Orçamento:")]
        public double ValorOrca { get; set; }



        [Display(Name ="Total de Horas:")]
        public int NumHorasProj { get; set; }
    }
}