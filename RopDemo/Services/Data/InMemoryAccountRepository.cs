using RopDemo.Domain;

namespace RopDemo.Services.Data;

internal sealed class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<Guid, Account> _accounts = [];

    public InMemoryAccountRepository()
    {
        Add(Account.CreateImperative("Aarjan", "Meirink", "aarjan.meirink@rtl.nl",
            TelephoneNumber.CreateImperative("0643545143"), DateTime.Now));
        Add(Account.CreateImperative("Saeed", "Salehi", "saeed.salehi@rtl.nl",
            TelephoneNumber.Empty(), DateTime.Now));
        Add(Account.CreateImperative("Neel", "Bhatt", "neel.bhatt@rtl.nl",
            TelephoneNumber.Empty(), DateTime.Now));
    }
    
    public bool IsUnique(string emailAddress) => _accounts.Values.All(account => account.EmailAddress != emailAddress);
    public List<Account> GetAccounts() => _accounts.Values.ToList();
    public Account? GetAccountOrNull(AccountId id) => _accounts.Values.FirstOrDefault(account => account.Id == id);
    public Result<Account> GetAccount(AccountId id) => 
        _accounts.Values.FirstOrDefault(account1 => account1.Id == id)
        ?? Result.Failure<Account>(new NotFoundError("Account.NotFound", $"Account with id: {id} not found."));

    public void Add(Account account) => _accounts.Add(account.Id.Value, account);

    public void SaveChanges()
    {
        //
    }
}