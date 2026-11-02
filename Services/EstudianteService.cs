using Aplicada1.Core;
using GestionLibrosApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionLibrosApi.Services;

public class EstudianteService : IServiceResult<Estudiante, int>
{
    private readonly GestionLibrosContext contexto;

    // El servicio recibe el DbContext por inyección de dependencias.
    public EstudianteService(GestionLibrosContext contexto)
    {
        this.contexto = contexto;
    }

    // Devuelve una lista de estudiantes que cumplan con el criterio recibido.
    public async Task<Result<List<Estudiante>>> GetList(Expression<Func<Estudiante, bool>> criterio)
    {
        var lista = await contexto.Estudiantes.Where(criterio).ToListAsync();
        return Result.Success(lista);
    }

    // Busca un solo estudiante por su Id. Puede devolver null si no existe.
    public async Task<Result<Estudiante?>> Buscar(int id)
    {
        var estudiante = await contexto.Estudiantes.FirstOrDefaultAsync(e => e.EstudianteId == id);
        return Result.Success(estudiante);
    }

    // Crea o modifica un estudiante, según si ya existe o no en la base.
    public async Task<Result> Guardar(Estudiante estudiante)
    {
        // Validación: no permitir nombres duplicados.
        var nombreDuplicado = await contexto.Estudiantes
            .AnyAsync(e => e.Nombres == estudiante.Nombres && e.EstudianteId != estudiante.EstudianteId);

        if (nombreDuplicado)
        {
            return Result.Failure(new Error("NOMBRE_DUPLICADO", "Ya existe un estudiante con ese nombre."));
        }

        var existe = await contexto.Estudiantes.AnyAsync(e => e.EstudianteId == estudiante.EstudianteId);

        if (existe)
        {
            // Ya existe: es una modificación.
            contexto.Estudiantes.Update(estudiante);
        }
        else
        {
            // No existe todavía: es una creación nueva.
            contexto.Estudiantes.Add(estudiante);
        }

        var filasAfectadas = await contexto.SaveChangesAsync();

        if (filasAfectadas > 0)
        {
            return Result.Success();
        }
        else
        {
            return Result.Failure(new Error("NO_GUARDADO", "No se pudo guardar el estudiante."));
        }
    }

    // Elimina un estudiante por su Id.
    public async Task<Result> Eliminar(int id)
    {
        var estudiante = await contexto.Estudiantes.FirstOrDefaultAsync(e => e.EstudianteId == id);

        if (estudiante is null)
        {
            return Result.Failure(new Error("NO_ENCONTRADO", "Estudiante no encontrado."));
        }

        contexto.Estudiantes.Remove(estudiante);
        await contexto.SaveChangesAsync();

        return Result.Success();
    }
}