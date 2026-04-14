using System.Text.Json.Serialization;

namespace Talabat.Core.Entity.Identity
{
    public class Address:BaseEntity
    {
        public Address(string firstName,string lastName,string street,string city,string country)
        {
            FirstName= firstName;
            LastName= lastName;
            Street= street;
            City= city;
            Country= country;

        }
    
        public string FirstName { get; set; } = null!;
        public string LastName{ get; set; } = null!;
        public string Street{ get; set; } = null!;
        public string City{ get; set; }= null!;
        public string Country { get; set; } = null!;
        
        //[JsonIgnore]
        public string ApplicationUserId { get; set; } = null!;//Foreign Key


       // [JsonIgnore]
        public ApplicationUser User { get; set; } = null!;//Navigational property [One]

    }
}