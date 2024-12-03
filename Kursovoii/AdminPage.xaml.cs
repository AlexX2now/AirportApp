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
using static System.Net.Mime.MediaTypeNames;

namespace Kursovoii
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        private int userid;

        KursoviiEntities db = new KursoviiEntities();
        public AdminPage(int userid)
        {
            InitializeComponent();
            this.userid = userid;

            var user = db.Пользователи.FirstOrDefault(x=>x.Код_пользователя == userid);

            username.Text = user.Фамилия + " " + user.Имя[0] + ". " + user.Отчество[0];
        }

        private void checkplanebtn_Click(object sender, RoutedEventArgs e)
        {
            //Посмотреть данные о самолетах
            closeallgrid();

            checkplanegrid.Visibility = Visibility.Visible;
            
            allplanes.Items.Clear();

            List<Самолёты> allpl = db.Самолёты.ToList();

            for (int i = 0; i < allpl.Count; i++)
            {
                allplanes.Items.Add(allpl[i].Название_самолёта + ". Вместительность людей: " + allpl[i].Вместительность_людей);
            }
        }

        private void addplanebtn_Click(object sender, RoutedEventArgs e)
        {
            //Добавить самолет
            closeallgrid();

            addplanegrid.Visibility = Visibility.Visible;
        }

        private void adduserbtn_Click(object sender, RoutedEventArgs e)
        {
            //Добавить пользователя
            closeallgrid();

            addusergrid.Visibility = Visibility.Visible;
        }

        private void checkuserbtn_Click(object sender, RoutedEventArgs e)
        {
            //посмотреть данные о самолетах
            closeallgrid();

            checkusergrid.Visibility = Visibility.Visible;

            allusers.Items.Clear();

            List<Пользователи> allus = db.Пользователи.ToList();

            for (int i = 0; i < allus.Count; i++)
            {
                allusers.Items.Add(allus[i].Фамилия + " " + allus[i].Имя + " " + allus[i].Отчество +
                    ". Дата рождения: " + allus[i].Дата_рождения.Value.ToShortDateString() +
                    ". Статус: " + allus[i].Статус_пользователя.Наименование + 
                    ". Номер телефона: " + allus[i].Номер_телефона +
                    ". Электронная почта: " + allus[i].Электронная_почта);
            }
        }

        private void checkhistorybtn_Click(object sender, RoutedEventArgs e)
        {
            //Просмотр истории посещения
            closeallgrid();

            checkhistorygrid.Visibility = Visibility.Visible;

            allhistory.Items.Clear();

            List<История_входа> allhis = db.История_входа.ToList();

            for (int i = 0; i < allhis.Count; i++)
            {
                allhistory.Items.Add(allhis[i].Пользователи.Фамилия + " " + allhis[i].Пользователи.Имя[0] + ". " +
                    allhis[i].Пользователи.Отчество[0] + " в " + allhis[i].Дата);
            }
        }

        private void closeallgrid()
        {
            checkplanegrid.Visibility = Visibility.Hidden;
            addplanegrid.Visibility = Visibility.Hidden;
            addusergrid.Visibility = Visibility.Hidden;
            checkusergrid.Visibility = Visibility.Hidden;
            checkhistorygrid.Visibility = Visibility.Hidden;
            change.Visibility = Visibility.Hidden;
        }

        private void addplane_Click(object sender, RoutedEventArgs e)
        {
            //Добавить самолет

            if (string.IsNullOrEmpty(nameofplane.Text) ||
                quantityppl.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните поля");
            }
            else
            {
                
                    using (var context = new KursoviiEntities())
                    {
                        var plane = new Самолёты
                        {
                            Название_самолёта = nameofplane.Text,
                            Вместительность_людей = Convert.ToInt16(quantityppl.Text)
                        };

                        context.Самолёты.Add(plane);
                        context.SaveChanges();
                    }
                    MessageBox.Show("Самолёт успешно добавлен!");             
            }
        }

        private void exitbtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        public bool checknum (string text)
        {
            bool result = true;

            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                    result = false;
            }

            return result;
        }

        private void createpol_Click(object sender, RoutedEventArgs e)
        {
            //Добавление пользователя
            if (string.IsNullOrEmpty(surname.Text) ||
                string.IsNullOrEmpty(name.Text) ||
                string.IsNullOrEmpty(trim.Text) ||
                string.IsNullOrEmpty(email.Text) ||
                string.IsNullOrEmpty(telephone.Text) ||
                string.IsNullOrEmpty(datebornpol.Text) ||
                string.IsNullOrEmpty(loginpas.Text) ||
                string.IsNullOrEmpty(passwordpol.Text))
            {
                MessageBox.Show("Не оставляйте поля пустыми");
            }
            else
            {
                if (!checknum(telephone.Text))
                {
                    MessageBox.Show("Введите правильный телефон");
                }
                else
                {
                    var checkus = db.Пользователи.FirstOrDefault(x=>x.Логин == loginpas.Text);

                    if (checkus != null)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует");
                    }
                    else
                    {
                        using (var context = new KursoviiEntities())
                        {
                            var newuser = new Пользователи
                            {
                                Фамилия = surname.Text,
                                Имя = name.Text,
                                Отчество = trim.Text,
                                Дата_рождения = datebornpol.SelectedDate.Value,
                                Номер_телефона = telephone.Text,
                                Электронная_почта = email.Text,
                                Код_статуса_пользователя = 1,
                                Логин = loginpas.Text,
                                Пароль = passwordpol.Text
                            };
                            context.Пользователи.Add(newuser);
                            context.SaveChanges();
                        }
                        MessageBox.Show("Пользователь успешно добавлен!");
                    }
                }
            }
        }

        private void changestat_Click(object sender, RoutedEventArgs e)
        {
            closeallgrid();

            change.Visibility = Visibility.Visible;

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
                var neededfl = db.Рейс.FirstOrDefault(x=>x.Код_рейса == neededfly.SelectedIndex + 1);

                statustext.Text = neededfl.Статус_рейса.Наименование;
            }
        }
    }
}
