using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace reservation
{
    public partial class Form1 : Form
    {
        Button[] otaqlar;
        Label[] vaxtlar;

        int[] qalanVaxt = new int[6];
        bool[] rezervOlunub = new bool[6];
        Color[] evvelkiReng = new Color[6];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            otaqlar = new Button[]
            {
                button1,
                button2,
                button3,
                button4,
                button5,
                button6
            };

            vaxtlar = this.Controls.OfType<Label>()
                .Where(lbl => lbl.Text.Trim() == "0")
                .OrderBy(lbl => lbl.Top)
                .ThenBy(lbl => lbl.Left)
                .Take(6)
                .ToArray();

            for (int i = 0; i < 6; i++)
            {
                evvelkiReng[i] = otaqlar[i].BackColor;
                otaqlar[i].Click += Otaq_Click;
            }
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void Otaq_Click(object? sender, EventArgs e)
        {
            Button secilenOtaq = (Button)sender!;

            int otaqIndex = Array.IndexOf(otaqlar, secilenOtaq);

            if (otaqIndex == -1) return;

            if (rezervOlunub[otaqIndex])
            {
                MessageBox.Show("Bu otaq artıq rezerv olunub!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Ad və soyadı daxil edin!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            int muddet;
            if (!int.TryParse(textBox2.Text, out muddet) || muddet <= 0)
            {
                MessageBox.Show("Düzgün müddət daxil edin!", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            rezervOlunub[otaqIndex] = true;

            qalanVaxt[otaqIndex] = muddet * 60;

            otaqlar[otaqIndex].BackColor = Color.LightCoral;

            int deqiqe = qalanVaxt[otaqIndex] / 60;
            int saniye = qalanVaxt[otaqIndex] % 60;
            if (vaxtlar.Length > otaqIndex && vaxtlar[otaqIndex] != null)
            {
                vaxtlar[otaqIndex].Text = deqiqe.ToString("00") + ":" + saniye.ToString("00");
            }

            MessageBox.Show(
                "Otaq " + (otaqIndex + 1) + " uğurla rezerv edildi!",
                "Məlumat",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            textBox1.Clear();
            textBox2.Clear();
        }

        private void timer1_Tick(object? sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                if (rezervOlunub[i])
                {
                    qalanVaxt[i]--;

                    if (qalanVaxt[i] > 0)
                    {
                        int deqiqe = qalanVaxt[i] / 60;
                        int saniye = qalanVaxt[i] % 60;
                        if (vaxtlar.Length > i && vaxtlar[i] != null)
                        {
                            vaxtlar[i].Text = deqiqe.ToString("00") + ":" + saniye.ToString("00");
                        }
                    }
                    else
                    {
                        qalanVaxt[i] = 0;
                        rezervOlunub[i] = false;

                        otaqlar[i].BackColor = evvelkiReng[i];
                        if (vaxtlar.Length > i && vaxtlar[i] != null)
                        {
                            vaxtlar[i].Text = "0";
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void button5_Click(object sender, EventArgs e) { }
        private void button6_Click(object? sender, EventArgs e) { }
    }
}