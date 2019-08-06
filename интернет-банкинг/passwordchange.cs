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
    public partial class passwordchange : MetroFramework.Forms.MetroForm
    {
        public passwordchange(string id_client)
        {
            InitializeComponent();
            this.id_client = id_client;
        }
        string id_client;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите старый пароль!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Введите новый пароль!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    try
                    {
                        string sql = "SELECT password FROM client WHERE id_client=" + id_client;
                        MySqlConnection conn1 = DBUtils.GetDBConnection();
                        conn1.Open();
                        MySqlCommand command1 = new MySqlCommand(sql, conn1);
                        MySqlDataReader reader1 = command1.ExecuteReader();
                        reader1.Read();
                        string password = reader1[0].ToString();
                        reader1.Close();
                        conn1.Close();

                        if (password == textBox1.Text)
                        {
                            sql = "UPDATE client SET password='" + textBox2.Text + "' WHERE id_client=" + id_client;
                            conn1 = DBUtils.GetDBConnection();
                            conn1.Open();
                            command1 = new MySqlCommand(sql, conn1);
                            reader1 = command1.ExecuteReader();
                            reader1.Read();
                            reader1.Close();
                            conn1.Close();
                            MessageBox.Show("Пароль успешно изменен!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Вы ввели неверный пароль!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBox1.Text = "";
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString(), exc.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
