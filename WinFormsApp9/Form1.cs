using System.Drawing.Text;
using System.Net.Http;
using System.IO;

namespace WinFormsApp9
{
    public partial class Form1 : Form
    {
        private PrivateFontCollection privateFontCollection;

        public Form1()
        {
            InitializeComponent();
            privateFontCollection = new PrivateFontCollection();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string fontUrl = "https://fonts-online.ru/fonts/painter-cyr";
            string fontFilePath = Path.Combine(Path.GetTempPath(), "painter-cyr");

            byte[] fontData;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    fontData = await client.GetByteArrayAsync(fontUrl);
                    File.WriteAllBytes(fontFilePath, fontData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ошибка при загрузке шрифта: {ex.Message}");
                    return;
                }
            }

            privateFontCollection.AddFontFile(fontFilePath);

            if (privateFontCollection.Families.Length == 0)
            {
                MessageBox.Show("не удалось загрузить шрифт");
                return;
            }

            FontFamily fontFamily = privateFontCollection.Families[0];
            Font newFont = new Font(fontFamily, 12);

            ApplyFont(this, newFont);

            try
            {
                File.Delete(fontFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ошибка при удалении временного файлла: {ex.Message}");
            }
        }

        private void ApplyFont(Control parent, Font font)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox || control is Button || control is Label || control is ComboBox || control is ListBox)
                {
                    control.Font = font;
                }

                if (control.HasChildren)
                {
                    ApplyFont(control, font);
                }
            }
        }
    }
}
