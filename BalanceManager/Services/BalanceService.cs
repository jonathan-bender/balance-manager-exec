using System;
using System.Linq;
using BalanceManager.DAL;

namespace BalanceManager.Services
{
    public class BalanceService
    {
        #region Fields

        private readonly IBalancesContext _balancesContext;

        private static readonly object _creationLock = new object();
        private static readonly object _balanceAmountLock = new object();

        #endregion

        #region Ctor

        public BalanceService(IBalancesContext balancesContext)
        {
            _balancesContext = balancesContext;
        }

        #endregion

        #region Utilities

        private void UpdateBalance(BalanceInfo balanceInfo, float amount)
        {
            if (balanceInfo == null)
                throw new ArgumentNullException("balanceInfo");

            balanceInfo.Amount = amount;
            balanceInfo.UpdateDate = DateTime.UtcNow;
        }

        #endregion

        #region Methods

        public BalanceInfo GetById(int id)
        {
            return _balancesContext.Balances.FirstOrDefault(c => c.BalanceId == id);
        }

        public BalanceInfo CreateBalance()
        {
            lock (_creationLock)
            {
                var prevId = _balancesContext.Balances.Select(c => c.BalanceId).OrderByDescending(c => c).FirstOrDefault();
                var id = prevId + 1;
                var now = DateTime.UtcNow;

                var balanceInfo = new BalanceInfo
                {
                    BalanceId = id,
                    CreateDate = now,
                    UpdateDate = now
                };

                _balancesContext.Balances.Add(balanceInfo);

                return balanceInfo;
            }
        }

        public void Charge(int balanceId, float amount)
        {
            lock (_balanceAmountLock)
            {
                var balanceInfo = GetById(balanceId);
                if (balanceInfo == null)
                    throw new Exception("Balance not found");

                var balanceAmount = balanceInfo.Amount - amount;
                if (balanceAmount < 0)
                    throw new Exception("Balance was not charged. No sufficient funds for the charge.");

                UpdateBalance(balanceInfo, balanceAmount);
            }
        }

        public void Load(int balanceId, float amount)
        {
            lock (_balanceAmountLock)
            {
                var balanceInfo = GetById(balanceId);
                if (balanceInfo == null)
                    throw new Exception("Balance not found");

                var balanceAmount = balanceInfo.Amount + amount;

                UpdateBalance(balanceInfo, balanceAmount);
            }
        }

        public void Transfer(int fromBalanceId, int toBalanceId, float amount)
        {
            lock (_balanceAmountLock)
            {
                var fromBalanceInfo = GetById(fromBalanceId);
                if (fromBalanceInfo == null)
                    throw new Exception("From balance not found");

                var toBalanceInfo = GetById(toBalanceId);
                if (toBalanceInfo == null)
                    throw new Exception("To balance not found");

                var fromBalanceAmount = fromBalanceInfo.Amount - amount;
                var toBalanceAmount = toBalanceInfo.Amount + amount;
                if (fromBalanceAmount < 0)
                    throw new Exception("Balance was not charged. No sufficient funds for the charge.");


                var now = DateTime.UtcNow;

                fromBalanceInfo.Amount = fromBalanceAmount;
                fromBalanceInfo.UpdateDate = now;
                toBalanceInfo.Amount = toBalanceAmount;
                toBalanceInfo.UpdateDate = now;
            }
        }

        #endregion
    }
}