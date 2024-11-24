using CRUD.domain.Data;
using CRUD.domain.DTOS;
using CRUD.domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace CRUD.api.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class PeopleController : ControllerBase
	{
		private readonly CRUDdbcontext _context;

		public PeopleController(CRUDdbcontext context)
		{
			_context = context;
		}
		[HttpGet(Name = "GetPeople")]
		public IActionResult GetPeople()
		{
			try
			{
				if (!_context.Database.CanConnect())
				{
					return StatusCode(500, "No se puede conectar a la base de datos.");
				}
				var peopleList = _context.People.Select(p => new GetPeople
				{
					id = p.id,
					name = p.Name,
					age = p.Age,
				}).ToList();

				if (!peopleList.Any())
				{
					return NotFound("No se encontraron personas.");
				}

				return Ok(peopleList);
			}
			catch (Exception ex)
			{

				return StatusCode(500, $"Ocurrió un error inesperado al obtener los datos. {ex.Message}");
			}
		}

		[HttpGet("{id}", Name = "GetPeopleById")]
		public async Task<IActionResult> GetPeopleById(int id)
		{
			try
			{
				if (id <= 0)
				{
					return BadRequest("El id proporcionado no es válido.");
				}
				var search = await _context.People.FindAsync(id); 
				if (search == null)
				{
					return NotFound("Persona no encontrada");
				}

				return Ok(search);
			}
			catch (Exception ex)
			{

				return StatusCode(500, $"Ocurrió un error inesperado al obtener los datos. {ex.Message}");
			}
		}

		[HttpPost(Name = "PostPeople")]
		public async Task<IActionResult> PostPeople(PostPeople post)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var people = new People
				{
					Name = post.Name,
					Age = post.Age,
				};

				_context.Add(people);
				await _context.SaveChangesAsync();

				return Ok(new { Message = "Persona creada con exito" });
			}
			catch (Exception ex)
			{

				return StatusCode(500, $"Ocurrió un error inesperado al obtener los datos. {ex.Message}");
			}
		}

		[HttpPut("{id}", Name = "PutPeople")]
		public async Task<IActionResult> PutPeople(PutPeople put, int id)
		{
		

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id <= 0)
			{
				return BadRequest("El id proporcionado no es válido.");
			}

			try
			{

				var search = await _context.People.FindAsync(id);


				if (search == null)
				{
					return NotFound("Persona no encontrada");
				}
				search.Name = put.Name;
				search.Age = put.Age;

				_context.Update(search);
				await _context.SaveChangesAsync();

				return Ok(search);
			}
			catch (Exception ex)
			{

				return StatusCode(500, $"Ocurrió un error inesperado en. {ex.Message}");
			}
		}

		[HttpDelete("{id}", Name ="DeletePeople")]
		public async Task<IActionResult> DeletePeople(int id)
		{
			if (id<= 0)
			{
				return BadRequest("El id proporcionado no es válido.");
			}

			try
			{
				var delete =  _context.People.FirstOrDefault(d => d.id == id);
				_context.Remove(delete);
				 await _context.SaveChangesAsync();

				return Ok(new { Message = " Eliminado exitosamente.", Data = delete });
			}
			catch (Exception ex)
			{

				return StatusCode(500, $"Ocurrió un error inesperado en. {ex.Message}");
			}
		}

	}
}
