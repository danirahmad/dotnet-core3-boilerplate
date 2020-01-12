using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moonlay.Core.Models;
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
        private readonly ISignInService _signIn;

        public CreateModel(INewCustomerProducer newCustomerProducer, ISignInService signIn)
        {
            _producer = newCustomerProducer;
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

            await _producer.Publish(new Topics.MessageHeader
            {
                Token = Guid.NewGuid().ToString(),
                AppOrigin = "Moonlay.WebApp",
                Timestamp = DateTime.Now.ToString("s"),
                CurrentUser = _signIn.CurrentUser,
                IsCurrentUserDemo = _signIn.Demo
            },
            new NewCustomerTopic
            {
                FirstName = this.Form.FirstName,
                LastName = this.Form.LastName
            });

            return RedirectToPage("./Index");
        }
    }
}