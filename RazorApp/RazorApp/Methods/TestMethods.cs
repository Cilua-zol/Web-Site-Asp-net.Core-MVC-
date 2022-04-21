using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;
using RazorApp.Models;
using RazorApp.MongoDb;

namespace RazorApp.Methods;

public interface ITestMethods
{
    Task<Account> Do(string log, string pass);
    Task<Account> GetAccByLogin(string login);
}

public class TestMethods : ITestMethods
{
    private readonly IMongoCollection<Account> _accountCollection;

    public TestMethods(IDbClient client)
    {
        _accountCollection = client.GetAccountCollection();
    }

    public async Task<Account> Do(string log, string pass)
    {
        Account account = new Account();
        account.Email = log;
        account.Password = pass;

        await _accountCollection.InsertOneAsync(account);
        return account;
    }

    public async Task<Account> GetAccByLogin(string login)
    {
        var acc = await _accountCollection.Find(x => x.Email== login).FirstOrDefaultAsync();
        return acc;
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
    public async Task<string> GetCode()
    {
        int _min = 000000;
        int _max = 999999;
        Random _rdm = new Random();
        return _rdm.Next(_min, _max).ToString();
    }
    

    #endregion

}