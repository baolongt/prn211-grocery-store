using System.ComponentModel.DataAnnotations;
namespace PRN211_Grocery_store.Models.ViewModels
{
    public class PasswordChange
    {
        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "Re-password is required")]
        [Compare(otherProperty: "newPassword", ErrorMessage = "New password doesn't match.")]
        [DataType(DataType.Password)]
        public string rePassword { get; set; }
    }
}
