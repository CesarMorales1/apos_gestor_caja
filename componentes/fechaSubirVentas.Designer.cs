namespace apos_gestor_caja.Forms
{
    partial class VentasForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnObtenerArchivos = new System.Windows.Forms.Button();
            this.lstArchivos = new System.Windows.Forms.ListBox();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.txtCaja = new System.Windows.Forms.TextBox();
            this.lblCaja = new System.Windows.Forms.Label();
            this.lblCargando = new System.Windows.Forms.Label();
            this.btnSubirVentas = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnObtenerArchivos
            // 
            this.btnObtenerArchivos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(53)))), ((int)(((byte)(84)))));
            this.btnObtenerArchivos.FlatAppearance.BorderSize = 0;
            this.btnObtenerArchivos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnObtenerArchivos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnObtenerArchivos.ForeColor = System.Drawing.Color.White;
            this.btnObtenerArchivos.Location = new System.Drawing.Point(30, 340);
            this.btnObtenerArchivos.Name = "btnObtenerArchivos";
            this.btnObtenerArchivos.Size = new System.Drawing.Size(180, 40);
            this.btnObtenerArchivos.TabIndex = 0;
            this.btnObtenerArchivos.Text = "Obtener Archivos";
            this.btnObtenerArchivos.UseVisualStyleBackColor = false;
            this.btnObtenerArchivos.Click += new System.EventHandler(this.btnObtenerArchivos_Click);
            // 
            // lstArchivos
            // 
            this.lstArchivos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lstArchivos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstArchivos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstArchivos.FormattingEnabled = true;
            this.lstArchivos.ItemHeight = 28;
            this.lstArchivos.Location = new System.Drawing.Point(30, 150);
            this.lstArchivos.Name = "lstArchivos";
            this.lstArchivos.Size = new System.Drawing.Size(624, 170);
            this.lstArchivos.TabIndex = 1;
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.CustomFormat = "dd/MM/yyyy";
            this.dtpFechaInicio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaInicio.Location = new System.Drawing.Point(170, 65);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(160, 34);
            this.dtpFechaInicio.TabIndex = 2;
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.CustomFormat = "dd/MM/yyyy";
            this.dtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaFin.Location = new System.Drawing.Point(434, 65);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(160, 34);
            this.dtpFechaFin.TabIndex = 3;
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.AutoSize = true;
            this.lblFechaInicio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFechaInicio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblFechaInicio.Location = new System.Drawing.Point(53, 70);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(114, 28);
            this.lblFechaInicio.TabIndex = 4;
            this.lblFechaInicio.Text = "Fecha Inicio";
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFechaFin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblFechaFin.Location = new System.Drawing.Point(336, 70);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(93, 28);
            this.lblFechaFin.TabIndex = 5;
            this.lblFechaFin.Text = "Fecha Fin";
            // 
            // txtCaja
            // 
            this.txtCaja.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtCaja.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCaja.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCaja.Location = new System.Drawing.Point(170, 105);
            this.txtCaja.Name = "txtCaja";
            this.txtCaja.Size = new System.Drawing.Size(64, 34);
            this.txtCaja.TabIndex = 6;
            this.txtCaja.TextChanged += new System.EventHandler(this.txtCaja_TextChanged);
            // 
            // lblCaja
            // 
            this.lblCaja.AutoSize = true;
            this.lblCaja.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCaja.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblCaja.Location = new System.Drawing.Point(115, 107);
            this.lblCaja.Name = "lblCaja";
            this.lblCaja.Size = new System.Drawing.Size(49, 28);
            this.lblCaja.TabIndex = 7;
            this.lblCaja.Text = "Caja";
            // 
            // lblCargando
            // 
            this.lblCargando.AutoSize = true;
            this.lblCargando.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCargando.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.lblCargando.Location = new System.Drawing.Point(260, 350);
            this.lblCargando.Name = "lblCargando";
            this.lblCargando.Size = new System.Drawing.Size(0, 28);
            this.lblCargando.TabIndex = 8;
            this.lblCargando.Visible = false;
            // 
            // btnSubirVentas
            // 
            this.btnSubirVentas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(53)))), ((int)(((byte)(84)))));
            this.btnSubirVentas.FlatAppearance.BorderSize = 0;
            this.btnSubirVentas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubirVentas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSubirVentas.ForeColor = System.Drawing.Color.White;
            this.btnSubirVentas.Location = new System.Drawing.Point(474, 344);
            this.btnSubirVentas.Name = "btnSubirVentas";
            this.btnSubirVentas.Size = new System.Drawing.Size(180, 40);
            this.btnSubirVentas.TabIndex = 9;
            this.btnSubirVentas.Text = "Subir Ventas";
            this.btnSubirVentas.UseVisualStyleBackColor = false;
            this.btnSubirVentas.Click += new System.EventHandler(this.btnSubirVentas_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(713, 50);
            this.panelHeader.TabIndex = 10;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(252, 38);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Gestión de Ventas";
            // 
            // VentasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(713, 400);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.btnSubirVentas);
            this.Controls.Add(this.lblCargando);
            this.Controls.Add(this.lblCaja);
            this.Controls.Add(this.txtCaja);
            this.Controls.Add(this.lblFechaFin);
            this.Controls.Add(this.lblFechaInicio);
            this.Controls.Add(this.dtpFechaFin);
            this.Controls.Add(this.dtpFechaInicio);
            this.Controls.Add(this.lstArchivos);
            this.Controls.Add(this.btnObtenerArchivos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "VentasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Ventas";
            this.Load += new System.EventHandler(this.VentasForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnObtenerArchivos;
        private System.Windows.Forms.ListBox lstArchivos;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.Label lblFechaInicio;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.TextBox txtCaja;
        private System.Windows.Forms.Label lblCaja;
        private System.Windows.Forms.Label lblCargando;
        private System.Windows.Forms.Button btnSubirVentas;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
    }
}