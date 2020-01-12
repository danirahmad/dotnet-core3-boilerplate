using Microsoft.AspNetCore.Mvc.RazorPages;
using Moonlay.WebApp.Producers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moonlay.WebApp
{
    public class CreateModel : PageModel
    {
        private readonly INewCustomerProducer _newCustomerProducer;

        public CreateModel(INewCustomerProducer newCustomerProducer)
        {
            _newCustomerProducer = newCustomerProducer;
        }

        public Task OnGet()
        {

            return Task.CompletedTask;
        }

        public async Task OnPostAsync()
        {
            var message = new MessageTypes.LogMessage
            {
                IP = "192.168.0.1",
                Message = "a test message 2",
                Severity = MessageTypes.LogLevel.Info,
                Tags = new Dictionary<string, string> { { "location", "CA" } }
            };

            await _newCustomerProducer.Publish(Guid.NewGuid().ToString(), message);
        }
    }
}