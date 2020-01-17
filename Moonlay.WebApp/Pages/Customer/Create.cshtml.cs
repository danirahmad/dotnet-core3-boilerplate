using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moonlay.Confluent.Kafka;
using Moonlay.Core.Models;
using Moonlay.Topics.Customers;
using System;
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

        private readonly IKafkaProducer _producer;
        private readonly ISignInService _signIn;

        public CreateModel(IKafkaProducer producer, ISignInService signIn)
        {
            _producer = producer;
            _signIn = signIn;
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

            await _producer.Publish("new-customer-topic2", _signIn.GenMessageHeader(),
            new NewCustomerTopic
            {
                FirstName = this.Form.FirstName,
                LastName = this.Form.LastName
            });

            return RedirectToPage("./Index");
        }
    }
}