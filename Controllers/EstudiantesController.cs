using GestionLibrosApi.Models;
using GestionLibrosApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionLibrosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstudiantesController : ControllerBase
{
    private readonly EstudianteService estudianteService;

    // El controlador recibe el servicio por inyección de dependencias,
    // así no accede directamente a la base de datos.
    public EstudiantesController(EstudianteService estudianteService)
    {
        this.estudianteService = estudianteService;
    }

    // GET: api/Estudiantes
    // Devuelve la lista completa de estudiantes.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resultado = await estudianteService.GetList(e => true);

        if (resultado.IsSuccess)
        {
            return Ok(resultado.Value); // 200 OK, con la lista en el cuerpo de la respuesta
        }
        else
        {
            return BadRequest(resultado.Error); // 400, algo salió mal
        }
    }

    // GET: api/Estudiantes/5
    // Devuelve un solo estudiante por su Id.
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var resultado = await estudianteService.Buscar(id);

        if (!resultado.IsSuccess)
        {
            return BadRequest(resultado.Error);
        }

        if (resultado.Value is null)
        {
            return NotFound(); // 404, no existe un estudiante con ese Id
        }

        return Ok(resultado.Value); // 200 OK
    }

    // POST: api/Estudiantes
    // Crea un estudiante nuevo. Los datos vienen en el cuerpo (body) de la petición.
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Estudiante estudiante)
    {
        var resultado = await estudianteService.Guardar(estudiante);

        if (resultado.IsSuccess)
        {
            return Ok(estudiante); // 200 OK, devolvemos el estudiante creado
        }
        else
        {
            return BadRequest(resultado.Error);
        }
    }

    // PUT: api/Estudiantes/5
    // Modifica un estudiante existente.
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] Estudiante estudiante)
    {
        if (id != estudiante.EstudianteId)
        {
            return BadRequest("El id de la ruta no coincide con el id del estudiante enviado.");
        }

        var resultado = await estudianteService.Guardar(estudiante);

        if (resultado.IsSuccess)
        {
            return Ok(estudiante);
        }
        else
        {
            return BadRequest(resultado.Error);
        }
    }

    // DELETE: api/Estudiantes/5
    // Elimina un estudiante por su Id.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await estudianteService.Eliminar(id);

        if (resultado.IsSuccess)
        {
            return Ok("Estudiante eliminado correctamente.");
        }
        else
        {
            return NotFound(resultado.Error);
        }
    }
}