using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;


namespace интернет_банкинг
{
    public partial class phone : Form
    {
        public phone()
        {
            WindowsMediaPlayer WMP = new WindowsMediaPlayer();
            WMP.URL = @"D:\Колледж\4 КУРС\ПРАКТИКА\интернет-банкинг\images\1.mp3";
            WMP.controls.play();

            InitializeComponent();
            int width = Screen.PrimaryScreen.Bounds.Width;
            Location = new Point(width - this.Size.Width, 20);
        }

        private void phone_Load(object sender, EventArgs e)
        {
            System.Drawing.Text.PrivateFontCollection privateFonts1 = new System.Drawing.Text.PrivateFontCollection();
            privateFonts1.AddFontFile("D:\\Колледж\\4 КУРС\\ПРАКТИКА\\интернет-банкинг\\шрифт1\\sm SFNSDisplay-Thin.ttf");
            Font font1 = new Font(privateFonts1.Families[0], 37);
            label1.Font = font1;
            
            System.Drawing.Text.PrivateFontCollection privateFonts2 = new System.Drawing.Text.PrivateFontCollection();
            privateFonts2.AddFontFile("D:\\Колледж\\4 КУРС\\ПРАКТИКА\\интернет-банкинг\\шрифт1\\SF-UI\\SF-UI-Text-Semibold.otf");
            Font font2 = new Font(privateFonts2.Families[0], 10);
            label2.Font = font2;
            label3.Font = new Font(privateFonts2.Families[0], 8);
            this.Text = string.Empty;
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.BackColor = Color.AliceBlue;
            this.TransparencyKey = this.BackColor;
           
            label1.Text = DateTime.Now.ToString("HH:mm");
            label2.Text = DateTime.Now.DayOfWeek.ToString() + ", " + DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) +" "+ DateTime.Now.ToString("dd");
            label1.Parent = pictureBox1;
            label2.Parent = pictureBox1;
            label3.Parent = pictureBox1;
            //label3.ForeColor = ColorTranslator.FromHtml("#29514C");
            timer1.Enabled = true;
            timer1.Interval = 1000;
            ActiveControl = pictureBox1;
            pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            int width = pictureBox1.Width;
            Size size = new Size();
            size = pictureBox1.BackgroundImage.Size;
            label1.Width = size.Width-30;
            label1.Left = pictureBox1.Left+20;
            label2.Width = size.Width - 30;
            label2.Left = pictureBox1.Left + 20;
        }

        private void phone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm");
        }
    }
}
