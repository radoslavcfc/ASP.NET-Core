using System.Linq;
using System.Threading.Tasks;

using Panda.Domain;

namespace Panda.Services
{
    public interface IReceiptsService
    {
        Task CreateReceiptAsync(Receipt receipt);

        IQueryable<Receipt> GetAllReceiptsWithRecipient();

        IQueryable<Receipt> GetAllReceiptsWithRecipientAndPackage();
    }
}
