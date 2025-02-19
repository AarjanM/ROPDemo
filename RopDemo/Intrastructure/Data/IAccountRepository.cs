using RopDemo.Domain;

namespace RopDemo.Intrastructure.Data;

public interface IAccountRepository
{
    bool IsUnique(string emailAddress);
    List<Account> GetAccounts();
    Account? GetAccountOrNull(AccountId id);
    Result<Account> GetAccount(AccountId id);
    void Add(Account account);
    void SaveChanges();
}