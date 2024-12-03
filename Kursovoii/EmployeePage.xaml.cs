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
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Window
    {
        private int userid;

        KursoviiEntities db = new KursoviiEntities();
        public EmployeePage(int userid)
        {
            InitializeComponent();
            this.userid = userid;

            var user = db.Пользователи.FirstOrDefault(x=>x.Код_пользователя == userid);

            username.Text = user.Фамилия + " " + user.Имя[0] + ".";


        }

        private void CheckPass_Click(object sender, RoutedEventArgs e)
        {
            checkPass.Visibility = Visibility.Visible;
            change.Visibility = Visibility.Hidden;
            createFlight.Visibility = Visibility.Hidden;
            checkFlight.Visibility = Visibility.Hidden;
            showzagr.Visibility = Visibility.Hidden;

            allpass.Items.Clear();

            List<Данные_о_пассажирах> allpassengers = db.Данные_о_пассажирах.ToList();

            for (int i = 0; i < allpassengers.Count; i++)
            {
                allpass.Items.Add(allpassengers[i].Фамилия + " " + allpassengers[i].Имя + " " + allpassengers[i].Отчество +
                    " " + allpassengers[i].Дата_рождения.Value.ToShortDateString() + ". Серия паспорта: " + allpassengers[i].Паспорт_пассажира.Серия + 
                    ", номер паспорта: " + allpassengers[i].Паспорт_пассажира.Номер);
            }
        }

        private void CheckInfo_Click(object sender, RoutedEventArgs e)
        {
            checkFlight.Visibility = Visibility.Visible;
            createFlight.Visibility = Visibility.Hidden;
            checkPass.Visibility = Visibility.Hidden;
            change.Visibility = Visibility.Hidden;
            showzagr.Visibility = Visibility.Hidden;

            allreis.Items.Clear();

            List<Рейс> allreises = db.Рейс.ToList();

            for (int i = 0; i < allreises.Count; i++)
            {
                allreis.Items.Add("Рейс из " + allreises[i].Город_отправки + " в " + allreises[i].Город_прибытия + 
                    ". Самолет: " + allreises[i].Самолёты.Название_самолёта
                    + ". Статус рейса: " + allreises[i].Статус_рейса.Наименование 
                    + ". Дата полёта: " + allreises[i].Дата_полёта.Value.ToShortDateString());
            }
        }

        private void CreateFlight_Click(object sender, RoutedEventArgs e)
        {
            createFlight.Visibility = Visibility.Visible;
            checkFlight.Visibility = Visibility.Hidden;
            checkPass.Visibility = Visibility.Hidden;

            change.Visibility = Visibility.Hidden;
            showzagr.Visibility = Visibility.Hidden;

            List<Самолёты> allplanes = db.Самолёты.ToList();

            for (int i = 0; i < allplanes.Count; i++)
            {
                flightcomobobox.Items.Add(allplanes[i].Название_самолёта);
            }
        }

        private void exitbtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
        private void changestat_Click(object sender, RoutedEventArgs e)
        {
            createFlight.Visibility = Visibility.Hidden;
            checkFlight.Visibility = Visibility.Hidden;
            checkPass.Visibility = Visibility.Hidden;

            change.Visibility = Visibility.Visible;
            showzagr.Visibility = Visibility.Hidden;

            neededfly.Items.Clear();
            newstatus.Items.Clear();
            statustext.Text = "";

            List<Рейс> allfly = db.Рейс.ToList();

            for (int i = 0; i < allfly.Count; i++)
            {
                neededfly.Items.Add(allfly[i].Город_отправки + " - " + allfly[i].Город_прибытия);
            }

            List<Статус_рейса> allstat = db.Статус_рейса.ToList();

            for (int i = 0; i < allstat.Count; i++)
            {
                newstatus.Items.Add(allstat[i].Наименование);
            }
        }

        private void savestat_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(statustext.Text) ||
                string.IsNullOrEmpty(newstatus.Text))
            {
                MessageBox.Show("Выберите рейс и новый статус");
            }
            else
            {
                if (statustext.Text == newstatus.Text)
                {
                    MessageBox.Show("Выбран один и тот же статус");
                }
                else
                {
                    if (statustext.Text == "Рейс отменён")
                    {
                        MessageBox.Show("Рейс отменён, его статус нельзя поменять");
                    }
                    else
                    {
                        var needeflytochange = db.Рейс.FirstOrDefault(x => x.Код_рейса == neededfly.SelectedIndex + 1);

                        needeflytochange.Код_статуса_рейса = newstatus.SelectedIndex + 1;

                        db.SaveChanges();

                        MessageBox.Show("Статус рейса успешно сохранен!");
                    }
                }
            }  
        }

        private void neededfly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (neededfly.SelectedIndex != -1)
            {
                var neededfl = db.Рейс.FirstOrDefault(x => x.Код_рейса == neededfly.SelectedIndex + 1);

                statustext.Text = neededfl.Статус_рейса.Наименование;
            }
        }
        private void createbtn_Click(object sender, RoutedEventArgs e)
        {
            //Создание рейса
            if (string.IsNullOrEmpty(flightcomobobox.Text) ||
                string.IsNullOrEmpty(dateflight.Text) ||
                string.IsNullOrEmpty(cityfrom.Text) ||
                string.IsNullOrEmpty(cityto.Text))
            {
                MessageBox.Show("Не оставляйте поля пустыми.");
            }
            else
            {
                if (cityfrom.Text == cityto.Text)
                {
                    MessageBox.Show("Нельзя лететь в один и тот же город");
                }
                else
                {
                    DateTime flightdate;
                    DateTime.TryParse(dateflight.Text, out flightdate);

                    var checkus = db.Рейс.FirstOrDefault(x=>x.Город_отправки == cityfrom.Text &&
                    x.Город_прибытия == cityto.Text && x.Дата_полёта == flightdate);

                    if (checkus != null)
                    {
                        MessageBox.Show("Такой рейс уже существует");
                    }
                    else
                    {
                        //Само создание
                        var selectedolane = db.Самолёты.FirstOrDefault(x => x.Название_самолёта == flightcomobobox.Text);

                        using (var context = new KursoviiEntities())
                        {
                            var reis = new Рейс
                            {
                                Дата_полёта = DateTime.Parse(dateflight.Text),
                                Код_самолёта = selectedolane.Код_самолёта,
                                Город_отправки = cityfrom.Text,
                                Город_прибытия = cityto.Text,
                                Код_статуса_рейса = 1,
                                Код_сотрудника = userid
                            };

                            context.Рейс.Add(reis);
                            context.SaveChanges();
                        }

                        MessageBox.Show("Рейс успешно создан!");
                    }                    
                }
            }
            
        }

        private void showzapbtn_Click(object sender, RoutedEventArgs e)
        {
            createFlight.Visibility = Visibility.Hidden;
            checkFlight.Visibility = Visibility.Hidden;
            checkPass.Visibility = Visibility.Hidden;

            change.Visibility = Visibility.Hidden;
            showzagr.Visibility = Visibility.Visible;

            allzapis.Items.Clear();

            List<Записи_на_рейс> alzap = db.Записи_на_рейс.ToList();

            for (int i = 0; i < alzap.Count; i++)
            {
                allzapis.Items.Add(alzap[i].Данные_о_пассажирах.Фамилия + " " + alzap[i].Данные_о_пассажирах.Имя[0] + ". " +
                    alzap[i].Данные_о_пассажирах.Отчество[0] + ". Рейс: " + alzap[i].Рейс.Город_отправки + " - " + alzap[i].Рейс.Город_прибытия + 
                    " Дата: " + alzap[i].Рейс.Дата_полёта.Value.ToShortDateString());
            }
        }
    }
}
