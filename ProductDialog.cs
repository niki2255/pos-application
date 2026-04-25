using System;
using System.Globalization;
using System.Windows.Forms;

namespace SimplePOS
{
    public class ProductDialog : Form
    {
        public Product Result { get; private set; } = new Product();
        private readonly TextBox tBarcode = new();
        private readonly TextBox tName = new();
        private readonly TextBox tCost = new();
        private readonly TextBox tSale = new();
        private readonly TextBox tQty = new();

        public ProductDialog(string barcodeInit)
        {
            Text = "Dodaj Artikal";
            StartPosition = FormStartPosition.CenterParent;

            // Važno za Win11 / različite skale
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = SystemFonts.MessageBoxFont;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(10);

            // Glavni layout: 2 reda (forma + panel sa dugmadima)
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                AutoSize = true
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(root);

            // Forma (4 reda × 2 kolone): labela + textbox
            var form = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 6)
            };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); // leve labele fiksne širine
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));  // desni textbox zauzima ostatak

            var lbl1 = new Label { Text = "Barkod:", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0, 3, 6, 3) };
            var lbl2 = new Label { Text = "Naziv:", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0, 3, 6, 3) };
            var lbl3 = new Label { Text = "Nabavna cena:", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0, 3, 6, 3) };
            var lbl4 = new Label { Text = "Prodajna cena:", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0, 3, 6, 3) };
            var lbl5 = new Label { Text = "Kolicina:", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0, 3, 6, 3) };

            tBarcode.Dock = DockStyle.Fill; tBarcode.Text = barcodeInit ?? "";
            tName.Dock = DockStyle.Fill;
            tCost.Dock = DockStyle.Fill; tCost.Text = "";
            tSale.Dock = DockStyle.Fill; tSale.Text = "";
            tQty.Dock = DockStyle.Fill; tQty.Text = "1";

            form.Controls.Add(lbl1, 0, 0);
            form.Controls.Add(tBarcode, 1, 0);
            form.Controls.Add(lbl2, 0, 1);
            form.Controls.Add(tName, 1, 1);
            form.Controls.Add(lbl3, 0, 2);
            form.Controls.Add(tCost, 1, 2);
            form.Controls.Add(lbl4, 0, 3);
            form.Controls.Add(tSale, 1, 3);
            form.Controls.Add(lbl5, 0, 4);
            form.Controls.Add(tQty, 1, 4);

            // Donji panel sa dugmadima
            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true
            };
            var ok = new Button { Text = "OK", AutoSize = true };
            var cancel = new Button { Text = "Otkaži", AutoSize = true };

            ok.Click += Ok_Click;
            cancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            btnPanel.Controls.Add(ok);
            btnPanel.Controls.Add(cancel);

            // Dodaj u root
            root.Controls.Add(form, 0, 0);
            root.Controls.Add(btnPanel, 0, 1);

            // Enter i Esc
            AcceptButton = ok;
            CancelButton = cancel;

            // Minimalna širina da fieldovi budu čitljivi
            MinimumSize = new System.Drawing.Size(440, 0);
        }

        private void Ok_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tBarcode.Text) || string.IsNullOrWhiteSpace(tName.Text))
            {
                MessageBox.Show("Barkod i Naziv su obavezni.");
                return;
            }

            // Dozvoli decimalni zarez ili tačku
            var rs = new CultureInfo("sr-RS");
            if (!TryParseDecimal(tCost.Text, rs, out var cp) || !TryParseDecimal(tSale.Text, rs, out var sp) || !TryParseDecimal(tQty.Text, rs, out var qt))
            {
                MessageBox.Show("Neispravan unos cene.");
                return;
            }

            Result = new Product
            {
                Barcode = tBarcode.Text.Trim(),
                Name = tName.Text.Trim(),
                CostPrice = cp,
                SalePrice = sp,
                Quantity = qt
            };

            DialogResult = DialogResult.OK;
        }

        private static bool TryParseDecimal(string? input, CultureInfo culture, out decimal value)
        {
            // prihvati i tačku i zarez
            var normalized = (input ?? "").Replace(',', '.');
            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                   || decimal.TryParse(input ?? "", NumberStyles.Number, culture, out value);
        }
    }
}