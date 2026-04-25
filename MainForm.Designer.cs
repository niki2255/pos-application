namespace SimplePOS
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabPOS;
        private System.Windows.Forms.TabPage tabInventory;
        private System.Windows.Forms.TabPage tabStats;

        private System.Windows.Forms.TextBox txtScan;
        private System.Windows.Forms.DataGridView gridCart;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.Button btnClearCart;

        private System.Windows.Forms.DataGridView gridInv;
        private System.Windows.Forms.Button btnInvAdd;
        private System.Windows.Forms.Button btnInvSave;
        private System.Windows.Forms.Button btnInvDelete;

        private System.Windows.Forms.Label lblStatsRevenue;
        private System.Windows.Forms.Label lblStatsProfit;
        private System.Windows.Forms.Label lblStatsUnits;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabs = new TabControl();
            tabPOS = new TabPage();
            comboBox1 = new ComboBox();
            textBox4 = new TextBox();
            label3 = new Label();
            label1 = new Label();
            label2 = new Label();
            textBox2 = new TextBox();
            btnClearCart = new Button();
            btnCheckout = new Button();
            lblTotal = new Label();
            gridCart = new DataGridView();
            txtScan = new TextBox();
            tabInventory = new TabPage();
            textBox1 = new TextBox();
            btnInvDelete = new Button();
            btnInvSave = new Button();
            btnInvAdd = new Button();
            gridInv = new DataGridView();
            tabStats = new TabPage();
            dgvSales = new DataGridView();
            dgvSummary = new DataGridView();
            cmbMonth = new ComboBox();
            cmbYear = new ComboBox();
            cmbLevel = new ComboBox();
            btnRefresh = new Button();
            button2 = new Button();
            button1 = new Button();
            lblStatsUnits = new Label();
            lblStatsProfit = new Label();
            lblStatsRevenue = new Label();
            tabCustomers = new TabPage();
            textBox3 = new TextBox();
            dataGridView2 = new DataGridView();
            dataGridView1 = new DataGridView();
            tabs.SuspendLayout();
            tabPOS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCart).BeginInit();
            tabInventory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridInv).BeginInit();
            tabStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSummary).BeginInit();
            tabCustomers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // tabs
            // 
            tabs.Controls.Add(tabPOS);
            tabs.Controls.Add(tabInventory);
            tabs.Controls.Add(tabStats);
            tabs.Controls.Add(tabCustomers);
            tabs.Location = new Point(12, 12);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(860, 525);
            tabs.TabIndex = 0;
            tabs.Click += tabs_Click;
            // 
            // tabPOS
            // 
            tabPOS.Controls.Add(comboBox1);
            tabPOS.Controls.Add(textBox4);
            tabPOS.Controls.Add(label3);
            tabPOS.Controls.Add(label1);
            tabPOS.Controls.Add(label2);
            tabPOS.Controls.Add(textBox2);
            tabPOS.Controls.Add(btnClearCart);
            tabPOS.Controls.Add(btnCheckout);
            tabPOS.Controls.Add(lblTotal);
            tabPOS.Controls.Add(gridCart);
            tabPOS.Controls.Add(txtScan);
            tabPOS.Location = new Point(4, 24);
            tabPOS.Name = "tabPOS";
            tabPOS.Padding = new Padding(3);
            tabPOS.Size = new Size(852, 497);
            tabPOS.TabIndex = 0;
            tabPOS.Text = "Kasa";
            tabPOS.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(74, 454);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(199, 23);
            comboBox1.TabIndex = 10;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(384, 454);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(165, 23);
            textBox4.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(279, 454);
            label3.Name = "label3";
            label3.Size = new Size(99, 21);
            label3.TabIndex = 9;
            label3.Text = "Napomena:";
            label3.Click += label3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(349, 420);
            label1.Name = "label1";
            label1.Size = new Size(97, 21);
            label1.TabIndex = 6;
            label1.Text = "Popust (%):";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(7, 454);
            label2.Name = "label2";
            label2.Size = new Size(61, 21);
            label2.TabIndex = 7;
            label2.Text = "Kupac:";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(449, 422);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 5;
            textBox2.TextChanged += textBox2_TextChanged;
            textBox2.Leave += textBox2_Leave;
            // 
            // btnClearCart
            // 
            btnClearCart.Location = new Point(600, 436);
            btnClearCart.Name = "btnClearCart";
            btnClearCart.Size = new Size(120, 35);
            btnClearCart.TabIndex = 4;
            btnClearCart.Text = "Isprazni korpu";
            btnClearCart.UseVisualStyleBackColor = true;
            btnClearCart.Click += btnClearCart_Click;
            // 
            // btnCheckout
            // 
            btnCheckout.Location = new Point(726, 436);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(120, 35);
            btnCheckout.TabIndex = 3;
            btnCheckout.Text = "Izdaj račun";
            btnCheckout.UseVisualStyleBackColor = true;
            btnCheckout.Click += btnCheckout_Click;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotal.Location = new Point(6, 420);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(123, 21);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "Ukupno: 0 EUR";
            // 
            // gridCart
            // 
            gridCart.AllowUserToAddRows = false;
            gridCart.AllowUserToDeleteRows = false;
            gridCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridCart.Location = new Point(6, 50);
            gridCart.Name = "gridCart";
            gridCart.RowTemplate.Height = 25;
            gridCart.Size = new Size(840, 367);
            gridCart.TabIndex = 1;
            gridCart.CellContentClick += gridCart_CellContentClick;
            // 
            // txtScan
            // 
            txtScan.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtScan.Location = new Point(6, 6);
            txtScan.Name = "txtScan";
            txtScan.PlaceholderText = "Skener / Barkod (ENTER)";
            txtScan.Size = new Size(400, 29);
            txtScan.TabIndex = 0;
            txtScan.TextChanged += txtScan_TextChanged;
            txtScan.KeyDown += txtScan_KeyDown;
            // 
            // tabInventory
            // 
            tabInventory.Controls.Add(textBox1);
            tabInventory.Controls.Add(btnInvDelete);
            tabInventory.Controls.Add(btnInvSave);
            tabInventory.Controls.Add(btnInvAdd);
            tabInventory.Controls.Add(gridInv);
            tabInventory.Location = new Point(4, 24);
            tabInventory.Name = "tabInventory";
            tabInventory.Padding = new Padding(3);
            tabInventory.Size = new Size(852, 497);
            tabInventory.TabIndex = 1;
            tabInventory.Text = "Inventar";
            tabInventory.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(413, 12);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = " Unesi Barkod Ili Ime";
            textBox1.Size = new Size(276, 29);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // btnInvDelete
            // 
            btnInvDelete.Location = new Point(258, 12);
            btnInvDelete.Name = "btnInvDelete";
            btnInvDelete.Size = new Size(120, 29);
            btnInvDelete.TabIndex = 3;
            btnInvDelete.Text = "Obriši artikal";
            btnInvDelete.UseVisualStyleBackColor = true;
            btnInvDelete.Click += btnInvDelete_Click;
            // 
            // btnInvSave
            // 
            btnInvSave.Location = new Point(132, 12);
            btnInvSave.Name = "btnInvSave";
            btnInvSave.Size = new Size(120, 29);
            btnInvSave.TabIndex = 2;
            btnInvSave.Text = "Sačuvaj izmene";
            btnInvSave.UseVisualStyleBackColor = true;
            btnInvSave.Click += btnInvSave_Click;
            // 
            // btnInvAdd
            // 
            btnInvAdd.Location = new Point(6, 12);
            btnInvAdd.Name = "btnInvAdd";
            btnInvAdd.Size = new Size(120, 29);
            btnInvAdd.TabIndex = 1;
            btnInvAdd.Text = "Dodaj artikal";
            btnInvAdd.UseVisualStyleBackColor = true;
            btnInvAdd.Click += btnInvAdd_Click;
            // 
            // gridInv
            // 
            gridInv.AllowUserToAddRows = false;
            gridInv.AllowUserToDeleteRows = false;
            gridInv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridInv.Location = new Point(6, 50);
            gridInv.Name = "gridInv";
            gridInv.RowTemplate.Height = 25;
            gridInv.Size = new Size(840, 436);
            gridInv.TabIndex = 0;
            // 
            // tabStats
            // 
            tabStats.Controls.Add(dgvSales);
            tabStats.Controls.Add(dgvSummary);
            tabStats.Controls.Add(cmbMonth);
            tabStats.Controls.Add(cmbYear);
            tabStats.Controls.Add(cmbLevel);
            tabStats.Controls.Add(btnRefresh);
            tabStats.Controls.Add(button2);
            tabStats.Controls.Add(button1);
            tabStats.Controls.Add(lblStatsUnits);
            tabStats.Controls.Add(lblStatsProfit);
            tabStats.Controls.Add(lblStatsRevenue);
            tabStats.Location = new Point(4, 24);
            tabStats.Name = "tabStats";
            tabStats.Padding = new Padding(3);
            tabStats.Size = new Size(852, 497);
            tabStats.TabIndex = 2;
            tabStats.Text = "Statistika";
            tabStats.UseVisualStyleBackColor = true;
            tabStats.Click += tabStats_Click;
            // 
            // dgvSales
            // 
            dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSales.Location = new Point(437, 132);
            dgvSales.Name = "dgvSales";
            dgvSales.RowTemplate.Height = 25;
            dgvSales.Size = new Size(392, 335);
            dgvSales.TabIndex = 10;
            dgvSales.CellDoubleClick += dgvSales_CellDoubleClick;
            // 
            // dgvSummary
            // 
            dgvSummary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSummary.Location = new Point(20, 132);
            dgvSummary.Name = "dgvSummary";
            dgvSummary.RowTemplate.Height = 25;
            dgvSummary.Size = new Size(394, 335);
            dgvSummary.TabIndex = 9;
            // 
            // cmbMonth
            // 
            cmbMonth.FormattingEnabled = true;
            cmbMonth.Location = new Point(667, 34);
            cmbMonth.Name = "cmbMonth";
            cmbMonth.Size = new Size(162, 23);
            cmbMonth.TabIndex = 8;
            // 
            // cmbYear
            // 
            cmbYear.FormattingEnabled = true;
            cmbYear.Location = new Point(482, 34);
            cmbYear.Name = "cmbYear";
            cmbYear.Size = new Size(164, 23);
            cmbYear.TabIndex = 7;
            // 
            // cmbLevel
            // 
            cmbLevel.FormattingEnabled = true;
            cmbLevel.Location = new Point(300, 34);
            cmbLevel.Name = "cmbLevel";
            cmbLevel.Size = new Size(161, 23);
            cmbLevel.TabIndex = 6;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(482, 88);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(164, 23);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(667, 88);
            button2.Name = "button2";
            button2.Size = new Size(164, 23);
            button2.TabIndex = 4;
            button2.Text = "Resetuj bazu";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(665, 469);
            button1.Name = "button1";
            button1.Size = new Size(164, 23);
            button1.TabIndex = 3;
            button1.Text = "Resetuj statistiku";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // lblStatsUnits
            // 
            lblStatsUnits.AutoSize = true;
            lblStatsUnits.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatsUnits.Location = new Point(20, 90);
            lblStatsUnits.Name = "lblStatsUnits";
            lblStatsUnits.Size = new Size(224, 21);
            lblStatsUnits.TabIndex = 2;
            lblStatsUnits.Text = "Ukupno prodatih komada: 0";
            // 
            // lblStatsProfit
            // 
            lblStatsProfit.AutoSize = true;
            lblStatsProfit.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatsProfit.Location = new Point(20, 55);
            lblStatsProfit.Name = "lblStatsProfit";
            lblStatsProfit.Size = new Size(134, 21);
            lblStatsProfit.TabIndex = 1;
            lblStatsProfit.Text = "Ukupan profit: 0";
            // 
            // lblStatsRevenue
            // 
            lblStatsRevenue.AutoSize = true;
            lblStatsRevenue.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatsRevenue.Location = new Point(20, 20);
            lblStatsRevenue.Name = "lblStatsRevenue";
            lblStatsRevenue.Size = new Size(150, 21);
            lblStatsRevenue.TabIndex = 0;
            lblStatsRevenue.Text = "Ukupna prodaja: 0";
            // 
            // tabCustomers
            // 
            tabCustomers.Controls.Add(textBox3);
            tabCustomers.Controls.Add(dataGridView2);
            tabCustomers.Controls.Add(dataGridView1);
            tabCustomers.Location = new Point(4, 24);
            tabCustomers.Name = "tabCustomers";
            tabCustomers.Size = new Size(852, 497);
            tabCustomers.TabIndex = 3;
            tabCustomers.Text = "Kupci";
            tabCustomers.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBox3.Location = new Point(31, 21);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = " Unesi Ime";
            textBox3.Size = new Size(361, 29);
            textBox3.TabIndex = 5;
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(433, 70);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowTemplate.Height = 25;
            dataGridView2.Size = new Size(392, 424);
            dataGridView2.TabIndex = 1;
            dataGridView2.CellDoubleClick += dataGridView2_CellDoubleClick;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(31, 70);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(361, 424);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 549);
            Controls.Add(tabs);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PANAMA OUTFIT - kasa";
            Load += MainForm_Load;
            tabs.ResumeLayout(false);
            tabPOS.ResumeLayout(false);
            tabPOS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridCart).EndInit();
            tabInventory.ResumeLayout(false);
            tabInventory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridInv).EndInit();
            tabStats.ResumeLayout(false);
            tabStats.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSummary).EndInit();
            tabCustomers.ResumeLayout(false);
            tabCustomers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        private Button button2;
        private Button button1;
        private TextBox textBox1;
        private DataGridView dgvSales;
        private DataGridView dgvSummary;
        private ComboBox cmbMonth;
        private ComboBox cmbYear;
        private ComboBox cmbLevel;
        private Button btnRefresh;
        private Label label1;
        private TextBox textBox2;
        private TextBox textBox4;
        private Label label2;
        private Label label3;
        private ComboBox comboBox1;
        private TabPage tabCustomers;
        private DataGridView dataGridView2;
        private DataGridView dataGridView1;
        private TextBox textBox3;
    }
}
