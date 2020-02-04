using Projeto.GestaoProjetos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projeto.GestaoProjetos.Db
{
    //public class ClientesDao
    //{
    //    //método para incluir um novo cliente
    //    public static void IncluirCliente(Cliente cliente)
    //    {
    //        using (var ctx = new ProjetoConnection())
    //        {
    //            ctx.Clientes.Add(cliente);
    //            ctx.SaveChanges();
    //        }
    //    }
    //    public static IEnumerable<Cliente> ListarClientes()
    //    {
    //        using (var ctx = new ProjetoConnection())
    //        {
    //            var clientes = ctx.Clientes.ToList();                
    //            return clientes;
    //        }
    //    }
    //    public static Cliente BuscarCliente(int idCliente)
    //    {
    //        using (var ctx = new ProjetoConnection())
    //        {
    //            var cliente = ctx.Clientes.FirstOrDefault(p => p.IDCLIENTE.Equals(idCliente)); 
    //            return cliente;             
    //        }
    //    }
    //    public static void AlterarCliente(Cliente cliente)
    //    {
    //        using (var ctx = new ProjetoConnection())
    //        {
    //            ctx.Entry<Cliente>(cliente).State = EntityState.Modified;
    //            ctx.SaveChanges();
    //        }
    //    }
    //    public static void ExcluirCliente(Cliente cliente)
    //    {
    //        using (var ctx = new ProjetoConnection())
    //        {
    //            ctx.Entry<Cliente>(cliente).State = EntityState.Deleted;
    //            ctx.SaveChanges();
    //        }
    //    }
    //}
}