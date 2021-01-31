using Day2.Model;
using System;
using System.Threading.Tasks;

namespace Day2.Repositories
{
    public interface IPriceRepository
    {
        Task<bool> IsHoliday(DateTime? skiDate);
        Task<Ticket> RetrieveBasePrice(string liftPassType);
    }
}