using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Models.Client
{
    public class ClientIndexVM
    {
        public string Id { get; set; } = null!;

        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsAdmin { get; set; }
    }
}
