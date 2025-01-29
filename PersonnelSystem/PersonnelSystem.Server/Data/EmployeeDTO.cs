namespace PersonnelSystem.Server.Data
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ManagerId { get; set; }
        public bool isManager { get; set; }
        public List<int> RoleIds { get; set; }
    }

    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
    }
}
