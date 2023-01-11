namespace DemoApp
{
    internal class Slides
    {
        private readonly object _lock = new object();

        private void TransferMoneyUnsafe(decimal amount, Account source, Account target)
        {
            if (source.Balance < amount)
            {
                return;
            }

            source.Balance -= amount;
            target.Balance += amount;
        }

        private void TransferMoney(decimal amount, Account source, Account target)
        {
            lock (_lock)
            {
                if (source.Balance < amount)
                {
                    return;
                }

                source.Balance -= amount;
                target.Balance += amount;
            }
        }

        private string Report(Account a, Account b)
        {
            lock (_lock)
            {
                return $"A = {a.Balance} B={b.Balance}";
            }
        }

        public void Run()
        {
            var source = new Account();
            var target = new Account();

            source.Balance = 500;
            target.Balance = 500;

            TransferMoney(300, source, target);


            TransferMoney(300, source, target);
            TransferMoneyUnsafe(300, source, target);

            Report(source, target);
        }

        private class Account
        {
            public decimal Balance { get; set; }
        }
    }
}