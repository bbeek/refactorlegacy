using System;

namespace ReplaceGlobalReference
{
    class Program
    {
        static void Main(string[] args)
        {
            var campaign = new MarketingCampaign();

            Console.WriteLine($"IsCrazySaleDay {campaign.IsCrazySalesDay()}");
        }


    }
}
