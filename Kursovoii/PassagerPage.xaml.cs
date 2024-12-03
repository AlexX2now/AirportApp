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

namespace Kursovoii
{
    /// <summary>
    /// Логика взаимодействия для PassagerPage.xaml
    /// </summary>
    public partial class PassagerPage : Window
    {
        KursoviiEntities db = new KursoviiEntities();
        public PassagerPage()
        {
            InitializeComponent();

            List<Рейс> allflight = db.Рейс.ToList();

            List<Flight> allrise = new List<Flight>();
            for (int i = 0; i < allflight.Count; i++)
            {
                allrise.Add(new Flight(allflight[i].Самолёты.Название_самолёта,
                    allflight[i].Дата_полёта.Value.ToShortDateString(), allflight[i].Город_отправки,
                    allflight[i].Город_прибытия, allflight[i].Статус_рейса.Наименование)); ;
            }
            flightlist.ItemsSource = allrise;
        }

        private void exitbtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void editreis_Click(object sender, RoutedEventArgs e)
        {
            //Запись на рейс
            CreateRec cr = new CreateRec();
            cr.Show();
        }

        public class Flight
        {
            public string PlaneName { get; set; }
            public string FlightDate { get; set; }
            public string CityFrom { get; set; }
            public string CityTo { get; set; }
            public string Status { get; set; }

            public Flight(string planeName, string flightDate, string cityFrom, string cityTo, string status)
            {
                PlaneName = planeName;
                FlightDate = flightDate;
                CityFrom = cityFrom;
                CityTo = cityTo;
                Status = status;
            }
        }
    }
}
