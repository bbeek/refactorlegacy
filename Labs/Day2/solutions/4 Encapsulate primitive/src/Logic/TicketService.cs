using Day2.Model;
using Day2.Repositories;
using System;
using System.Threading.Tasks;

namespace Day2.Logic
{
    public class TicketService
    {
        private readonly IPriceRepository repository;
        private readonly ReductionService reductionService;

        public TicketService(IPriceRepository repository, ReductionService reductionService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.reductionService = reductionService ?? throw new ArgumentNullException(nameof(reductionService));
        }

        public async Task<Ticket> CalculatePrice(int? age, string liftPassType, DateTime? skiDate)
        {
            if (age! < 6)
            {
                return Ticket.Free();
            }

            Ticket response;

            var basePrice = await repository.RetrieveBasePrice(liftPassType);

            if ("night".Equals(liftPassType))
            {
                if (age == null)
                {
                    response = Ticket.Free();
                }
                else if (age > 64)
                {
                    response = basePrice
                        .ApplyReduction(60);
                }
                else
                {
                    response = basePrice;
                }
            }
            else
            {
                int reduction = await reductionService.FindPotentialReduction(age, skiDate);

                if (age! < 15)
                {
                    response = basePrice
                        .ApplyReduction(30);
                }
                else if (age! > 64)
                {
                    response = basePrice
                                .ApplyReduction(25)
                                .ApplyReduction(reduction);
                }
                else
                {
                    response = basePrice
                            .ApplyReduction(reduction);
                }
            }

            return response;
        }
    }
}
