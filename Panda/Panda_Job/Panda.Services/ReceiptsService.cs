using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;

using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public class ReceiptsService : IReceiptsService
    {
        private readonly PandaDbContext pandaDbContext;

        public ReceiptsService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public async Task CreateReceipt(Receipt receipt)
        {
            await this.pandaDbContext
                .Receipts.AddAsync(receipt);

            await this.pandaDbContext.SaveChangesAsync();
        }

        public IQueryable<Receipt> GetAllReceiptsWithRecipient()
        {
            var receiptsWithRecipientDb = this.pandaDbContext
                .Receipts
                .Where(r => r.IsDeleted == false)
                .Include(receipt => receipt.Recipient);

            return receiptsWithRecipientDb;
        }

        public IQueryable<Receipt> GetAllReceiptsWithRecipientAndPackage()
        {
            var receiptsAllDb = this.pandaDbContext.Receipts
                .Where(r => r.IsDeleted == false)
                .Include(receipt => receipt.Recipient)
                .Include(receipt => receipt.Package);
            return receiptsAllDb;
        }
    }
}
