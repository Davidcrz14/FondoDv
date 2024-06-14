using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FondoA
{
    public partial class Form1 : Form
    {
        private string selectedImagePath = "";
        private string wallpaperImagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "wallpaper.jpg");
        private string fondosFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FondV", "Fondos");

        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            InitializeColorComboBox();
        }

        private void InitializeColorComboBox()
        {
            string[] imageFiles = Directory.GetFiles(fondosFolderPath, "*.jpg")
                                           .Concat(Directory.GetFiles(fondosFolderPath, "*.png"))
                                           .ToArray();

            // Agrega los nombres de los archivos de imagen al ComboBox
            foreach (string file in imageFiles)
            {
                comboBox1.Items.Add(Path.GetFileNameWithoutExtension(file));
            }

            // Asocia el evento de selección cambiada del ComboBox
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedImageName = comboBox1.SelectedItem.ToString();
            string jpgImagePath = Path.Combine(fondosFolderPath, selectedImageName + ".jpg");
            string pngImagePath = Path.Combine(fondosFolderPath, selectedImageName + ".png");

            if (File.Exists(jpgImagePath))
            {
                pictureBox1.ImageLocation = jpgImagePath;
            }
            else if (File.Exists(pngImagePath))
            {
                pictureBox1.ImageLocation = pngImagePath;
            }
            else
            {
                MessageBox.Show("No se pudo encontrar la imagen seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetWallpaper(string imagePath)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue("WallpaperStyle", 10.ToString()); // Establece el valor para "Rellenar"
            key.SetValue("TileWallpaper", 0.ToString());
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string imagePath = "";

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                imagePath = selectedImagePath;
            }
            else
            {
                string selectedImageName = comboBox1.SelectedItem?.ToString(); // Verifica si hay un elemento seleccionado
                if (!string.IsNullOrEmpty(selectedImageName))
                {
                    // Obtén la ruta completa de la imagen seleccionada
                    string jpgImagePath = Path.Combine(fondosFolderPath, selectedImageName + ".jpg");
                    string pngImagePath = Path.Combine(fondosFolderPath, selectedImageName + ".png");

                    // Verifica si existe la imagen con extensión .jpg
                    if (File.Exists(jpgImagePath))
                    {
                        imagePath = jpgImagePath;
                    }
                    // Si no existe, verifica si existe la imagen con extensión .png
                    else if (File.Exists(pngImagePath))
                    {
                        imagePath = pngImagePath;
                    }
                    else
                    {
                        // Si ninguna de las dos extensiones existe, muestra un mensaje de error
                        MessageBox.Show("No se pudo encontrar la imagen seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (!string.IsNullOrEmpty(imagePath))
            {
                SaveImage(imagePath);
                SetWallpaper(imagePath);
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ninguna imagen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveImage(string imagePath)
        {
            File.Copy(imagePath, wallpaperImagePath, true);
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const int SPI_SETDESKWALLPAPER = 0x0014;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(96, 96, 96); // Cambia el color de fondo al pasar el ratón por encima
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(64, 64, 64); // Restaura el color de fondo cuando el ratón sale del botón
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            try
            {
                WebClient client = new WebClient();
                byte[] imageData = client.DownloadData(url);
                MemoryStream stream = new MemoryStream(imageData);
                pictureBox1.Image = Image.FromStream(stream);

                // Guardar la imagen descargada como fondo de pantalla
                string tempImagePath = Path.Combine(Path.GetTempPath(), "tempWallpaper.jpg");
                pictureBox1.Image.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                SetWallpaper(tempImagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la imagen desde la URL proporcionada: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Crear una instancia del formulario que contiene las instrucciones
            Form2 instructionsForm = new Form2(); // Asegúrate de reemplazar "Form2" con el nombre de tu formulario de instrucciones

            // Mostrar el formulario
            instructionsForm.ShowDialog(); // Esto abrirá el formulario como un diálogo modal, lo que significa que el usuario deberá cerrarlo antes de poder interactuar con el formulario principal
        }

        
    }
}
