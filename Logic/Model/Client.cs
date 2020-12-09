using System;
using Data.Bundles;

namespace Logic.Model
{
    public class Client : IBundlable
    {
        public Client(string firstName = "", string lastName = "", BankAccount bankAccount = null,
            Requirements requirements = null)
        {
            Id = Guid.NewGuid().ToString();
            FirstName = firstName;
            LastName = lastName;
            BankAccount = bankAccount ?? new BankAccount(BankAccount.Empty);
            Requirements = requirements ?? new Requirements();
        }

        public string Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BankAccount BankAccount { get; set; }

        public Requirements Requirements { get; set; }

        public void StoreInBundle(Bundle bundle)
        {
            bundle.Put(nameof(Id), Id);
            bundle.Put(nameof(FirstName), FirstName);
            bundle.Put(nameof(LastName), LastName);
            bundle.Put(nameof(BankAccount), BankAccount.Id);
            bundle.Put(nameof(Requirements), Requirements);
        }

        public void RestoreFromBundle(Bundle bundle)
        {
            Id = bundle.GetString(nameof(Id));
            FirstName = bundle.GetString(nameof(FirstName));
            LastName = bundle.GetString(nameof(LastName));
            BankAccount = new BankAccount(bundle.GetString(nameof(BankAccount)));
            Requirements = bundle.GetBundlable<Requirements>(nameof(Requirements));
        }
    }
}