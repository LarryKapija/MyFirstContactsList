using SQLite;

namespace ContactList.Models
{
    public class Person 
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public bool Done { get; set; }
        public string Name { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ProfilePhoto { get; set; }
    }
}
