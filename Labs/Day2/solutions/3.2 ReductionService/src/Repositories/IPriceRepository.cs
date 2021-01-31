using System;
using System.Threading.Tasks;

namespace Day2.Repositories
{
    public interface IPriceRepository
    {
        Task<bool> IsHoliday(DateTime? skiDate);
        Task<double> RetrieveBasePrice(string liftPassType);
    }
}