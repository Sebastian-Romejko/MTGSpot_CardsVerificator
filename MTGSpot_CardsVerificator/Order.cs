using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGSpot_CardsVerificator
{
    public class Order
    {
        public string Person { get; set; }
        public List<Card> Cards { get; set; }

        public Order(string person)
        {
            Person = person;
            Cards = new List<Card>();
        }
    }
}
