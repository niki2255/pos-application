using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing.Printing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection.Metadata;







namespace SimplePOS
{

    public partial class MainForm : Form
    {
        private void LoadCustomers()
        {
            comboBox1.Items.Clear();

            using (var con = Db.Open())
            {


                string query = "SELECT id, name FROM customers ORDER BY name";

                using (var cmd = new SQLiteCommand(query, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(new CustomerItem
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"].ToString()
                        });
                    }
                }
            }
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        public class CustomerItem
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
        public static class InvoicePdf
        {
            // Pozovi: InvoicePdf.Generate(saleId, cart, "PANAMA OUTFIT d.o.o.", "Aleksandra Dubceka 38h, Beograd", "PIB/MB (opciono)", buyerName: null, buyerAddr: null);
            public static string Generate(
                int saleId,
                BindingList<CartItem> cart,
                string sellerName = "PADRINO FASHION ROOM",
                string sellerAddress = "",
                string sellerExtra = "",
                string? buyerName = null,
                decimal discountPercent = 0m,
                string? buyerAddress = null)

            {
                var culture = new CultureInfo("sr-RS");

                // clamp 0-100
                if (discountPercent < 0m) discountPercent = 0m;
                if (discountPercent > 100m) discountPercent = 100m;

                decimal subtotal = 0m;
                foreach (var it in cart) subtotal += it.Total;

                decimal factor = 1m - (discountPercent / 100m);
                decimal discountAmount = subtotal * (discountPercent / 100m);
                decimal total = subtotal * factor; // it.Total = it.Price * it.Qty

                var fileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
                Directory.CreateDirectory(fileDir);
                var filePath = Path.Combine(fileDir, $"Racun_{saleId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

                // QuestPDF dokument
                QuestPDF.Settings.License = LicenseType.Community;
                QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Size(PageSizes.A4);
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.Header().Element(HeaderSection);
                        page.Content().Element(ContentSection);
                        page.Footer().AlignCenter().Text(txt =>
                        {
                            txt.Span("Hvala na kupovini! ").SemiBold();
                            txt.Span($"Račun br. {saleId} • {DateTime.Now:dd.MM.yyyy HH:mm}");
                        });

                        // Header
                        void HeaderSection(QuestPDF.Infrastructure.IContainer c)
                        {
                            c.Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text(sellerName).FontSize(18).SemiBold();
                                    if (!string.IsNullOrWhiteSpace(sellerAddress))
                                        col.Item().Text(sellerAddress);
                                    if (!string.IsNullOrWhiteSpace(sellerExtra))
                                        col.Item().Text(sellerExtra);
                                });

                                row.ConstantItem(200).Border(1).Padding(5).Column(col =>
                                {
                                    col.Item().AlignRight().Text($"RAČUN #{saleId}").FontSize(16).SemiBold();
                                    col.Item().AlignRight().Text($"{DateTime.Now:dd.MM.yyyy HH:mm}");
                                    if (!string.IsNullOrWhiteSpace(buyerName))
                                    {
                                        col.Item().PaddingTop(10).AlignRight().Text("Kupac:").SemiBold();
                                        col.Item().AlignRight().Text(buyerName);
                                        if (!string.IsNullOrWhiteSpace(buyerAddress))
                                            col.Item().AlignRight().Text(buyerAddress);
                                    }
                                });
                            });
                        }

