using CRUD.domain.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.web.Controllers
{
	public class PeopleController : Controller
	{
		private readonly HttpClient _httpClient;

		public PeopleController(HttpClient httpClient)
        {
			_httpClient = httpClient;
		}
        public async Task<IActionResult> Index()
		{
			var people = await _httpClient.GetFromJsonAsync<List<GetPeople>>("http://localhost:5263/api/people/GetPeople");
			if (people == null)
			{
				ViewBag.ErrorMessage = "No se pudieron obtener las personas";
				return View();
			}
				
			return View(people);
		}

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostPeople newPerson)
        {
            if (!ModelState.IsValid)
            {
                return View(newPerson);
            }

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5263/api/People/PostPeople", newPerson);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Hubo un error al crear la persona.";
                return View(newPerson);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var response = await _httpClient.GetAsync($"http://localhost:5263/api/People/GetPeopleById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var person = await response.Content.ReadFromJsonAsync<PutPeople>();

                if (person == null)
                {
                    ViewBag.ErrorMessage = "No se encontraron datos de la persona.";
                    return RedirectToAction("Index");
                }

                return View(person);
            }
            else
            {
                ViewBag.ErrorMessage = "Hubo un error al obtener la información.";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(PutPeople newPerson)
        {
            if (!ModelState.IsValid)
            {
                return View(newPerson);
            }

            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5263/api/People/PutPeople/{newPerson.id}", newPerson);

            if (response.IsSuccessStatusCode) 
            {
                return RedirectToAction("index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = $"Hubo un error al actualizar la persona: {errorContent}";
                return View(newPerson);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "El ID proporcionado no es válido.";
                return RedirectToAction("Index");
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"http://localhost:5263/api/People/DeletePeople/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "La persona se eliminó correctamente.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Hubo un error al eliminar la persona: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error inesperado: {ex.Message}";
            }

            return RedirectToAction("Index");
        }


    }
}
