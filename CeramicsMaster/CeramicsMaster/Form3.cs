using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CeramicsMaster
{
    public partial class Form3 : Form
    {
        private int ch = 10;
        private System.Windows.Forms.Timer timer;

        public Form3()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ch--;
            if (ch <= 0)
            {
                timer.Stop();
                this.Close();
            }

            label2.Text = (ch / 60).ToString();
            label4.Text = (ch % 60).ToString();
        }
    }
}