                        // Content (tabela stavki + total)
                        void ContentSection(QuestPDF.Infrastructure.IContainer c)
                        {
                            c.PaddingVertical(10).Column(col =>
                            {
                                col.Item().LineHorizontal(1);

                                col.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(6); // Artikal
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(2); // Količina
                                        columns.RelativeColumn(2); // Cena
                                        columns.RelativeColumn(2); // Ukupno
                                    });

                                    // header
                                    table.Header(header =>
                                    {
                                        header.Cell().Padding(4).Text("Artikal").SemiBold();
                                        header.Cell().Padding(4).Text("Šifra").SemiBold();
                                        header.Cell().Padding(4).Text("Količina").SemiBold();
                                        header.Cell().Padding(4).AlignRight().Text("Cena").SemiBold();
                                        header.Cell().Padding(4).AlignRight().Text("Ukupno").SemiBold();
                                        header.Cell().ColumnSpan(5).PaddingTop(4).Element(e => e.LineHorizontal(1));
                                    });

                                    // rows
                                    foreach (var it in cart)
                                    {
                                        table.Cell().Padding(4).Text(it.Name);
                                        table.Cell().Padding(4).Text(it.Barcode);
                                        table.Cell().Padding(4).Text(it.Qty.ToString(culture));
                                        table.Cell().Padding(4).AlignRight().Text($"{it.Price.ToString("N2", culture)} EUR");
                                        table.Cell().Padding(4).AlignRight().Text($"{it.Total.ToString("N2", culture)} EUR");
                                    }
                                });

                                col.Item().PaddingTop(6).LineHorizontal(1);

                                col.Item().Row(row =>
                                {
                                    row.RelativeItem();
                                    row.ConstantItem(220).Column(rcol =>
                                    {
                                        rcol.Item().Row(r =>
                                        {
                                            r.RelativeItem().Text("ZBIR:").SemiBold();
                                            r.ConstantItem(120).AlignRight().Text($"{subtotal.ToString("N2", culture)} EUR").SemiBold();
                                        });

                                        // Popust (samo ako postoji)
                                        if (discountPercent > 0m)
                                        {
                                            rcol.Item().Row(r =>
                                            {
                                                r.RelativeItem().Text($"POPUST ({discountPercent.ToString("0.##", culture)}%):").SemiBold();
                                                r.ConstantItem(120).AlignRight().Text($"-{discountAmount.ToString("N2", culture)} EUR").SemiBold();
                                            });
                                        }

                                        // Total posle popusta
                                        rcol.Item().Row(r =>
                                        {
                                            r.RelativeItem().Text("UKUPNO:").SemiBold();
                                            r.ConstantItem(120).AlignRight().Text($"{total.ToString("N2", culture)} EUR").SemiBold();
                                        });
                                    });
                                });
                            });
                        }
                    });
                }).GeneratePdf(filePath);

                return filePath;
            }

            // Opcionalno: otvori PDF odmah u default viewer-u
            public static void Open(string filePath)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                catch { /* ignore */ }
            }
        }

        private int SaveOrGetCustomer(string name, string note, SQLiteConnection con)
        {



            string checkQuery = "SELECT id FROM customers WHERE name = @name";
            using (var checkCmd = new SQLiteCommand(checkQuery, con))
            {
                checkCmd.Parameters.AddWithValue("@name", name);
                var existingId = checkCmd.ExecuteScalar();

                if (existingId != null && existingId != DBNull.Value)
                {
                    string updateQuery = "UPDATE customers SET note = @note WHERE id = @id";
                    using (var updateCmd = new SQLiteCommand(updateQuery, con))
                    {
                        updateCmd.Parameters.AddWithValue("@note", note);
                        updateCmd.Parameters.AddWithValue("@id", Convert.ToInt32(existingId));
                        updateCmd.ExecuteNonQuery();
                    }

                    return Convert.ToInt32(existingId);
                }
            }

            string insertQuery = "INSERT INTO customers (name, note) VALUES (@name, @note); SELECT last_insert_rowid();";
            using (var insertCmd = new SQLiteCommand(insertQuery, con))
            {
                insertCmd.Parameters.AddWithValue("@name", name);
                insertCmd.Parameters.AddWithValue("@note", note);

                return Convert.ToInt32((long)insertCmd.ExecuteScalar());
            }

        }
        // Primer tvoje klase stavke (uskladi sa realnim modelom)
        private void ApplyPermissions()
        {
            tabStats.Parent = isAdmin ? tabs : null;
            tabCustomers.Parent = isAdmin ? tabs : null;
            btnInvAdd.Enabled = isAdmin ? true : false;
            btnInvSave.Enabled = isAdmin ? true : false;
            btnInvDelete.Enabled = isAdmin ? true : false;
            gridInv.ReadOnly = !isAdmin;

        }

        BindingList<CartItem> cart = new();
        PrintDocument printDoc = new();

        private readonly CultureInfo _rs = new CultureInfo("sr-RS");
        private string _printText; // koristi se kad ručno štampaš sačuvan račun
        private bool isAdmin;
        public MainForm(bool adminMode)
        {
            InitializeComponent();
            isAdmin = adminMode;
            ApplyPermissions();
            gridCart.AutoGenerateColumns = true;
            gridCart.DataSource = cart;
            textBox2.Text = "0";
            gridCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (gridCart.Columns.Contains("CostPrice"))
                gridCart.Columns["CostPrice"].Visible = false;

            if (gridCart.Columns.Contains("Profit"))
                gridCart.Columns["Profit"].Visible = false;

            if (gridCart.Columns.Contains("Total"))
                gridCart.Columns["Total"].Visible = false;

            //  if (gridCart.Columns.Contains("Qty")) // ako je duplirana ili nepotrebna
            //     gridCart.Columns["Qty"].Visible = false;

            var col = gridCart.Columns["barcode"]; if (col != null) col.HeaderText = "Barkod";
            col = gridCart.Columns["name"]; if (col != null) col.HeaderText = "Naziv";
            col = gridCart.Columns["ProductId"]; if (col != null) col.HeaderText = "ID";
            col = gridCart.Columns["price"]; if (col != null) col.HeaderText = "Cena";
            col = gridCart.Columns["Qty"]; if (col != null) col.HeaderText = "Kolicina";

            printDoc.PrintPage += PrintDoc_PrintPage;
            LoadInventory();
            LoadCustomers();
            LoadCustomersGrid();
            InitStatsUi();
            RefreshStats();
        }

        private void LoadInventory()
        {
            using var c = Db.Open();
            var dt = new DataTable();

            // normalizuj unos (skener često doda CR/LF)
            string q = (textBox1.Text ?? string.Empty)
                       .Replace("\r", "").Replace("\n", "")
                       .Trim();

            if (!string.IsNullOrEmpty(q))
            {
                // tačan pogodak po barkodu (TEXT kolona → parametar!)
                using (var da = new SQLiteDataAdapter(
                    "SELECT id, barcode, name, cost_price, sale_price, product_quantity " +
                    "FROM products " +
                    "WHERE barcode = @barcode " +
                    "ORDER BY name;", c))
                {
                    da.SelectCommand.Parameters.AddWithValue("@barcode", q);
                    da.Fill(dt);
                }

                // ako nema tačnog pogodka, probaj delimično (LIKE)
                if (dt.Rows.Count == 0)
                {
                    using var da2 = new SQLiteDataAdapter(
                        "SELECT id, barcode, name, cost_price, sale_price , product_quantity " +
                        "FROM products " +
                        "WHERE barcode LIKE @like OR name LIKE @like " +
                        "ORDER BY name;", c);
                    da2.SelectCommand.Parameters.AddWithValue("@like", $"%{q}%");
                    da2.Fill(dt);
                }
            }
            else
            {
                using var da = new SQLiteDataAdapter(
                    "SELECT id, barcode, name, cost_price, sale_price , product_quantity " +
                    "FROM products " +
                    "ORDER BY id;", c);
                da.Fill(dt);
            }

            gridInv.DataSource = dt;
            gridInv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (gridInv.Columns.Contains("cost_price"))
                gridInv.Columns["cost_price"].Visible = isAdmin ? true : false;

            var col = gridInv.Columns["barcode"]; if (col != null) col.HeaderText = "Barkod";
            col = gridInv.Columns["name"]; if (col != null) col.HeaderText = "Naziv";
            col = gridInv.Columns["cost_price"]; if (col != null) col.HeaderText = "Nabavna cena";
            col = gridInv.Columns["sale_price"]; if (col != null) col.HeaderText = "Prodajna cena";
            col = gridInv.Columns["product_quantity"]; if (col != null) col.HeaderText = "Kolicina";
        }

        private void btnInvAdd_Click(object sender, EventArgs e)
        {
            using var dlg = new ProductDialog("");
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var p = dlg.Result;
                using var c = Db.Open();
                using var cmd = new SQLiteCommand("INSERT INTO products(barcode,name,cost_price,sale_price, product_quantity) VALUES(@b,@n,@cp,@sp,@pq)", c);
                cmd.Parameters.AddWithValue("@b", p.Barcode);
                cmd.Parameters.AddWithValue("@n", p.Name);
                cmd.Parameters.AddWithValue("@cp", p.CostPrice);
                cmd.Parameters.AddWithValue("@sp", p.SalePrice);
                cmd.Parameters.AddWithValue("@pq", p.Quantity);
                try { cmd.ExecuteNonQuery(); } catch (Exception ex) { MessageBox.Show("Greška: " + ex.Message); }
                LoadInventory();
            }
        }

        private void btnInvSave_Click(object sender, EventArgs e)
        {
            gridInv.EndEdit();
            if (gridInv.DataSource is DataTable dt)
            {
                using var c = Db.Open();
                foreach (DataRow row in dt.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        using var cmd = new SQLiteCommand("UPDATE products SET barcode=@b,name=@n,cost_price=@cp,sale_price=@sp, product_quantity=@pq WHERE id=@id", c);
                        cmd.Parameters.AddWithValue("@b", row["barcode"]);
                        cmd.Parameters.AddWithValue("@n", row["name"]);
                        cmd.Parameters.AddWithValue("@cp", row["cost_price"]);
                        cmd.Parameters.AddWithValue("@sp", row["sale_price"]);
                        cmd.Parameters.AddWithValue("@pq", row["product_quantity"]);
                        cmd.Parameters.AddWithValue("@id", row["id"]);
                        cmd.ExecuteNonQuery();
                    }
                }
                dt.AcceptChanges();
                LoadInventory();
            }
        }

        private void btnInvDelete_Click(object sender, EventArgs e)
        {
            if (gridInv.CurrentRow == null) return;
            var id = Convert.ToInt64(((DataRowView)gridInv.CurrentRow.DataBoundItem)["id"]);
            if (MessageBox.Show("Obrisati iz inventara?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using var c = Db.Open();
                using var cmd = new SQLiteCommand("DELETE FROM products WHERE id=@id", c);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                LoadInventory();
            }
        }

        private void txtScan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var code = txtScan.Text.Trim();
                txtScan.Clear();
                if (!string.IsNullOrEmpty(code)) AddByBarcode(code);
                e.SuppressKeyPress = true;
            }
        }

        private void AddByBarcode(string barcode)
        {
            using var c = Db.Open();
            using var cmd = new SQLiteCommand("SELECT id, barcode, name, cost_price, sale_price FROM products WHERE barcode=@b", c);
            cmd.Parameters.AddWithValue("@b", barcode);
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                var p = new CartItem
                {
                    ProductId = (long)r["id"],
                    Barcode = (string)r["barcode"],
                    Name = (string)r["name"],
                    CostPrice = Convert.ToDecimal(r["cost_price"]),
                    Price = Convert.ToDecimal(r["sale_price"]),
                    Qty = 1
                };
                var ex = cart.FirstOrDefault(x => x.ProductId == p.ProductId);
                if (ex != null) { ex.Qty += 1; }
                else cart.Add(p);
                UpdateTotals();
            }
            else
            {
                using var dlg = new ProductDialog(barcode);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var np = dlg.Result;
                    using var ins = new SQLiteCommand("INSERT INTO products(barcode,name,cost_price,sale_price) VALUES(@b,@n,@cp,@sp)", c);
                    ins.Parameters.AddWithValue("@b", np.Barcode);
                    ins.Parameters.AddWithValue("@n", np.Name);
                    ins.Parameters.AddWithValue("@cp", np.CostPrice);
                    ins.Parameters.AddWithValue("@sp", np.SalePrice);
                    try { ins.ExecuteNonQuery(); } catch (Exception ex2) { MessageBox.Show("Greška: " + ex2.Message); }
                }

                LoadInventory();
            }
        }

        private void UpdateTotals()
        {
            decimal subtotal = cart.Sum(x => x.Total);
            decimal discountPct = GetDiscountPercent();
            decimal factor = GetDiscountFactor();

            decimal totalAfterDiscount = subtotal * factor;

            if (discountPct > 0m)
                lblTotal.Text = $"Ukupno: {totalAfterDiscount:N2} EUR  (Popust {discountPct:N0}%)";
            else
                lblTotal.Text = $"Ukupno: {subtotal:N2} EUR";
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            cart.Clear(); UpdateTotals();
        }

        private void LoadCustomersGrid()
        {
            using (var con = Db.Open())
            {


                string query = "SELECT id, name, note FROM customers ORDER BY name";

                using (var da = new SQLiteDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            // privremeno zaustavi checkout dok ne vidimo brojke

            if (cart.Count == 0) { MessageBox.Show("Korpa je prazna."); return; }

            using var c = Db.Open();
            using var tx = c.BeginTransaction();

            decimal subtotal = cart.Sum(x => x.Total);
            decimal factor = GetDiscountFactor();

            decimal total = subtotal * factor;

            // profit po stavci: (prodajna_posle_popusta - nabavna) * qty
            decimal profit = cart.Sum(x => ((x.Price * factor) - x.CostPrice) * x.Qty);

            // decimal total = cart.Sum(x => x.Total);
            // decimal profit = cart.Sum(x => (x.Price  - x.CostPrice) * x.Qty);

            long saleId;


            string customerName = comboBox1.Text.Trim();
            string customerNote = textBox4.Text.Trim();

            int customerId = SaveOrGetCustomer(customerName, customerNote, c);

            if (comboBox1.SelectedItem is CustomerItem selectedCustomer)
            {
                customerId = selectedCustomer.Id;
            }



            if (string.IsNullOrWhiteSpace(customerName))
            {
                MessageBox.Show("Unesi ime kupca.");
                return;
            }




            string checkQuery = "SELECT id FROM customers WHERE name = @name";
            using (var checkCmd = new SQLiteCommand(checkQuery, c))
            {
                checkCmd.Parameters.AddWithValue("@name", customerName);
                var existingId = checkCmd.ExecuteScalar();

                if (existingId != null)
                {
                    string updateQuery = "UPDATE customers SET note = @note WHERE id = @id";
                    using (var updateCmd = new SQLiteCommand(updateQuery, c))
                    {
                        updateCmd.Parameters.AddWithValue("@note", customerNote);
                        updateCmd.Parameters.AddWithValue("@id", Convert.ToInt32(existingId));
                        updateCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string insertQuery = "INSERT INTO customers (name, note) VALUES (@name, @note)";
                    using (var insertCmd = new SQLiteCommand(insertQuery, c))
                    {
                        insertCmd.Parameters.AddWithValue("@name", customerName);
                        insertCmd.Parameters.AddWithValue("@note", customerNote);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }


            try
            {
                // 1) upiši sale (profit ne prikazujemo na računu; po želji ga preskoči i u bazi)
                using (var s = new SQLiteCommand(
                    "INSERT INTO sales(total,profit,customer_id,customer_name) VALUES(@t,@p,@cid,@cnm); SELECT last_insert_rowid();", c, tx))
                {
                    s.Parameters.AddWithValue("@t", total);
                    s.Parameters.AddWithValue("@p", profit);
                    s.Parameters.AddWithValue("@cid", (object?)customerId ?? DBNull.Value);
                    s.Parameters.AddWithValue("@cnm", customerName);
                    saleId = (long)(long)s.ExecuteScalar();
                }

                // 2) upiši stavke i smanji lager
                foreach (var it in cart)
                {
                    using (var si = new SQLiteCommand(
                        "INSERT INTO sale_items(sale_id,product_id,qty,price,cost_price) VALUES(@s,@p,@q,@pr,@cp)", c, tx))
                    {
                        si.Parameters.AddWithValue("@s", saleId);
                        si.Parameters.AddWithValue("@p", it.ProductId);
                        si.Parameters.AddWithValue("@q", it.Qty);
                        si.Parameters.AddWithValue("@pr", it.Price * factor);
                        si.Parameters.AddWithValue("@cp", it.CostPrice);
                        si.ExecuteNonQuery();
                    }

                    using (var upg = new SQLiteCommand(
            "SELECT product_quantity FROM products WHERE id = @p", c, tx))
                    {
                        upg.Parameters.AddWithValue("@p", it.ProductId);
                        int affected = Convert.ToInt32(upg.ExecuteScalar());
                        if (affected - it.Qty == 0)
                        {
                            using (var upd = new SQLiteCommand(
                        "DELETE FROM products WHERE id = @p", c, tx))
                            {
                                upd.Parameters.AddWithValue("@p", it.ProductId);
                                int affected1 = upd.ExecuteNonQuery();
                                if (affected1 == 0)
                                    throw new Exception($"Nema dovoljno na lageru za: {it.Name}");
                            }
                        }
                        else
                        {
                            using (var upt = new SQLiteCommand(
          "UPDATE products SET product_quantity = product_quantity - @s WHERE id = @p", c, tx))
                            {
                                upt.Parameters.AddWithValue("@p", it.ProductId);
                                upt.Parameters.AddWithValue("@s", it.Qty);
                                upt.ExecuteNonQuery();

                            }
                        }
                    }


                }

                tx.Commit();

                // 3) sačuvaj račun kao dokument (TXT); NE štampamo automatski
                //string path = SaveReceiptToFile((int)saleId, cart, total);
                var pdfPath = InvoicePdf.Generate((int)saleId, cart,
                            sellerName: "PADRINO FASHION ROOM",
                            sellerAddress: "",
                            discountPercent: GetDiscountPercent(),
                            sellerExtra: "", buyerName: comboBox1.Text, buyerAddress: textBox4.Text);



                LoadCustomers();


                // očisti i osveži UI
                cart.Clear();
                UpdateTotals();
                LoadInventory();
                RefreshStats();      // ako ti prikazuje statistiku negde
                                     // ako imaš grid za inventar


                MessageBox.Show("Račun izdat uspešno.");
            }
            catch (Exception ex)
            {
                try { tx.Rollback(); } catch { }
                MessageBox.Show("Greška: " + ex.Message);
            }
        }


        private string SaveReceiptToFile(int saleId, BindingList<CartItem> items, decimal total)
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Receipts");
            Directory.CreateDirectory(dir);
            string file = Path.Combine(dir, $"Racun_{saleId}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

            var sb = new StringBuilder();
            sb.AppendLine("=== SIMPLE POS — RAČUN ===");
            sb.AppendLine($"Račun #: {saleId}");
            sb.AppendLine($"Datum: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            sb.AppendLine("----------------------------------------");
            foreach (var it in items)
            {
                decimal line = it.Total; // it.Price * it.Qty
                sb.AppendLine($"{it.Name}");
                sb.AppendLine($"  {it.Qty} x {it.Price.ToString("N2", _rs)} = {line.ToString("N2", _rs)} RSD");
            }
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"UKUPNO: {total.ToString("N2", _rs)} RSD");
            sb.AppendLine("Valuta: RSD (din)");
            sb.AppendLine("========================================");

            File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
            return file;
        }

        private void PrintDoc_PrintPage(object? sender, PrintPageEventArgs e)
        {
            float y = 10;
            var g = e.Graphics;
            var font = new System.Drawing.Font("Consolas", 10);
            g.DrawString("SIMPLE POS — RAČUN", new System.Drawing.Font("Consolas", 12, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 10, y); y += 20;
            g.DrawString(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), font, System.Drawing.Brushes.Black, 10, y); y += 20;
            g.DrawString("----------------------------------------", font, System.Drawing.Brushes.Black, 10, y); y += 16;
            foreach (var it in cart)
            {
                g.DrawString($"{it.Name}", font, System.Drawing.Brushes.Black, 10, y); y += 16;
                g.DrawString($"{it.Qty} x {it.Price:N2} = {it.Total:N2}", font, System.Drawing.Brushes.Black, 20, y); y += 16;
            }
            g.DrawString("----------------------------------------", font, System.Drawing.Brushes.Black, 10, y); y += 16;
            g.DrawString($"UKUPNO: {cart.Sum(x => x.Total):N2}", new System.Drawing.Font("Consolas", 11, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 10, y); y += 18;
            g.DrawString($"PROFIT:  {cart.Sum(x => x.Profit):N2}", new System.Drawing.Font("Consolas", 11, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 10, y);
        }

        private void LoadCustomersGrid(string search = "")
        {
            using (var con = Db.Open())
            {


                string query = @"
            SELECT id, name, note
            FROM customers
            WHERE name LIKE @search
            ORDER BY name";

                using (var cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
        }

        private void RefreshStats()
        {
            using var c = Db.Open();
            using (var cmd = new SQLiteCommand("SELECT COALESCE(SUM(total),0), COALESCE(SUM(profit),0) FROM sales", c))
            using (var r = cmd.ExecuteReader())
            {
                if (r.Read())
                {
                    lblStatsRevenue.Text = $"Ukupna prodaja: {Convert.ToDecimal(r.GetDouble(0)):N2} EUR";
                    lblStatsProfit.Text = $"Ukupan profit: {Convert.ToDecimal(r.GetDouble(1)):N2} EUR";
                }
            }
            using var cmd2 = new SQLiteCommand("SELECT COALESCE(SUM(qty),0) FROM sale_items", c);
            var totalUnits = Convert.ToInt32(cmd2.ExecuteScalar());
            lblStatsUnits.Text = $"Ukupno prodatih komada: {totalUnits}";
        }

        private void InitStatsUi()
        {
            // nivoi agregacije
            cmbLevel.Items.Clear();
            cmbLevel.Items.AddRange(new[] { "Godine", "Meseci", "Dani" });
            cmbLevel.SelectedIndex = 0; // Godine

            // popuni godine iz baze (distinct po sales.created_at)
            cmbYear.Items.Clear();
            using (var c = Db.Open())
            using (var cmd = new SQLiteCommand(
                "SELECT DISTINCT strftime('%Y', datetime(ts,'localtime')) AS y FROM sales ORDER BY y;", c))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read()) cmbYear.Items.Add(r.GetString(0));
            }
            if (cmbYear.Items.Count == 0) cmbYear.Items.Add(DateTime.Now.Year.ToString());
            cmbYear.SelectedIndex = cmbYear.Items.Count - 1;

            // meseci 01-12
            cmbMonth.Items.Clear();
            for (int m = 1; m <= 12; m++) cmbMonth.Items.Add(m.ToString("00"));
            cmbMonth.SelectedIndex = Math.Max(0, DateTime.Now.Month - 1);

            // eventi
            cmbLevel.SelectedIndexChanged += (_, __) => ToggleMonthVisibility();
            btnRefresh.Click += (_, __) => LoadSummary();
            dgvSummary.SelectionChanged += (_, __) => LoadSalesForSelectedPeriod();

            // prvi prikaz
            ToggleMonthVisibility();
            LoadSummary();
        }

        private void ToggleMonthVisibility()
        {
            var level = cmbLevel.SelectedItem?.ToString();
            cmbYear.Enabled = level == "Meseci" || level == "Dani";
            cmbMonth.Enabled = level == "Dani";
        }

        private void LoadSummary()
        {
            using var c = Db.Open();
            using var cmd = new SQLiteCommand();
            cmd.Connection = c;

            var level = cmbLevel.SelectedItem?.ToString();

            if (level == "Godine")
            {
                cmd.CommandText = @"
            SELECT strftime('%Y', datetime(s.ts,'localtime')) AS period,
                   COUNT(DISTINCT s.id) AS receipts,
                   SUM(si.qty) AS items_sold,
                   SUM(si.qty*si.price) AS revenue,
                   SUM(si.qty*(si.price - si.cost_price)) AS profit
            FROM sales s
            JOIN sale_items si ON si.sale_id = s.id
            GROUP BY period
            ORDER BY period;";
            }
            else if (level == "Meseci")
            {
                var year = cmbYear.SelectedItem?.ToString() ?? DateTime.Now.Year.ToString();
                cmd.CommandText = @"
            SELECT strftime('%Y-%m', datetime(s.ts,'localtime')) AS period,
                   COUNT(DISTINCT s.id) AS receipts,
                   SUM(si.qty) AS items_sold,
                   SUM(si.qty*si.price) AS revenue,
                   SUM(si.qty*(si.price - si.cost_price)) AS profit
            FROM sales s
            JOIN sale_items si ON si.sale_id = s.id
            WHERE strftime('%Y', datetime(s.ts,'localtime')) = @year
            GROUP BY period
            ORDER BY period;";
                cmd.Parameters.AddWithValue("@year", year);
            }
            else // Dani
            {
                var year = cmbYear.SelectedItem?.ToString() ?? DateTime.Now.Year.ToString();
                var ym = $"{year}-{(cmbMonth.SelectedIndex + 1).ToString("00")}";
                cmd.CommandText = @"
            SELECT strftime('%Y-%m-%d', datetime(s.ts,'localtime')) AS period,
                   COUNT(DISTINCT s.id) AS receipts,
                   SUM(si.qty) AS items_sold,
                   SUM(si.qty*si.price) AS revenue,
                   SUM(si.qty*(si.price - si.cost_price)) AS profit
            FROM sales s
            JOIN sale_items si ON si.sale_id = s.id
            WHERE strftime('%Y-%m', datetime(s.ts,'localtime')) = @ym
            GROUP BY period
            ORDER BY period;";
                cmd.Parameters.AddWithValue("@ym", ym);
            }

            var dt = new DataTable();
            using (var da = new SQLiteDataAdapter(cmd)) da.Fill(dt);

            dgvSummary.AutoGenerateColumns = true;
            dgvSummary.DataSource = dt;
            dgvSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            FormatMoney(dgvSummary, "revenue", "profit");
            AlignRight(dgvSummary, "receipts", "items_sold", "revenue", "profit");

            // automatski učitaj račune za prvi red
            LoadSalesForSelectedPeriod();
        }

        private void LoadSalesForSelectedPeriod()
        {
            if (dgvSummary.CurrentRow == null) { dgvSales.DataSource = null; return; }
            var period = dgvSummary.CurrentRow.Cells["period"].Value?.ToString();
            if (string.IsNullOrEmpty(period)) { dgvSales.DataSource = null; return; }

            var level = cmbLevel.SelectedItem?.ToString();
            string whereExpr;
            string paramValue;

            if (level == "Godine")
            {
                whereExpr = "strftime('%Y', datetime(s.ts,'localtime')) = @p";
                paramValue = period;             // "YYYY"
            }
            else if (level == "Meseci")
            {
                whereExpr = "strftime('%Y-%m', datetime(s.ts,'localtime')) = @p";
                paramValue = period;             // "YYYY-MM"
            }
            else
            {
                whereExpr = "strftime('%Y-%m-%d', datetime(s.ts,'localtime')) = @p";
                paramValue = period;             // "YYYY-MM-DD"
            }

            using var c = Db.Open();
            using var cmd = new SQLiteCommand($@"
        SELECT
          s.id,
          datetime(s.ts,'localtime') AS ts,
          s.total,
          (SELECT SUM(si.qty*(si.price - si.cost_price)) FROM sale_items si WHERE si.sale_id = s.id) AS profit,
          (SELECT SUM(si.qty) FROM sale_items si WHERE si.sale_id = s.id) AS items
        FROM sales s
        WHERE {whereExpr}
        ORDER BY s.ts;", c);
            cmd.Parameters.AddWithValue("@p", paramValue);

            var dt = new DataTable();
            using (var da = new SQLiteDataAdapter(cmd)) da.Fill(dt);

            dgvSales.AutoGenerateColumns = true;
            dgvSales.DataSource = dt;
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            FormatMoney(dgvSales, "total", "profit");
            AlignRight(dgvSales, "items", "total", "profit");
        }

        private static void FormatMoney(DataGridView dgv, params string[] cols)
        {
            foreach (var c in cols)
                if (dgv.Columns.Contains(c))
                    dgv.Columns[c].DefaultCellStyle.Format = "N2";
        }

        private static void AlignRight(DataGridView dgv, params string[] cols)
        {
            foreach (var c in cols)
                if (dgv.Columns.Contains(c))
                    dgv.Columns[c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using var c = Db.Open();
            using var tx = c.BeginTransaction();

            try
            {
                using (var cmd = c.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
                DELETE FROM sale_items;
                DELETE FROM sales;
                DELETE FROM products;
            ";
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();

                using (var vacuum = c.CreateCommand())
                {
                    vacuum.CommandText = "VACUUM;";
                    vacuum.ExecuteNonQuery();
                }

                using (var reset = c.CreateCommand())
                {
                    reset.CommandText = "DELETE FROM sqlite_sequence;";
                    reset.ExecuteNonQuery();
                }

                cart.Clear();
                UpdateTotals();
                LoadInventory();
                RefreshStats();

                MessageBox.Show("Baza je uspešno obrisana (sve tabele prazne).");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                MessageBox.Show("Greška: " + ex.Message);
            }
        }
        private decimal GetDiscountPercent()
        {
            // textBox2 = popust u %, npr "20" ili "20,5"
            var raw = (textBox2.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(raw))
                return 0m;

            // dozvoli i zarez i tačku
            raw = raw.Replace(',', '.');

            if (!decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var pct))
                return 0m;

            if (pct < 0m) pct = 0m;
            if (pct > 100m) pct = 100m;

            return pct;
        }

        private decimal GetDiscountFactor()
        {
            var pct = GetDiscountPercent();
            return 1m - (pct / 100m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using var c = Db.Open();
            using var tx = c.BeginTransaction();

            try
            {
                using (var cmd = c.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
               
                DELETE FROM sales;
                DELETE FROM sale_items;
             
            ";
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();

                using (var vacuum = c.CreateCommand())
                {
                    vacuum.CommandText = "VACUUM;";
                    vacuum.ExecuteNonQuery();
                }

                cart.Clear();
                UpdateTotals();
                LoadInventory();
                RefreshStats();

                MessageBox.Show("Statistika je uspešno obrisana.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                MessageBox.Show("Greška: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void txtScan_TextChanged(object sender, EventArgs e)
        {

        }

        private void gridCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabStats_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            UpdateTotals();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            textBox2.Text = GetDiscountPercent().ToString("0.##", _rs); UpdateTotals();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                int saleId = Convert.ToInt32(dgvSales.Rows[e.RowIndex].Cells["id"].Value);

                string invoicesFolder = Path.Combine(Application.StartupPath, "Invoices");

                if (!Directory.Exists(invoicesFolder))
                {
                    MessageBox.Show("Folder Invoices ne postoji.");
                    return;
                }

                string[] files = Directory.GetFiles(invoicesFolder, $"Racun_{saleId}_*.pdf");

                if (files.Length == 0)
                {
                    MessageBox.Show("Račun nije pronađen.");
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = files[0],
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri otvaranju računa: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is CustomerItem selectedCustomer)
            {
                using (var con = Db.Open())
                {


                    string query = "SELECT note FROM customers WHERE id = @id";

                    using (var cmd = new SQLiteCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedCustomer.Id);

                        var result = cmd.ExecuteScalar();
                        textBox4.Text = result?.ToString() ?? "";
                    }
                }
            }
        }

        private void tabs_Click(object sender, EventArgs e)
        {
            LoadCustomers();
            LoadCustomersGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int customerId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
            LoadSalesByCustomer(customerId);
        }
        private void LoadSalesByCustomer(int customerId)
        {
            using (var con = Db.Open())
            {
                ;

                string query = @"
            SELECT id, customer_name, total, profit, ts
            FROM sales
            WHERE customer_id = @customer_id
            ORDER BY id DESC";

                using (var cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@customer_id", customerId);

                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView2.DataSource = dt;
                    }
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            LoadCustomersGrid(textBox3.Text.Trim());
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                int saleId = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value);

                string invoicesFolder = Path.Combine(Application.StartupPath, "Invoices");

                if (!Directory.Exists(invoicesFolder))
                {
                    MessageBox.Show("Folder Invoices ne postoji.");
                    return;
                }

                string[] files = Directory.GetFiles(invoicesFolder, $"Racun_{saleId}_*.pdf");

                if (files.Length == 0)
                {
                    MessageBox.Show("Račun nije pronađen.");
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = files[0],
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri otvaranju računa: " + ex.Message);
            }
        }
    }
}
