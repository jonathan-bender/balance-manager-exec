using System.Collections.Generic;

namespace BalanceManager.DAL
{
    public interface IBalancesContext
    {
        IEnumerable<BalanceInfo> Balances { get; }
    }
}
