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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kursovoii
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KursoviiEntities db = new KursoviiEntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void enterpass_Click(object sender, RoutedEventArgs e)
        {
            //Вход в окно пассажира
            PassagerPage pp = new PassagerPage();
            pp.Show();
            this.Close();
        }

        private void entermain_Click(object sender, RoutedEventArgs e)
        {
            //Вход в окно сотрудника, админа
            if (string.IsNullOrEmpty(loginin.Text) ||
                string.IsNullOrEmpty(passin.Text))
            {
                MessageBox.Show("Не оставляйте поля пустыми");
            }
            else
            {
                //Тут проверка есть ли такой человек в бд и последующий вход в систему
                var user = db.Пользователи.FirstOrDefault(x=>x.Логин == loginin.Text && x.Пароль == passin.Text);

                if (user == null)
                {
                    MessageBox.Show("Вы неправильно ввели логин или пароль");
                }
                else
                {
                    //Вход в систему
                    using (var context = new KursoviiEntities())
                    {
                        var history = new История_входа
                        {
                            Код_пользователя = user.Код_пользователя,
                            Дата = DateTime.Now
                        };

                        context.История_входа.Add(history);
                        context.SaveChanges();
                    }
                        switch (user.Код_статуса_пользователя)
                        {
                            //Сотрудник
                            case 1:
                                EmployeePage ep = new EmployeePage(user.Код_пользователя);
                                ep.Show();
                                this.Close();
                                break;
                            //Администратор 
                            case 2:
                                AdminPage ap = new AdminPage(user.Код_пользователя);
                                ap.Show();
                                this.Close();
                                break;
                        } 
                }
            }
        }
    }
}
