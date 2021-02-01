using System;

namespace ParameterizeMethod
{
    public class MarketingCampaign
    {
        public bool HasCrazySaleActive()
        {
            var activeCampaigns = new MarketingRepository().GetMarketingCampaigns();
            
            return activeCampaigns.Contains("CrazySale");
        }

    }
}
