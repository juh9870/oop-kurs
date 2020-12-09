using System;
using System.Text.RegularExpressions;

namespace Logic.Model
{
    public class BankAccount : IEquatable<BankAccount>
    {
        public const string Empty = "0000 0000 0000 0000";
        private static readonly Regex AccountNumberFormatRegex = new Regex(@"^[\d ]+$");

        public BankAccount(string id)
        {
            id = id.Replace(" ", null);
            CheckCodeValidity(id);
            Id = id;
        }

        public string Id { get; }

        public bool Equals(BankAccount other)
        {
            return AccountEquals(this, other);
        }

        public static void CheckCodeValidity(string accountNumber)
        {
            if (accountNumber.Replace(" ", null).Length != 16) throw new InvalidAccountNumberLengthException();
            if (!AccountNumberFormatRegex.IsMatch(accountNumber)) throw new InvalidAccountNumberCharacterException();
        }

        public override string ToString()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            return obj is BankAccount acc && AccountEquals(this, acc);
        }

        public override int GetHashCode()
        {
            return Id != null ? Id.GetHashCode() : 0;
        }

        private static bool AccountEquals(BankAccount a, BankAccount b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (!(a is null) && !(b is null)) return a.Id == b.Id;

            return false;
        }

        public static bool operator ==(BankAccount a, BankAccount b)
        {
            return AccountEquals(a, b);
        }

        public static bool operator !=(BankAccount a, BankAccount b)
        {
            return !AccountEquals(a, b);
        }

        public class AccountNumberFormatException : Exception
        {
        }

        public class InvalidAccountNumberLengthException : AccountNumberFormatException
        {
        }

        public class InvalidAccountNumberCharacterException : AccountNumberFormatException
        {
        }
    }
}