namespace IdentityService.Model
{
    /// <summary>
    /// A model to represent user
    /// </summary>
    public class UserResponse
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public List<Address> UserAddresses { get; set; } = new List<Address>();
    }

    public partial class Address
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; } = null!;

        public string? AddressLine2 { get; set; }

        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public string PostalCode { get; set; } = null!;

        public string Country { get; set; } = null!;

        public bool? IsDefault { get; set; }
    }

}
