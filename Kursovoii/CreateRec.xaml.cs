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
    /// Логика взаимодействия для CreateRec.xaml
    /// </summary>
    public partial class CreateRec : Window
    {
        KursoviiEntities db = new KursoviiEntities();
        public CreateRec()
        {
            InitializeComponent();

            List<Рейс> vallflight = db.Рейс.ToList();

            List<Рейс> allflight = vallflight.Where(x=>x.Код_статуса_рейса == 1).ToList();

            for (int i = 0; i < allflight.Count; i++)
            {
                flightcombo.Items.Add(allflight[i].Город_отправки + " - " + allflight[i].Город_прибытия + " - " +
                    allflight[i].Дата_полёта.Value.ToShortDateString());
            }
        }

        private void createbtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(surname.Text) ||
                string.IsNullOrEmpty(name.Text) ||
                string.IsNullOrEmpty(trim.Text) ||
                string.IsNullOrEmpty(dateborn.Text) ||
                string.IsNullOrEmpty(numpas.Text) ||
                string.IsNullOrEmpty(serpas.Text) ||
                string.IsNullOrEmpty(flightcombo.Text))
            {
                MessageBox.Show("Не оставляйте поля пустыми");
            }
            else
            {
                if(!IsNumber(numpas.Text) ||
                   !IsNumber(serpas.Text) ||
                   numpas.Text.Length != 6 ||
                   serpas.Text.Length != 4)
                {
                    MessageBox.Show("Номер или серия паспорта написаны некорректно");
                }
                else
                {
                    //Поиск нужного рейса
                    string cities = flightcombo.Text;
                    string[] splitCities = cities.Split(new string[] { " - " }, StringSplitOptions.None);

                    string cityfrom = splitCities[0];
                    string cityto = splitCities[1];
                    string dateflight = splitCities[2];
                    DateTime dateval;

                    DateTime.TryParse(dateflight, out dateval);

                    var needsfly = db.Рейс.FirstOrDefault(x=>x.Город_отправки == cityfrom && x.Город_прибытия == cityto && x.Дата_полёта.Value == dateval);

                    //Данные для проверки, есть ли запись данного пассажира по рейсу и паспорту
                    var checkus = db.Записи_на_рейс.FirstOrDefault(x=>x.Данные_о_пассажирах.Паспорт_пассажира.Номер == numpas.Text 
                    && x.Данные_о_пассажирах.Паспорт_пассажира.Серия == serpas.Text && x.Код_рейса == needsfly.Код_рейса);

                    //Проверка, есть ли свободное место на рейсе
                    List<Записи_на_рейс> allflight = db.Записи_на_рейс.ToList();
                    List<Записи_на_рейс> neededfl = allflight.Where(x=>x.Код_рейса == needsfly.Код_рейса).ToList();

                    if (neededfl.Count >= needsfly.Самолёты.Вместительность_людей)
                    {
                        MessageBox.Show("Мест на рейсе нет");
                    }
                    else
                    {
                        if (checkus != null)
                        {
                            MessageBox.Show("Такой пассажир уже записан на данный рейс");
                        }
                        else
                        {
                            //Взятие номера последнего пасспорта
                            List<Паспорт_пассажира> allpass = db.Паспорт_пассажира.ToList();
                            int lastpass = allpass.Count + 5;

                            //Взятие Кода рейса и пользователя
                            List<Данные_о_пассажирах> allpasager = db.Данные_о_пассажирах.ToList();
                            int lastpassager = allpasager.Count + 5;

                            //Добавление пассажира
                            using (var context = new KursoviiEntities())
                            {
                                var Passport = new Паспорт_пассажира
                                {
                                    Номер = numpas.Text,
                                    Серия = serpas.Text
                                };
                                context.Паспорт_пассажира.Add(Passport);
                                context.SaveChanges();

                                var Passager = new Данные_о_пассажирах
                                {
                                    Фамилия = surname.Text,
                                    Имя = name.Text,
                                    Отчество = trim.Text,
                                    Дата_рождения = DateTime.Parse(dateborn.ToString()),
                                    Код_пасспорта = lastpass
                                };
                                context.Данные_о_пассажирах.Add(Passager);
                                //Еще нада добавление в таблицу Записи на рейс тут организовать как нить потом
                                context.SaveChanges();

                                var Zapis = new Записи_на_рейс
                                {
                                    Код_рейса = flightcombo.SelectedIndex + 1,
                                    Код_пассажира = lastpassager,
                                    Дата_записи = DateTime.Now
                                };
                                context.Записи_на_рейс.Add(Zapis);
                                context.SaveChanges();
                            }
                            //Получение номера рейса
                            var lastzap = db.Записи_на_рейс.OrderByDescending(x => x.Код_записи).FirstOrDefault();

                            MessageBox.Show("Вы успешно записались на рейс! Ваш номер: " + (lastzap.Код_записи) + "\n" +
                                " Запомните его!");
                        }
                    }       
                }
            }
        }

        public static bool IsNumber(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
