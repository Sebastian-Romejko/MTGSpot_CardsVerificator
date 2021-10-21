using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MTGSpot_CardsVerificator
{
    /// <summary>
    /// Interaction logic for MTGSpotListWindow.xaml
    /// </summary>
    public partial class MTGSpotListWindow : Window
    {
        public MTGSpotListWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string cardList = CardList.Text;

            MainWindow.MTGSpotCardList = MakeMTGSpotCardList(cardList);

            this.Close();
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public List<Card> MakeMTGSpotCardList(string cardList)
        {
            List<Card> MTGSpotCardList = new List<Card>();

            string[] lines = cardList.Split(Environment.NewLine.ToCharArray());

            for (int i = 30; i < lines.Length; i += 12)
            {
                if (lines[i] == "Łącznie:") 
                {
                    break;
                }

                Card card = new Card();
                card.Name = GetRidOfCardStates(lines[i]);
                card.Amount = Convert.ToInt32(lines[i + 8]);
                card.Price = Convert.ToDecimal(lines[i + 6].Trim().TrimEnd(" zł".ToCharArray()).Replace('.',','));
                card.FullPrice = Convert.ToDecimal(lines[i + 10].Trim().TrimEnd(" zł".ToCharArray()).Replace('.', ','));

                MTGSpotCardList.Add(card);
            }

            return MTGSpotCardList;
        }

        private string GetRidOfCardStates(string name)
        {
            name = name.Trim();
            if (name.StartsWith("NM"))
                name = name.Remove(0, 3);//name.TrimStart("NM ".ToCharArray());

            if (name.StartsWith("EX"))
                name = name.Remove(0, 3);//name.TrimStart("EX ".ToCharArray());

            if (name.StartsWith("GD"))
                name = name.Remove(0, 3);//name.TrimStart("GD ".ToCharArray());

            if (name.StartsWith("Foil"))
                name = name.Remove(0, 5);//name.TrimStart("FOIL ".ToCharArray());

            if (name.EndsWith(" ***"))
                name = name.TrimEnd(" ***".ToCharArray());

            if (name.EndsWith("(V.1)"))
                name = name.TrimEnd(" (V.1)".ToCharArray());

            if (name.EndsWith("(V.2)"))
                name = name.TrimEnd(" (V.2)".ToCharArray());

            return name.Trim();
        }
    }
}
 