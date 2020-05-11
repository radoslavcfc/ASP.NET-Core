using Panda.Domain;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IReceiptsService
    {
        void CreateReceipt(Receipt receipt);

        List<Receipt> GetAllReceiptsWithRecipient();

        List<Receipt> GetAllReceiptsWithRecipientAndPackage();
    }
}
