using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGSpot_CardsVerificator
{
    public class Card
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public decimal FullPrice { get; set; }

        public Card()
        {

        }

        public Card(Card card)
        {
            Name = card.Name;
            Amount = card.Amount;
            Price = card.Price;
            FullPrice = card.FullPrice;
        }

        public override string ToString()
        {
            return Amount + " " + Name + " (" + Amount + " * " + Price + " = " + FullPrice + ")";
        }
    }
}
