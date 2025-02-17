using RopDemo.Domain;

namespace RopDemo.Services.Data;

public interface IAccountRepository
{
    bool IsUnique(string emailAddress);
    List<Account> GetAccounts();
    Account? GetAccountOrNull(AccountId id);
    Result<Account> GetAccount(AccountId id);
    void Add(Account account);
    void SaveChanges();
}