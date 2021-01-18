namespace TheatricalPlays
{
    class TragedyCalculator : PerformanceCalculator
    {
        public override int Amount
        {
            get
            {
                var result = 40000;
                if (Performance.Audience > 30)
                {
                    result += 1000 * (Performance.Audience - 30);
                }

                return result;
            }
        }
    }
}
