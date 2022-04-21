using System.Transactions;
using MongoDB.Driver;
using RazorApp.Models;
using RazorApp.MongoDb;
using Transaction = System.Transactions.Transaction;

namespace RazorApp.Methods
{
    public interface ITransactionMethods
    {
        Task<Operation> Replenishment(string email, double sum);
        Task<List<Operation>> GetAllOperations(string email);
        Task<Trans> Transaction(string email, string Card, string phone, double summ, string description);
    }
    public class TransactionMethods : ITransactionMethods
    {
        private readonly IMongoCollection<Operation> _operationCollection;
        private readonly IMongoCollection<Wallet> _walletCollection;
        private readonly IMongoCollection<Trans> _transCollection;
        private IStorage Storage { get; }
        private IWalletMethods WalletMethods { get; }
        public TransactionMethods(IDbClient client, IStorage storage, IWalletMethods walletMethods)
        
        {
            _operationCollection = client.GetOperationCollection();
            _walletCollection = client.GetWalletCollection();
            _transCollection = client.GetTransCollection();
            Storage = storage;
            WalletMethods = walletMethods;
        }

        public async Task TransactionByEvr(Wallet walletOut, Wallet walletTo, double summ)
        {
            if (await Storage.CheckWalletStatus(walletTo) && await Storage.CheckWalletStatus(walletOut))
            {
                if (walletOut.Balance >= summ)
                {
                    walletOut.Balance -= summ;
                    walletTo.Balance += summ;
                    await Storage.UpdateWalletBallance(walletOut); await Storage.UpdateWalletBallance(walletTo);
                    
                }
            }
        }
        public async Task<Trans> Transaction(string email, string Card, string phone, double summ, string description)
        {
            Trans transaction = new Trans();
            transaction.TransactionId = Guid.NewGuid().ToString("N");
            transaction.TransactionTime = DateTime.UtcNow;
            transaction.EmailPost = email;
            transaction.Summ = summ;
            transaction.Card = Card;
            transaction.Phone = phone;
            transaction.Description = description;
            transaction.IsCompleted = false;
            
            var walletOut = await WalletMethods.GetWalletByEmail(email) ;
            if(transaction.Phone != null)
            {
                var walletTo = await Storage.GetWalletByPhoneNymber(phone);
                await TransactionByEvr(walletOut, walletTo, summ);
                transaction.IsCompleted = true;
            }

            if (transaction.Phone == null && transaction.Card != null)
            {
                var walletTo = await _walletCollection.Find(u => u.СardNumber == Card).FirstOrDefaultAsync();
                await TransactionByEvr(walletOut, walletTo, summ);
                transaction.IsCompleted = true;
            }
            await _transCollection.InsertOneAsync(transaction);
            return transaction;

        }

        public async Task<Operation> Replenishment(string email, double sum)
        {
            Operation operation = new Operation();
            operation.Id = Guid.NewGuid().ToString("N");
            operation.OperationTime = DateTime.UtcNow;
            operation.Summ = sum;
            operation.Email = email;
            operation.OperationType = Operation.Type.Replenishment;
            operation.IsCompleted = false;
            Wallet wallet = await WalletMethods.GetWalletByEmail(email);
            bool walletStatus = await Storage.CheckWalletStatus(wallet);
            if (walletStatus == true && wallet is not null)
            {

                wallet.Balance += sum;
                await Storage.UpdateWalletBallance(wallet);
                operation.IsCompleted = true;
            }

            await _operationCollection.InsertOneAsync(operation);
            return operation;
        }

        public async Task<List<Operation>> GetAllOperations(string email)
        {
            var operations = await _operationCollection.Find(u => u.IsCompleted == true && u.Email == email).ToListAsync();
            return operations;
        }
    }
}