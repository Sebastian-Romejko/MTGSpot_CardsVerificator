using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MTGSpot_CardsVerificator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Card> MTGSpotCardList { get; set; }
        public static List<Order> orders { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MTGSpotCardList = new List<Card>();
        }

        private void WTB_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "xslx documents (.xlsx)|*.xlsx"; 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dlg.FileName;
            }
            else
            {
                return;
            }

            DataTable dane = GetData(filename);

            orders = new List<Order>();

            foreach(DataColumn column in dane.Columns)
            {
                if(column.Ordinal % 4 == 0 && orders.Where(x => x.Person == column.ColumnName).Count() == 0)
                {
                    orders.Add(new Order(column.ColumnName));
                }
            }

            for (int i = 0; i <= dane.Rows.Count - 1; i++)
            {
                foreach (Order order in orders)
                {
                    DataColumn column = dane.Columns[order.Person];

                    if (!string.IsNullOrEmpty(dane.Rows[i][column.Ordinal].ToString()))
                    {
                        Card card = new Card();
                        card.Name = dane.Rows[i][column.Ordinal].ToString();
                        card.Amount = Convert.ToInt32(dane.Rows[i][column.Ordinal + 1].ToString());
                        card.Price = Convert.ToDecimal(dane.Rows[i][column.Ordinal + 2].ToString());
                        card.FullPrice = Convert.ToDecimal(dane.Rows[i][column.Ordinal + 3].ToString());

                        order.Cards.Add(card);
                    }
                }
            }

            CardsWTB.ItemsSource = orders.Where(x => x.Person == "Seba").FirstOrDefault().Cards;
        }

        private void ListMTGSpot_Click(object sender, RoutedEventArgs e)
        {
            MTGSpotListWindow listWindow = new MTGSpotListWindow();
            listWindow.ShowDialog();

            CardsMTGSpot.ItemsSource = MTGSpotCardList;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        public DataTable GetData(String plik)
        {
            DataTable dt = null;
            try
            {
                using (var stream = new FileStream(plik, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        DataSet ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = false,
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        dt = ds.Tables["MTGSpot"];
                    }
                }
            }
            catch (Exception exc)
            {

            }

            return dt;
        }

        private void Braki_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            /*
            #region Zniknęło z MTGSpot
            stringBuilder.AppendLine("Zniknęło z MTGSpotu:");

            foreach (Order order in orders)
            {
                foreach (Card card in order.Cards)
                {
                    Card foundCard = MTGSpotCardList.Where(x => x.Name.Contains(card.Name)).FirstOrDefault();
                    
                    if (foundCard == null)
                    {
                        stringBuilder.AppendLine(card.ToString());
                    }
                    else if (foundCard.Amount != card.Amount)
                    {
                        stringBuilder.AppendLine(card.Name + " (" + foundCard.Amount + " / " + card.Amount + ")");
                    }
                    else if (foundCard.FullPrice != card.FullPrice)
                    {
                        stringBuilder.AppendLine(card.Name + " (" + foundCard.FullPrice + " / " + card.FullPrice + ")");
                    }
                }
            }
            #endregion
            */
            #region Nie ma na liście
            stringBuilder.AppendLine("Różnice:");

            var MTGSpotCardListBraki = new List<Card>(MTGSpotCardList);

            foreach (Order order in orders)
            {
                foreach (Card card in MTGSpotCardList)
                {
                    Card foundCard = order.Cards.Where(x => card.Name.Contains(x.Name)).FirstOrDefault();
                    if (foundCard != null)
                    {
                        MTGSpotCardListBraki.Where(x => x == card).FirstOrDefault().Amount -= foundCard.Amount;
                        MTGSpotCardListBraki.Where(x => x == card).FirstOrDefault().Price -= foundCard.Price;
                    }
                    else
                    {
                        //MTGSpotCardListBraki.Add(new Card(card));
                    }
                }

                /*foreach(Card card in order.Cards.Where(x => MTGSpotCardList.Where(y => y.Name.Contains(x.Name)).Count() == 0))
                {
                    Card newCard = new Card(card);
                    newCard.Amount -= card.Amount * 2;
                    MTGSpotCardListBraki.Add(newCard);
                }*/
            }

            foreach (Card card in MTGSpotCardList)
            {
                bool isCardFound = false;
                foreach (Order order in orders)
                {
                    if (order.Cards.Where(x => card.Name.Contains(x.Name)).Count() != 0)
                    {
                        continue;
                    }
                    else
                    {
                        isCardFound= true;
                    }
                }

                if(!isCardFound)
                {
                    Card newCard = new Card(card);
                    newCard.Amount -= card.Amount * 2;
                    MTGSpotCardListBraki.Add(newCard);
                }
            }

            foreach (Card card in MTGSpotCardListBraki.Where(x => x.Amount != 0 && x.Price != 0))
            {
                stringBuilder.AppendLine(card.ToString());
            }

            #endregion

            Braki.Clear();
            Braki.AppendText(stringBuilder.ToString());
        }
    }
}
