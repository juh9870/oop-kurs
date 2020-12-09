using Logic.Model;
using NUnit.Framework;

namespace LogicTests
{
    public class BankAccountTests
    {
        [Test]
        public void Should_Throw_Error_On_Invalid_Code_Length([Values(
                "0000 0000 0000 0000 0",
                "0000 0000 0000 000",
                ""
            )]
            string code)
        {
            Assert.Throws<BankAccount.InvalidAccountNumberLengthException>(() => { new BankAccount(code); });
        }

        [Test]
        public void Should_Throw_Error_On_Invalid_Code_Char([Values(
                "0000 0000 0000 00a0",
                "0000 0000 0000 0zz0",
                "00d0 0000 0000 0zz0"
            )]
            string code)
        {
            Assert.Throws<BankAccount.InvalidAccountNumberCharacterException>(() => { new BankAccount(code); });
        }

        [Test]
        public void Should_Parse_With_Any_Spaces([Values(
                "0000 0000 0000 0000",
                "0000000000000000",
                "0000 00000000 0000",
                "0000 000000000000",
                "000000000000 0000",
                "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0"
            )]
            string code)
        {
            Assert.DoesNotThrow(() => { new BankAccount(code); });
        }

        [Test]
        public void Should_Parse_With_Any_Numbers([Values(
                "1234 5678 9012 3456",
                "9999 9999 9999 9999",
                "1342 5876 0798 4365"
            )]
            string code)
        {
            Assert.DoesNotThrow(() => { new BankAccount(code); });
        }

        [Test]
        public void Should_Parse_Own_Statics([Values(
                BankAccount.Empty
            )]
            string code)
        {
            Assert.DoesNotThrow(() => { new BankAccount(code); });
        }

        [Test]
        public void Comparison()
        {
            var a = new BankAccount("0000 1111 2222 3333");
            var a2 = new BankAccount("0000 1111 2222 3333");
            var b = new BankAccount("1122 3344 5566 7788");

            Assert.IsTrue(a == a2);
            Assert.IsTrue(a != b);
            Assert.IsFalse(a == b);
            Assert.IsTrue(a.Equals(a));
            Assert.IsTrue(a.Equals(a2));
            Assert.IsTrue(Equals(a, a2));
            Assert.IsFalse(a.Equals(null));

            Assert.AreEqual(a.ToString(), a2.ToString());
            Assert.AreEqual(a.GetHashCode(), a2.GetHashCode());
        }
    }
}