using FluentValidation;
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
    public class CategoriaController : Controller
    {
        private IList<Categoria> ListaCategoria;

        public CategoriaController()
        {
            ListaCategoria = new List<Categoria>();

            ListaCategoria.Add(new Categoria()
            {
                IdCategoria = 2,
                NomeCategoria = "Salgadinhos"
            });

        }

        [HttpGet]
        public ActionResult Ola()
        {
            return Ok("Ola");
        }

        [HttpGet("consultaId/{IdCategoria}")]
        public ActionResult consultaID(int IdCategoria)
        {
            var resultado = ListaCategoria.Where(p => p.IdCategoria == IdCategoria);

            if (resultado.Count() == 0)
                return NotFound("Categoria não encontrada");
            return Ok(resultado);

        }

        [HttpPost("AdicionarCategoria")]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult AdicionarCategoria(int IdCategoria, string NomeCategoria, [FromBody] Categoria categoria)

        {
            var resultadoCategoria = ListaCategoria.Where(p => p.IdCategoria == categoria.IdCategoria).FirstOrDefault();
            if (resultadoCategoria != null)
            {
                return BadRequest("Categoria não pode ser inserida, pois codigo ja existe");
            }

            var resultadoNomeProduto = ListaCategoria.Where(p => p.NomeCategoria == categoria.NomeCategoria).FirstOrDefault();
            if (resultadoNomeProduto != null)
            {
                return BadRequest("Nome da categoria ja existe, tente outro nome");

            }

            ListaCategoria.Add(categoria);
            return CreatedAtAction(nameof(AdicionarCategoria), categoria);

        }

        [HttpPut("Atualizar/{Codigo}")]
        public ActionResult Atualizar(int IdCategoria, [FromBody] Categoria categoria)
        {
            var resultado = ListaCategoria.Where(P => P.IdCategoria == IdCategoria).FirstOrDefault();
            if (resultado == null)
            {
                return NotFound();
            }
            categoria.IdCategoria = IdCategoria;
            ListaCategoria.Remove(resultado);
            ListaCategoria.Add(categoria);

            return NoContent();

        }

        [HttpDelete("Remover/{IdCategoria}")]
        public ActionResult Remover(int IdCategoria)
        {
            var resultadoCategoria = ListaCategoria.Where(p => p.IdCategoria == IdCategoria).FirstOrDefault();
            if (resultadoCategoria == null)
            {
                return NotFound("Categoria não encontrada");
            }
            ListaCategoria.Remove(resultadoCategoria);
            return NoContent();
        }




    }


}