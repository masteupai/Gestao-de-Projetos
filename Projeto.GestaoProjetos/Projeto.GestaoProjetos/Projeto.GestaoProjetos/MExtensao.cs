using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Biblioteca.Classes
{
    public static class MExtensao
    {
        public static int ContarDigitos(this string txt)
        {
            return txt.Replace(" ", "").Length;
        }
        public static string TrocarCaracteres(this string cpf , char c)
        {
            string valor = cpf.Replace(".", "");
            cpf = valor.Replace("-", "");
            return cpf;            
        }
        public static string ValidarVazio(this string txt)
        {
            if (txt.Trim().Equals(""))
            {
                throw new Exception("O Texto não pode ser vazio");
            }
            return txt;
        }
        public static string ValidarEmail(this string txt)
        {
            //"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
            if (!Regex.IsMatch(txt,
                @"^[a-zA-Z0-9\._\-]+\@+[a-zA-Z0-9\._\-]+\.[a-zA-Z]+$"))
            {
                throw new Exception("Informe um email válido!");
            }
            return txt;
        }
        public static string RetirarAcentos(this string texto)
        {
            string com_Acentos =
                "ÄÀÁÂÃäàáâãËÈÉÊëèéêÏÌÍÎïìíîÖÒÓÔÕöòóôõÜÙÚÛüùúûÇçÑñ";
            string sem_Acentos =
                "AAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUUuuuuCcNn";

            for (int i = 0; i < com_Acentos.Length; i++)
            {
                texto = texto.Replace(com_Acentos[i], sem_Acentos[i]);
            }
            return texto;
        }     
        public static bool ValidaCPF(this string vrCPF)
        {

            string valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                  valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                return false;

            return true;





        }
        public static string GerarTexto(this string txt, params object[] itens)
        {
            try
            {
                return string.Format(txt, itens);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static int ContarLetrasA(this string txt)
        {
            int qtA = 0;
            txt = txt.RetirarAcentos();
            for (int i = 0; i < txt.Length; i++)
            {
                if (txt[i] == 'a' || txt[i] == 'A')
                {
                    qtA = qtA + 1;
                }
            }
            return qtA;
        }
        public static int ContarLetrasX(this string txt, char y)
        {
            int qtA = 0;
            txt = txt.RetirarAcentos();
            for (int i = 0; i < txt.Length; i++)
            {
                if (txt[i] == y || (txt[i] - 32) == y || (txt[i] + 32) == y)
                {
                    qtA = qtA + 1;
                }
            }
            return qtA;
        }
        public static int ContarNomes(this string[] txt, string conj)
        {
            int qtA = 0;
            foreach (var item in txt)
            {
                if (item.Contains(conj))
                {
                    qtA += 1;
                }
            }
            return qtA;
        }
        public static List<string> BuscarPalavras(this string[] palavras, string s)
        {
            var lista = new List<string>();
            foreach (var item in palavras)
            {
                if (item.Contains(s))
                {
                    lista.Add(item);
                }
            }
            return lista;
        }
        public static IEnumerable<string> BuscarPalavras1(this string[] palavras , string s)
        {
            foreach (var item in palavras)
            {
                if (item.Contains(s))
                {
                    yield return item; //retorno "em prestações" só pode usar em IEnumerable
                }
            }
        }
        public static bool ValidaCnpj(this string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
}
