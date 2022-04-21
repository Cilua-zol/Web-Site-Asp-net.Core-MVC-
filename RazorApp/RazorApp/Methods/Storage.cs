using MongoDB.Driver;
using RazorApp.Models;
using RazorApp.MongoDb;

namespace RazorApp.Methods
{
    public interface IStorage
    {
        Task<bool> CheckWalletStatus(Wallet wallet);
        Task UpdateWalletBallance(Wallet wallet);
        Task<Wallet> GetWalletByPhoneNymber(string phone);

    }
    public class Storage: IStorage
    {
        private readonly IMongoCollection<Wallet> _walletCollection;
        public Storage(IDbClient client)
        {
            _walletCollection = client.GetWalletCollection();
        }
        public async Task<bool> CheckWalletStatus(Wallet wallet)
        {
            return wallet.WalletStatus == Wallet.Status.Active || wallet.WalletStatus == Wallet.Status.Returned;
        }

        public async Task<Wallet> GetWalletByPhoneNymber(string phone)
        {
            Wallet wallet = await _walletCollection.Find(u => u.PhoneNumber == phone).FirstOrDefaultAsync();
            return wallet;
        }
        public async Task UpdateWalletBallance(Wallet wallet)
        {
            try
            {
                var filter = Builders<Wallet>.Filter.Eq(e => e.Id, wallet.Id);
                var update = Builders<Wallet>.Update
                    .Set(u => u.Balance, wallet.Balance);
                await _walletCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}