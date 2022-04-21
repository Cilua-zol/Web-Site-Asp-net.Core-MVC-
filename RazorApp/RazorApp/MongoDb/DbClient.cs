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
        IMongoCollection<SessionModel> GetSessionCollection();
        IMongoCollection<Operation> GetOperationCollection();
        IMongoCollection<Trans> GetTransCollection();

    }

    public class DbClient : IDbClient
    {
        private IMongoCollection<Account> AccountCollection;
        
        private IMongoCollection<Wallet> WalletCollection;
        
        private IMongoCollection<SessionModel> SessionCollection;
        private IMongoCollection<Operation> OperationCollection;
        private IMongoCollection<Trans> TransCollection;
        public DbClient()
        {

            var settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27017");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("BonchBank");

            AccountCollection = database.GetCollection<Account>("Accounts");
            WalletCollection = database.GetCollection<Wallet>("Wallets");
            SessionCollection = database.GetCollection<SessionModel>("Sessions");
            OperationCollection = database.GetCollection<Operation>("Operations");
            TransCollection = database.GetCollection<Trans>("Trans");



        }

        public IMongoCollection<Account> GetAccountCollection() => AccountCollection;
        public IMongoCollection<Wallet> GetWalletCollection() => WalletCollection;
        public IMongoCollection<Operation> GetOperationCollection() => OperationCollection;
        public IMongoCollection<SessionModel> GetSessionCollection() => SessionCollection;
        public IMongoCollection<Trans> GetTransCollection() => TransCollection;

    }
}