namespace backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required Address Address { get; set; }
        public required string Phone { get; set; }
        public required string Website { get; set; }
        public required Company Company { get; set; }
    }

    public class Address
    {
        public required string Street { get; set; }
        public required string Suite { get; set; }
        public required string City { get; set; }
        public required string Zipcode { get; set; }
        public required Geo Geo { get; set; }
    }

    public class Geo
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Company
    {
        public required string Name { get; set; }
        public required string CatchPhrase { get; set; }
        public required string Bs { get; set; }
    }
}
