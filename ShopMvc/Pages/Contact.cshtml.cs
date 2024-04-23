using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace ShopMvc.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {

        }
        [BindProperty]
        [Required(ErrorMessage = "First Name is reuqired!")]
        public string FirstName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Last Name is reuqired!")]
        public string LastName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = " Email is reuqired!")]
        [EmailAddress]
        [Display(Name ="Email*")]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Phone { get; set; } = "";

        [BindProperty,Required]
        public string Subject { get; set; } = "";

        [BindProperty, Required(ErrorMessage = " Message is reuqired!")]
        [MinLength(5, ErrorMessage = "The characterst should be at least  5")]
        [MaxLength(1024, ErrorMessage = "The characters should be less than 1024")]
        [Display(Name="Message*")]
        public string Message { get; set; } = "";

        public List<SelectListItem> SubjectList { get; } = new List<SelectListItem> { 
        
        new SelectListItem {Value = "Order Status", Text="Order Status"},
        new SelectListItem {Value = "Refund Request", Text = "Refund Request"},
        new SelectListItem {Value = "Job Application", Text = "Job Application"},
        new SelectListItem {Value = "Other", Text = "Other"},
        };

        public string SuccessMessage { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnPost()
        {
            FirstName = Request.Form["firstname"];
            LastName = Request.Form["lastname"];
            Email = Request.Form["email"];
            Phone = Request.Form["phone"];
            Subject = Request.Form["subject"];
            Message = Request.Form["message"];

            //check if any required field is emtpy
            if (!ModelState.IsValid)
            {
                //Error
                ErrorMessage = "Please fill all required fields";
                return;
            }
            if (Phone == null) Phone = "";

            //Add this message to database
            try
            {
                string connectionString = "Data Source=DESKTOP-15CV0PF\\SQLEXPRESS;Initial Catalog=shop;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO messages " +
                        "(firstname, lastname, email, phone, subject, message) VALUES " +
                        "(@firstname, @lastname, @email, @phone, @subject, @message);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@lastname", LastName);
                        command.Parameters.AddWithValue("@email", Email);
                        command.Parameters.AddWithValue("@phone", Phone);
                        command.Parameters.AddWithValue("@subject", Subject);
                        command.Parameters.AddWithValue("@message", Message);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            //Send Confirmation Email tp the client

            SuccessMessage = "Your message has been received correctly";

            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            Subject = "";
            Message = "";

            ModelState.Clear(); 
        }
    }
}
