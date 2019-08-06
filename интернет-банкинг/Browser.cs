using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace интернет_банкинг
{
    public partial class Browser : Form
    {
        public Browser(Form form)
        {
            InitializeComponent();
            this.form = form;
        }
        Form form;

        private void Browser_Load(object sender, EventArgs e)
        {
            Left=70;
            Width = 1400;
            Height = 700;
        }

        private void Browser_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Activate();
        }
    }
}
