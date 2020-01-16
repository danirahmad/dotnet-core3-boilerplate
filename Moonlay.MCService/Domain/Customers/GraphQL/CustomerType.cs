using GraphQL.Types;
using Moonlay.MasterData.WebApi.Models;

namespace Moonlay.MasterData.WebApi.Customers.GraphQL
{
    public class CustomerType : ObjectGraphType<Customer>
    {
        public CustomerType()
        {
            Name = "Customer";
            Description = "A type to define the Customer.";

            Field(d => d.Id).Description("Customer Key");
            Field(d => d.FirstName, nullable: true).Description("Customer first name.");
            Field(d => d.LastName, nullable: true).Description("Customer last name.");

            Field(d => d.CreatedBy, nullable: true).Description("who is create the record");
            Field(d => d.CreatedAt, nullable: true).Description("when the record is created");

            Field(d => d.UpdatedBy, nullable: true).Description("who is update the record");
            Field(d => d.UpdatedAt, nullable: true).Description("the last time when the record was updated");
        }
    }
}