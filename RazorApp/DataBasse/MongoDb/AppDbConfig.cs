
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RazorApp.Models;
using Temas_Backend_Api.MongoDb;

namespace Temas_Backend_Api.MongoDb
{
    public interface IDbClient
    {
        IMongoCollection<Account> GetAccountCollection();
        
    }
    public class DbClient : IDbClient
    {
        private IMongoCollection<Account> AccountCollection;
        


        public DbClient(IOptions<BlogDbConfig> walletDbConfig)
        {
            
            var settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27017");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("BonchChat");
           
            AccountCollection = database.GetCollection<Account>("Accounts");
        } 
        public IMongoCollection<Account> GetAccountCollection() => AccountCollection;
       
    }

    public class BlogDbConfig
    {
    }
}
