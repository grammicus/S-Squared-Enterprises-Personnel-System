using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonnelSystem.Server.Data;
using PersonnelSystem.Server.Models;

namespace PersonnelSystem.Server.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;

        private static EmployeeDTO EmployeeDTO(Employee employee) =>
            new EmployeeDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                ManagerId = employee.ManagerId,
            };

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
        }

        private static Employee CreateEmployeeFromRequest(EmployeeDTO employeeDTO)
        {
            if (employeeDTO.isManager == true)
            {
                return new Manager()
                {
                    EmployeeId = employeeDTO.EmployeeId,
                    FirstName = employeeDTO.FirstName,
                    LastName = employeeDTO.LastName,
                    ManagerId = employeeDTO.ManagerId,
                };
            }
            return new Employee()
            {
                EmployeeId = employeeDTO.EmployeeId,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                ManagerId = employeeDTO.ManagerId,
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees(
            int pageIndex = 0,
            int pageSize = 10
        )
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            return await _context
                .Employees.Select(x => EmployeeDTO(x))
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        [HttpGet("managers")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetManagers()
        {
            if (_context.Managers == null)
            {
                return NotFound();
            }
            return await _context.Managers.Select(x => EmployeeDTO(x)).ToListAsync();
        }

        [HttpGet("managers/{id}/reports")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetReports(
            int id,
            int pageIndex = 0,
            int pageSize = 10
        )
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }

            return await _context
                .Employees.Where(m => m.ManagerId == id)
                .Select(x => EmployeeDTO(x))
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> PostEmployee(
            [FromBody] EmployeeDTO employeeDTO
        )
        {
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var employee = CreateEmployeeFromRequest(employeeDTO);
                if (employeeDTO.RoleIds != null && employeeDTO.RoleIds.Any())
                {
                    var roles = await _context
                        .Roles.Where(r => employeeDTO.RoleIds.Contains(r.RoleId))
                        .ToListAsync();
                    foreach (var role in roles)
                    {
                        employee.Roles.Add(role);
                    }
                }

                _context.Add(employee);
                await _context.SaveChangesAsync();

                return Ok(employeeDTO);
            }
        }
    }

    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public RoleController(EmployeeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            return await _context.Roles.Select(x => RoleDTO(x)).ToListAsync();
        }

        private static RoleDTO RoleDTO(Role role) =>
            new RoleDTO { RoleId = role.RoleId, Name = role.Name };
    }
}
