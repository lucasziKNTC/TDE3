using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDEProdutos.models;
using TDEProdutos.Validations;

namespace TDEProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProdutosController : Controller
    {
        private IList<Produto> ListaProdutos;

        public ProdutosController()
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

        [HttpGet]
        public ActionResult ola()
        {
            return Ok("Ola");
        }

        [HttpGet("BuscarPorCodigo/{codigo}")]

        public ActionResult BuscarPorCodigo(string codigo)
        {
            var resultado = ListaProdutos.Where(P => P.Codigo == codigo).FirstOrDefault();
            if (resultado == null)
            {
                return NotFound("O produto não existe na base de dados");
            }
           
            return Ok(resultado);
        }


        [HttpPost("Adicionar")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public ActionResult Adicionar(Produto Produto)
        {
            ProdutoValidation produtoValidation = new ProdutoValidation();
            var validacao = produtoValidation.Validate(Produto);
            if (!validacao.IsValid)
            {
                List<string> erros = new List<string>();
                foreach(var failure in validacao.Errors)
                {
                    erros.Add("Property" + failure.PropertyName +
                        "failed validation. Error Was: "
                        + failure.ErrorMessage);
                }
            }


            ListaProdutos.Add(Produto);
            return CreatedAtAction(nameof(Adicionar), Produto);
        }



        [HttpPut("Atualizar/{Codigo}")]
        public ActionResult Atualizar (string Codigo, [FromBody] Produto Produto)
        {
            var resultado = ListaProdutos.Where(P => P.Codigo == Codigo).FirstOrDefault();
            if (resultado == null)
            {
                return NotFound();
            }
            Produto.Codigo = Codigo;
            ListaProdutos.Remove(resultado);
            ListaProdutos.Add(Produto);

            return NoContent();

        }

        [HttpDelete("Remover/{Codigo}")]
        public ActionResult Remover(string Codigo)
        {
            var resultado = ListaProdutos.Where(P => P.Codigo ==Codigo).FirstOrDefault();
            if (resultado == null)
            {
                return NotFound();
            }
            ListaProdutos.Remove(resultado);
            return NoContent();
        }

        [HttpPut("desativar{Codigo}")]

        public ActionResult Desativar(string codigo)
        {
            var produtoDesativado = ListaProdutos.Where(P => P.Codigo == codigo).FirstOrDefault();
            if (produtoDesativado == null) return NotFound("Produto não pode ser desativado, pois codigo não existe");
            if (produtoDesativado != null && produtoDesativado.Ativo == false) return BadRequest("Produto já está desativado, operação não realizada");
            produtoDesativado.Ativo = false;

            using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("ls4206498@gmail.com", "SUASENHAdoEmail");
            }

            using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
            {
                mail.From = new System.Net.Mail.MailAddress("ls4206498@gmail.com");
                mail.To.Add(new System.Net.Mail.MailAddress("ls4206498@gmail.com"));


                mail.Subject = "Produto desativado";
                mail.Body = "Olá, atenção. O produto " + codigo + " foi desativado do seu catalogo!";
            }

            return Ok("Prduto desativo");

        }



    }


    }

