using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Panda.Services
{
    public class ReceiptsService : IReceiptsService
    { 
        private readonly PandaDbContext pandaDbContext;

        public ReceiptsService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public void CreateReceipt(Receipt receipt)
        {
            try
            {
                this.pandaDbContext.Receipts.Add(receipt);

                this.pandaDbContext.SaveChanges();
            }
            catch (DBConcurrencyException)
            {
                //TODO
                throw;
            }
            catch (DbUpdateException) 
            {
                //TODO
                throw;
            }
        }

        public List<Receipt> GetAllReceiptsWithRecipient()
        {
            var receiptsWithRecipientDb = this.pandaDbContext.Receipts.Include(receipt => receipt.Recipient).ToList();

            return receiptsWithRecipientDb;
        }

        public List<Receipt> GetAllReceiptsWithRecipientAndPackage()
        {
            var receiptsAllDb = this.pandaDbContext.Receipts
                .Include(receipt => receipt.Recipient)
                .Include(receipt => receipt.Package)
                .ToList();

            return receiptsAllDb;
        }
    }
}
