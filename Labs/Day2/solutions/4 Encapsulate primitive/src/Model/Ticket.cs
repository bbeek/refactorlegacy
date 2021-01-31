using System;

namespace Day2.Model
{
    public class Ticket
    {
        public double Cost { get; }

        public Ticket(double cost)
        {
            Cost = cost;
        }

        public Ticket ApplyReduction(int reduction)
        {
            return new Ticket(this.Cost * (1 - reduction / 100.0));
        }


        public Ticket RoundUp()
        {
            return new Ticket((int)Math.Ceiling(Cost));
        }

        public static Ticket Free()
        {
            return new Ticket(0);
        }
    }
}
