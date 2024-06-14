using System;
using System.Windows.Forms;

namespace FondoA
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // URL a la que se dirigirá al hacer clic en el botón
            string url = "https://wallhaven.cc/";

            // Abrir la URL en el navegador predeterminado del sistema
            System.Diagnostics.Process.Start(url);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
