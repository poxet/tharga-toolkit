using SampleDataTransfer.Entities;
using Tharga.Toolkit.ServerStorage;

namespace SampleConsoleHost.Business
{
    public class CustomerBusiness : BusinessBase<CustomerDto, CustomerDto>
    {
        protected override CustomerDto Convert(CustomerDto input)
        {
            return input;
        }
    }
}