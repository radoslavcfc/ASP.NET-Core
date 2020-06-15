using Panda.Domain;

using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public interface IReceiptsService
    {
        Task CreateReceipt(Receipt receipt);

        IQueryable<Receipt> GetAllReceiptsWithRecipient();

        IQueryable<Receipt> GetAllReceiptsWithRecipientAndPackage();
    }
}
