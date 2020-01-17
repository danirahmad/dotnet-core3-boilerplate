using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moonlay.Confluent.Kafka;
using Moonlay.Core.Models;
using Moonlay.WebApp.Clients;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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

        private readonly IKafkaProducer _producer;
        private readonly IManageDataSetClient _dataSetClient;
        private readonly ISignInService _signIn;

        [BindProperty]
        public NewDataSetForm Form { get; set; }

        public CreateDataSetModel(IManageDataSetClient dataSetClient, ISignInService signIn)
        {
            _dataSetClient = dataSetClient;
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

            var reply = await _dataSetClient.NewDatasetAsync(new MasterData.Protos.NewDatasetReq { Name = Form.Name, DomainName = Form.DomainName, OrganizationName = Form.OrgName });

            return RedirectToPage("./Index");
        }
    }
}