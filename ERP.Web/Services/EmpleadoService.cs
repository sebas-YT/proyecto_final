namespace ERP.Web.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Web.Domain.Dto;
using ERP.Web.Domain.Entities;
using ERP.Web.Data;

public interface IEmpleadoService
{
    Task<List<EmpleadoDto>> Consultar(string filtro);
    Task<bool> Crear(EmpleadoDto request);
    Task<bool> Eliminar(int Id);
    Task<bool> Modificar(EmpleadoDto request);
}

public class EmpleadoService : IEmpleadoService
{
    private readonly AppDbContext _context;
    public EmpleadoService(AppDbContext context)
    {
        _context = context;
    }
    ///Consultar los empleados existentes
    public async Task<List<EmpleadoDto>> Consultar(string filtro)
    {
        var empleados = await
            _context.Empleados
            .Include(c => c.DatosPersonales)
            .Where(c => c.DatosPersonales.Nombre.Contains(filtro))
            .Select(
                c =>
                new EmpleadoDto()
                {
                    Id = c.Id,
                    PersonaId = c.PersonaId,
                    Sueldo = c.Sueldo,
                    DatosPersonales = new PersonaDto()
                    {
                        Id = c.DatosPersonales.Id,
                        Nombre = c.DatosPersonales.Nombre,
                        FechaDeNacimiento = c.DatosPersonales.FechaDeNacimiento
                    }
                }
            )
            .ToListAsync();
        return empleados;
    }
    public async Task<bool> Crear(EmpleadoDto request)
    {
        var empleado = Empleado.Create(
            request.DatosPersonales.Nombre,
            request.DatosPersonales.FechaDeNacimiento,
            request.Sueldo
        );
        _context.Empleados.Add(empleado);
        ;
        return (await _context.SaveChangesAsync()) > 0;
    }
    public async Task<bool> Modificar(EmpleadoDto request)
    {
        //1. Busco el empleado
        var empleado = await _context.Empleados
            .Include(c => c.DatosPersonales)
            .FirstOrDefaultAsync(c => c.Id == request.Id);
        if (empleado == null)
            return false;
        //2. Modifico los datos
        empleado.DatosPersonales.Nombre = request.DatosPersonales.Nombre;
        empleado.DatosPersonales.FechaDeNacimiento = request.DatosPersonales.FechaDeNacimiento;
        empleado.Sueldo = request.Sueldo;
        //3. Guardo los cambios
        return (await _context.SaveChangesAsync()) > 0;
    }
    public async Task<bool> Eliminar(int Id)
    {
        //1. Busco el empleado
        var empleado = await _context.Empleados
            .Include(c => c.DatosPersonales)
            .FirstOrDefaultAsync(c => c.Id == Id);
        if (empleado == null)
            return false;
        //2. Elimino
        //2. Elimino
        _context.Empleados.Remove(empleado);
        //3. Guardo los cambios
        return (await _context.SaveChangesAsync()) > 0;
    }
}
    


 