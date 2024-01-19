using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
	public class RegisterViewModel
	{
		[Required]
		[Display(Name = "KullanıcıAdı")]
		public string? UserName { get; set; }

		[Required]
		[Display(Name = "AdSoyad")]
		public string? Name { get; set; }


		[Required]
		 [EmailAddress]
		 [Display(Name ="Eposta")]
         public string? Email { get; set; }


		[Required]
		[StringLength(10,ErrorMessage ="{0} alanı en az {2}karakter uzulupğunda olmalıdır",MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Parola")]
		public string? Password { get; set; }


		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Parola Tekrarı")]
		[Compare(nameof(Password),ErrorMessage ="parola eşleşmiyor")]
		public string? ConfirmPassword { get; set; }




	}
}
