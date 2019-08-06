using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace интернет_банкинг
{
    public partial class training : MetroFramework.Forms.MetroForm
    {
        public training(Form form,int step,string id_client)
        {
            InitializeComponent();
            if (form is main)
                mainform = (main)form;
            else
                userform = (user)form;
            this.step = step;
            this.id_client = id_client;
        }
        main mainform;
        user userform;
        int step;
        int stepstart;
        string id_client;

        private void training_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mainform != null)
            {
                mainform.Enabled = true;
                mainform.textBox1.BackColor = Color.White;
                mainform.textBox2.BackColor = Color.White;
                mainform.button1.FlatAppearance.BorderColor = Color.Black;
                mainform.button3.FlatAppearance.BorderColor = Color.Black;
                mainform.button4.FlatAppearance.BorderColor = Color.Black;
            }
            if (userform != null)
            {
                userform.Enabled = true;
                userform.Activate();
                userform.panel8.BackColor = Color.Red;
                userform.button1.BackColor = Color.White;
                userform.label20.BackColor = Color.White;
                userform.label21.BackColor = Color.White;
                userform.button2.BackColor = Color.White;
                userform.button6.BackColor = Color.White;
                userform.button3.BackColor = Color.White;
                userform.label7.BackColor = Color.White;
                userform.label8.BackColor = Color.White;
                userform.button4.BackColor = Color.White;
                userform.label17.BackColor = Color.White;
                userform.label16.BackColor = Color.White;
                userform.panel1.BackColor = Color.Red;
                userform.button5.BackColor = Color.White;
                userform.dateTimePicker1.BackColor = Color.White;
                userform.dateTimePicker2.BackColor = Color.White;
                userform.button8.BackColor = Color.White;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
                step++;
                vivodtext(step);
                button1.Enabled = true;   
        }

        private void vivodtext(int step)
        {
            if (mainform != null)
            {
                mainform.textBox1.BackColor = Color.White;
                mainform.textBox2.BackColor = Color.White;
                mainform.button1.FlatAppearance.BorderColor = Color.Black;
                mainform.button3.FlatAppearance.BorderColor = Color.Black;
                mainform.button4.FlatAppearance.BorderColor = Color.Black;
            }
            if(userform != null)
            {
                userform.panel8.BackColor = Color.Red;
                userform.button1.BackColor = Color.White;
                userform.label20.BackColor = Color.White;
                userform.label21.BackColor = Color.White;
                userform.button2.BackColor = Color.White;
                userform.button6.BackColor = Color.White;
                userform.button3.BackColor = Color.White;
                userform.label7.BackColor = Color.White;
                userform.label8.BackColor = Color.White;
                userform.button4.BackColor = Color.White;
                userform.label17.BackColor = Color.White;
                userform.label16.BackColor = Color.White;
                userform.panel1.BackColor = Color.Red;
                userform.button5.BackColor = Color.White;
                userform.dateTimePicker1.BackColor = Color.White;
                userform.dateTimePicker2.BackColor = Color.White;
                userform.button8.BackColor = Color.White;
            }
            if (step==0)
                label2.Text = "Здравствуйте, приветствуем вас в приложении \"Симулятор \rинтернет-банкинга\". " +
                    "Вы находитесь на странице авторизации. \rЧтобы продолжить нажмите кнопку \"Далее\".";
            if (step == 1)
            {
                label2.Text = "Для входа на главную страницу приложения " +
                    "вам необходимо \rввести логин и пароль в соответствующие поля ввода и \rнажать кнопку \"Вход\"";
                timer1.Enabled = true;
                timer2.Enabled = true;
            }
            if (step == 2)
            {
                label2.Text = "Если вы еще не зарегистрированы вам необходимо пройти\rпроцедуру регистрации, для этого нажмите кнопку \r\"Регистрация\"";
                timer1.Enabled = true;
                timer2.Enabled = true;
            }
            if (step == 3)
            {
                label2.Text = "Для получения дополнительной информации нажмите кнопку\r\"Информация\"";
                timer1.Enabled = true;
                timer2.Enabled = true;
                button5.Visible = true;
                button2.Visible = false;
            }
            if (step == 4)
            {
                label2.Text = "Приветствуем вас на главной странице приложения.\rОбратите внимание на главное меню, с его помощью \rвы можете выбирать интересующие вас функции.";
                timer2.Enabled = true;
            }
            if (step == 5)
            {
                label2.Text = "На вкладке \"Карты\" вы можете производить различные операции \rсо своими банковскими картами. Для перехода на данную \rвкладку нажмите на кнопку \"Карты\".Чтобы подобрать \rнеобходимую карту нажмите на кнопку \"Подобрать карту\". \rДля того чтобы открыть новую карту нажмите на кнопку\r\"Новая карта+\". За более подробной информацией обращайтесь \rк руководству пользователя. Доступ к нему вы можете получить \rнажав на соответствующую кнопку в левой части экрана.";
                timer1.Enabled = true;
                timer2.Enabled = true;
            }
            if (step == 6)
            {
                label2.Text = "На вкладке \"Платежи\" вы можете провести оплату платежа. \rДля перехода на данную вкладку нажмите на кнопку \r\"Платежи\". За более подробной информацией обращайтесь к \rруководству пользователя. Доступ к нему вы можете получить \rнажав на соответствующую кнопку в левой части экрана";
                timer1.Enabled = true;
                timer2.Enabled = true;
                userform.button2.PerformClick();
            }
            if (step == 7)
            {
                label2.Text = "На вкладке \"Переводы\" вы можете произвести перевод средств \rна другую карту. Для перехода на данную вкладку нажмите \rна кнопку \"Переводы\". За более подробной информацией \rобращайтесь к руководству пользователя. Доступ к нему вы \rможете получить нажав на соответствующую кнопку в \rлевой части экрана.";
                timer1.Enabled = true;
                timer2.Enabled = true;
                userform.button6.PerformClick();
            }
            if (step == 8)
            {
                label2.Text = "На вкладке \"Депозиты\" вы можете производить различные \rоперации с открытыми ранее депозитами. Для перехода на \rданную вкладку нажмите на кнопку \"Депозиты\".Чтобы подобрать \rнеобходимый депозит нажмите на кнопку \"Подобрать депозит\". \rДля того чтобы открыть новый депозит нажмите на кнопку\r\"Открыть депозит\". За более подробной информацией \rобращайтесь к руководству пользователя. Доступ к нему вы \rможете получить rнажав на соответствующую кнопку в левой \rчасти экрана.";
                timer1.Enabled = true;
                timer2.Enabled = true;
                userform.button3.PerformClick();
            }
            if (step == 9)
            {
                label2.Text = "На вкладке \"Кредиты\" вы можете производить различные \rоперации с взятыми кредитами. Для перехода на \rданную вкладку нажмите на кнопку \"Кредиты\".Чтобы подобрать \rнеобходимый кредит нажмите на кнопку \"Подобрать кредит\". \rДля того чтобы оформить новый кредить нажмите на кнопку\r\"Оформить кредит\". За более подробной информацией \rобращайтесь к руководству пользователя. Доступ к нему вы \rможете получить нажав на соответствующую кнопку в левой \rчасти экрана.";
                timer1.Enabled = true;
                timer2.Enabled = true;
                userform.button4.PerformClick();
            }
            if (step == 10)
            {
                label2.Text = "На инфопанели \"Курсы валют\" вы можете просмотреть \rтекущие курсы валют НБ РБ.";
                timer1.Enabled = true;
                timer2.Enabled = true;
            }
            if (step == 11)
            {
                label2.Text = "Для просмотра истории операций нажмите на кнопку \r\"История платежей\". Вы также можете просматривать \rсовершенные платежи за период времени. Для этого установите \rначальную и конечную дату.";
                timer1.Enabled = true;
                timer2.Enabled = true;
                userform.button5.PerformClick();
            }
            if (step == 12)
            {
                label2.Text = "Если вы хотите изменить текущий пароль нажмите на \rкнопку \"Сменить пароль\".";
                timer1.Enabled = true;
                timer2.Enabled = true;
                button5.Visible = true;
                button2.Visible = false;
            }
        }

        private void training_Load(object sender, EventArgs e)
        {
            vivodtext(step);
            stepstart = step;
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (step > stepstart)
            {
                step--;
                vivodtext(step);
                button2.Enabled = true;
            }
            if (step == stepstart)
            {
                button1.Enabled = false;
            }
            if (button5.Visible == true)
            {
                button5.Visible = false;
                button2.Visible = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mainform != null)
            {
                if (Left > 100)
                {
                    Left -= 15;
                }
                else
                {
                    timer1.Enabled = false;
                }
            }
            else
            {
                if (step < 10)
                {
                    if (Left < 1000)
                    {
                        Left += 15;
                    }
                    else
                    {
                        timer1.Enabled = false;
                    }
                }
                else
                {
                    if (Left > 100)
                    {
                            Left -= 28;
                            Top = 320;
                    }
                    else
                    {
                        timer1.Enabled = false;
                    }
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (step == 1)
            {
                if (mainform.textBox1.BackColor == Color.Red)
                {
                    mainform.textBox1.BackColor = Color.White;
                    mainform.textBox2.BackColor = Color.White;
                    mainform.button1.FlatAppearance.BorderColor = Color.Black;
                }
                else
                {
                    mainform.textBox1.BackColor = Color.Red;
                    mainform.textBox2.BackColor = Color.Red;
                    mainform.button1.FlatAppearance.BorderColor = Color.Red;
                }
            }
            if (step == 2)
            {
                if (mainform.button4.FlatAppearance.BorderColor == Color.Red)
                {
                    mainform.button4.FlatAppearance.BorderColor = Color.Black;
                }
                else
                {
                    mainform.button4.FlatAppearance.BorderColor = Color.Red;
                }
            }
            if (step == 3)
            {
                if (mainform.button3.FlatAppearance.BorderColor == Color.Red)
                {
                    mainform.button3.FlatAppearance.BorderColor = Color.Black;
                }
                else
                {
                    mainform.button3.FlatAppearance.BorderColor = Color.Red;
                }
            }
            if (step == 4)
            {
                if (userform.panel8.BackColor == Color.Yellow)
                {
                    userform.panel8.BackColor = Color.Red;
                }
                else
                {
                    userform.panel8.BackColor = Color.Yellow;
                }
            }
            if (step == 5)
            {
                if (userform.label20.BackColor == Color.Yellow)
                {
                    userform.button1.BackColor = Color.White;
                    userform.label20.BackColor = Color.White;
                    userform.label21.BackColor = Color.White;
                }
                else
                {
                    userform.button1.BackColor = Color.Yellow;
                    userform.label20.BackColor = Color.Yellow;
                    userform.label21.BackColor = Color.Yellow;
                }
            }
            if (step == 6)
            {
                if (userform.button2.BackColor == Color.Yellow)
                {
                    userform.button2.BackColor = Color.White;
                }
                else
                {
                    userform.button2.BackColor = Color.Yellow;
                }
            }
            if (step == 7)
            {
                if (userform.button6.BackColor == Color.Yellow)
                {
                    userform.button6.BackColor = Color.White;
                }
                else
                {
                    userform.button6.BackColor = Color.Yellow;
                }
            }
            if (step == 8)
            {
                if (userform.label7.BackColor == Color.Yellow)
                {
                    userform.button3.BackColor = Color.White;
                    userform.label7.BackColor = Color.White;
                    userform.label8.BackColor = Color.White;
                }
                else
                {
                    userform.button3.BackColor = Color.Yellow;
                    userform.label7.BackColor = Color.Yellow;
                    userform.label8.BackColor = Color.Yellow;
                }
            }
            if (step == 9)
            {
                if (userform.label17.BackColor == Color.Yellow)
                {
                    userform.button4.BackColor = Color.White;
                    userform.label17.BackColor = Color.White;
                    userform.label16.BackColor = Color.White;
                }
                else
                {
                    userform.button4.BackColor = Color.Yellow;
                    userform.label17.BackColor = Color.Yellow;
                    userform.label16.BackColor = Color.Yellow;
                }
            }
            if (step == 10)
            {
                if (userform.panel1.BackColor == Color.Yellow)
                {
                    userform.panel1.BackColor = Color.Red;
                }
                else
                {
                    userform.panel1.BackColor = Color.Yellow;
                }
            }
            if (step == 11)
            {
                if (userform.button5.BackColor == Color.Yellow)
                {
                    userform.button5.BackColor = Color.White;
                    userform.dateTimePicker1.BackColor = Color.White;
                    userform.dateTimePicker2.BackColor = Color.White;
                }
                else
                {
                    userform.button5.BackColor = Color.Yellow;
                    userform.dateTimePicker1.BackColor = Color.Yellow;
                    userform.dateTimePicker2.BackColor = Color.Yellow;
                }
            }
            if (step == 12)
            {
                if (userform.button8.BackColor == Color.Yellow)
                {
                    userform.button8.BackColor = Color.White;
                }
                else
                {
                    userform.button8.BackColor = Color.Yellow;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
            if (step == 12)
            {
                try
                {
                    string sql = "UPDATE client SET training='YES' WHERE id_client="+id_client;
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);                     
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Browser form = new Browser(this);
            form.Show();
        }
    }
}
