using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDEProdutos.models;
using TDEProdutos.Models;

namespace TDEProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private IList<Produto> ListaProdutos;

        public EstoqueController()
        {
            ListaProdutos = new List<Produto>();

            ListaProdutos.Add(new Produto()
            {
                NomeProduto = "SalgadinhoCheatos",
                Codigo = "0102",
                DescricaoProduto = "SalgadinhoCheatos",
                PrecoVendas = 5,
                PrecoCusto = 3,
                DataCadastro = new DateTime(2021, 08, 25),
                Estoque = 100,
                Imagem = "imagem1.png",
                Alturacm = 021,
                Larguracm = 014,
                Profundidadecm = 002,
                categoriaProduto = "Salgadinho",
                AtivoInativo = true

            });
        }
        [HttpPost("DebitarEstoque")]
        public ActionResult DebitarEstoque(DebitoProduto DebitoProduto)
        {
            var resultado = ListaProdutos.Where(P => P.Codigo == DebitoProduto.Codigo).FirstOrDefault();
            if (resultado == null)
            {
                return NotFound("O produto não existe na base de dados");
            }

            if (DebitoProduto.Quantidade > resultado.EstoqueAtual)
            {
                return BadRequest("O produto não tem estoque suficiente");
            }

            resultado.EstoqueAtual = resultado.EstoqueAtual - DebitoProduto.Quantidade;

            if (resultado.EstoqueAtual < resultado.EstoqueMinimo)
            {
                using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("ls4206498@gamail.com", "SUASENHAdoEmail");
                }

                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new System.Net.Mail.MailAddress("ls4206498@gamail.com");
                    mail.To.Add(new System.Net.Mail.MailAddress("ls4206498@gamail.com"));


                    mail.Subject = "estoque minimo";
                    mail.Body = "Olá, atenção. O estoque " + resultado.EstoqueAtual + " ta baixo!";
                }
            }

            return Ok("Produto  do estoque debitado com sucesso!");

        }
    }
}
