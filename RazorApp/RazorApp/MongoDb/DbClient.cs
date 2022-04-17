using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RazorApp.Methods;
using RazorApp.Models;

namespace RazorApp.MongoDb
{


    public interface IDbClient
    {
        IMongoCollection<Account> GetAccountCollection();
        IMongoCollection<Wallet> GetWalletCollection();

    }

    public class DbClient : IDbClient
    {
        private IMongoCollection<Account> AccountCollection;
        
        private IMongoCollection<Wallet> WalletCollection;

        public DbClient()
        {

            var settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27017");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("BonchBank");

            AccountCollection = database.GetCollection<Account>("Accounts");
            WalletCollection = database.GetCollection<Wallet>("Wallets");
        }

        public IMongoCollection<Account> GetAccountCollection() => AccountCollection;
        public IMongoCollection<Wallet> GetWalletCollection() => WalletCollection;

    }
}