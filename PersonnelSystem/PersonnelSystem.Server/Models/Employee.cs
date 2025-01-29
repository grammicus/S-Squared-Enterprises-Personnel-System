using System.ComponentModel.DataAnnotations;

namespace PersonnelSystem.Server.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public List<Role> Roles { get; set; } = [];

        public int? ManagerId { get; set; }
        public Manager Manager { get; set; }
    };

    public class Manager : Employee
    {
        public ICollection<Employee> Reports { get; } = new List<Employee>();
    };
}
