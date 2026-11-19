using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyEventLogger
{
    internal sealed class MainForm : Form
    {
        private static readonly Color Ink = Color.FromArgb(26, 47, 51);
        private static readonly Color Paper = Color.FromArgb(248, 249, 245);
        private static readonly Color Teal = Color.FromArgb(12, 106, 91);
        private readonly ListBox events = new ListBox();
        private readonly Label status = new Label();
        private readonly Label count = new Label();
        private readonly Button start = new Button();
        private readonly Button stop = new Button();
        private readonly Button clear = new Button();
        private readonly Button save = new Button();
        private bool recording;

        public MainForm()
        {
            Text = "Tuş Günlüğü";
            AutoScaleMode = AutoScaleMode.Font;
            Font = new Font("Segoe UI", 10);
            ClientSize = new Size(680, 500);
            MinimumSize = new Size(620, 410);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Paper;
            ForeColor = Ink;
            KeyPreview = true;

            var header = new Panel { Dock = DockStyle.Top, Height = 104, BackColor = Ink };
            header.Controls.Add(Caption("Tuş Günlüğü", 24, 10, 25, Color.White, true));
            header.Controls.Add(Caption("Yalnızca bu pencerede basılan tuşlar kaydedilir.", 26, 65, 10, Color.Gainsboro));
            Controls.Add(header);

            var body = new TableLayoutPanel {
                Dock = DockStyle.Fill, Padding = new Padding(24, 15, 24, 18), ColumnCount = 1, RowCount = 4
            };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            body.RowStyles.Add(new RowStyle(SizeType.Absolute, 49));
            body.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            body.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            body.RowStyles.Add(new RowStyle(SizeType.Absolute, 31));
            Controls.Add(body);
            body.BringToFront();

            var tools = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
            AddButton(tools, start, "Başlat", Teal, Color.White);
            AddButton(tools, stop, "Durdur", Color.White, Ink);
            AddButton(tools, clear, "Temizle", Color.White, Ink);
            AddButton(tools, save, ".txt kaydet", Color.White, Ink);
            body.Controls.Add(tools, 0, 0);

            var strip = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            var title = Caption("Olay geçmişi", 0, 3, 13, Ink, true);
            strip.Controls.Add(title);
            status.Dock = DockStyle.Right;
            status.Width = 160;
            status.TextAlign = ContentAlignment.MiddleRight;
            strip.Controls.Add(status);
            body.Controls.Add(strip, 0, 1);

            events.Dock = DockStyle.Fill;
            events.IntegralHeight = false;
            events.Margin = Padding.Empty;
            events.BorderStyle = BorderStyle.FixedSingle;
            events.Font = new Font("Consolas", 12);
            events.ForeColor = Ink;
            events.AccessibleName = "Saat ve tuş adı";
            body.Controls.Add(events, 0, 2);
            count.Dock = DockStyle.Fill;
            count.Margin = Padding.Empty;
            count.TextAlign = ContentAlignment.MiddleLeft;
            body.Controls.Add(count, 0, 3);

            start.Click += (sender, args) => SetRecording(true);
            stop.Click += (sender, args) => SetRecording(false);
            clear.Click += (sender, args) => { events.Items.Clear(); UpdateCount(); };
            save.Click += (sender, args) => SaveEntries();
            Activated += (sender, args) => UpdateStatus(true);
            Deactivate += (sender, args) => UpdateStatus(false);
            SetRecording(false);
            UpdateCount();
        }

        private static Label Caption(string text, int x, int y, int size, Color color, bool bold = false)
        {
            return new Label {
                AutoSize = true, Location = new Point(x, y), Text = text, ForeColor = color,
                Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular)
            };
        }

        private static void AddButton(FlowLayoutPanel panel, Button button, string text, Color fill, Color color)
        {
            button.Text = text;
            button.Size = new Size(108, 35);
            button.Margin = new Padding(0, 0, 8, 0);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(194, 207, 201);
            button.BackColor = fill;
            button.ForeColor = color;
            panel.Controls.Add(button);
        }

        private void SetRecording(bool value)
        {
            recording = value;
            start.Enabled = !value;
            stop.Enabled = value;
            UpdateStatus(true);
            if (value) events.Focus();
        }

        private void UpdateStatus(bool active)
        {
            status.Text = !recording ? "Kayıt kapalı" : active ? "●  Kayıt açık" : "Odak bekleniyor";
            status.ForeColor = recording && active ? Teal : Ink;
        }

        private void UpdateCount()
        {
            count.Text = "Toplam olay   " + events.Items.Count;
            clear.Enabled = save.Enabled = events.Items.Count > 0;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (recording && ContainsFocus && Form.ActiveForm == this)
            {
                var entry = new KeyEntry(DateTime.Now, e.KeyCode);
                events.Items.Add(entry);
                events.TopIndex = events.Items.Count - 1;
                UpdateCount();
            }
            base.OnKeyDown(e);
        }

        private void SaveEntries()
        {
            using (var dialog = new SaveFileDialog {
                Filter = "Metin dosyası (*.txt)|*.txt", DefaultExt = "txt",
                FileName = "tus-gunlugu-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt"
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    File.WriteAllLines(dialog.FileName, events.Items.Cast<KeyEntry>().Select(e => e.ToFileLine()), Encoding.UTF8);
                    MessageBox.Show(this, "Günlük kaydedildi.", Text);
                }
                catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
                {
                    MessageBox.Show(this, error.Message, "Dosya kaydedilemedi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
