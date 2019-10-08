using System.Collections.Generic;

namespace ProgramareAvansataCA.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<CollectionViewModel> Collections { get; set; }
    }

    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
