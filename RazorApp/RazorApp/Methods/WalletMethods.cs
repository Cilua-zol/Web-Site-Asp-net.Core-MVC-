using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;
using RazorApp.Models;
using RazorApp.MongoDb;

namespace RazorApp.Methods
{
    public interface IWalletMethods
    {
        Task<Wallet> CreateWallet(string cardNumber, string password, string phoneNumber);
    }

    public class WalletMethods : IWalletMethods
    {
        private readonly IMongoCollection<Wallet> _walletCollection;

        public WalletMethods(IDbClient dbClient)
        {
            _walletCollection = dbClient.GetWalletCollection();
        }
        //Added Wallet to Bd 
        public async Task<Wallet> CreateWallet(string cardNumber, string password, string phoneNumber)
        {
            Wallet wallet = new Wallet();
            wallet.Id = Guid.NewGuid().ToString("N");
            wallet.СardNumber = cardNumber;
            wallet.PhoneNumber = phoneNumber;
            wallet.Pass = await GetHash(password);
            wallet.Balance = 0;
            wallet.WalletStatus = Wallet.Status.Active;
            
            await _walletCollection.InsertOneAsync(wallet);
            return wallet;
        }

        #region Helpers
        
        public async Task<string> GetHash(string data)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }


        #endregion
}
}
