using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moonlay.Topics.Customers;
using Moonlay.WebApp.Producers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Moonlay.WebApp
{
    public class CreateModel : PageModel
    {
        public class NewCustomerForm
        {
            [MaxLength(64)]
            [Required]
            public string FirstName { get; set; }

            [MaxLength(64)]
            public string LastName { get; set; }
        }

        [BindProperty]
        public NewCustomerForm Form { get; set; }

        private readonly INewCustomerProducer _producer;

        public CreateModel(INewCustomerProducer newCustomerProducer)
        {
            _producer = newCustomerProducer;
        }

        public Task OnGet()
        {

            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _producer.Publish(Guid.NewGuid().ToString(), new NewCustomerTopic { FirstName = this.Form.FirstName, LastName = this.Form.LastName });

            return RedirectToPage("./Index");
        }
    }
}