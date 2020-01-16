using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moonlay.Core.Models;
using Moonlay.WebApp.Producers;

namespace Moonlay.WebApp
{
    public class CreateDataSetModel : PageModel
    {
        public class NewDataSetForm
        {
            [Required]
            [MaxLength(64)]
            [Display(Name="Domain")]
            public string DomainName { get; set; }

            [Required]
            [MaxLength(64)]
            public string Name { get; set; }

            [Required]
            [MaxLength(64)]
            [Display(Name = "Organization")]
            public string OrgName { get; set; }
        }

        private readonly INewDataSetProducer _producer;
        private readonly ISignInService _signIn;

        [BindProperty]
        public NewDataSetForm Form { get; set; }

        public CreateDataSetModel(INewDataSetProducer producer, ISignInService signIn)
        {
            _producer = producer;
            _signIn = signIn;
        }

        public void OnGet()
        {

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
            new Topics.MDM.DataSets.NewDataSetTopic { 
                Name = Form.Name,
                DomainName = Form.DomainName,
                OrgName = Form.OrgName
            });

            return RedirectToPage("./Index");
        }
    }
}