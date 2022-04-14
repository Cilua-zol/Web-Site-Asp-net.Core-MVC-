using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RazorApp.Models;

namespace RazorApp.MongoDb
{


    public interface IDbClient
    {
        IMongoCollection<Account> GetAccountCollection();

    }

    public class DbClient : IDbClient
    {
        private IMongoCollection<Account> AccountCollection;

        public DbClient()
        {

            var settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27017");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("BonchChat");

            AccountCollection = database.GetCollection<Account>("Accounts");
        }

        public IMongoCollection<Account> GetAccountCollection() => AccountCollection;

    }
}