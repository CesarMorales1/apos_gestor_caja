namespace apos_gestor_caja.componentes
{
    partial class CierreCaja
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

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.panelEncabezado = new System.Windows.Forms.Panel();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.dateTimePickerFecha = new System.Windows.Forms.DateTimePicker();
            this.labelFecha = new System.Windows.Forms.Label();
            this.textBoxCajeroId = new System.Windows.Forms.TextBox();
            this.labelCajero = new System.Windows.Forms.Label();
            this.textBoxCaja = new System.Windows.Forms.TextBox();
            this.labelCaja = new System.Windows.Forms.Label();
            this.buttonBuscar = new System.Windows.Forms.Button();
            this.buttonLimpiar = new System.Windows.Forms.Button();
            this.dataGridViewCierre = new System.Windows.Forms.DataGridView();
            this.labelTotalCantidad = new System.Windows.Forms.Label();
            this.labelTotalMonto = new System.Windows.Forms.Label();
            this.labelTotalBauche = new System.Windows.Forms.Label();
            this.panelEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCierre)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEncabezado
            // 
            this.panelEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panelEncabezado.Controls.Add(this.labelTitulo);
            this.panelEncabezado.Location = new System.Drawing.Point(0, 0);
            this.panelEncabezado.Name = "panelEncabezado";
            this.panelEncabezado.Size = new System.Drawing.Size(800, 50);
            this.panelEncabezado.TabIndex = 0;
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitulo.ForeColor = System.Drawing.Color.White;
            this.labelTitulo.Location = new System.Drawing.Point(20, 12);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(220, 25);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "Resumen del Cierre de Caja";
            // 
            // dateTimePickerFecha
            // 
            this.dateTimePickerFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerFecha.Location = new System.Drawing.Point(130, 70);
            this.dateTimePickerFecha.Name = "dateTimePickerFecha";
            this.dateTimePickerFecha.Size = new System.Drawing.Size(120, 22);
            this.dateTimePickerFecha.TabIndex = 1;
            // 
            // labelFecha
            // 
            this.labelFecha.AutoSize = true;
            this.labelFecha.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelFecha.Location = new System.Drawing.Point(80, 73);
            this.labelFecha.Name = "labelFecha";
            this.labelFecha.Size = new System.Drawing.Size(43, 15);
            this.labelFecha.TabIndex = 2;
            this.labelFecha.Text = "Fecha:";
            // 
            // textBoxCajeroId
            // 
            this.textBoxCajeroId.Location = new System.Drawing.Point(330, 70);
            this.textBoxCajeroId.Name = "textBoxCajeroId";
            this.textBoxCajeroId.Size = new System.Drawing.Size(50, 22);
            this.textBoxCajeroId.TabIndex = 3;
            this.textBoxCajeroId.Text = "4";
            this.textBoxCajeroId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxNumeros_KeyPress);
            // 
            // labelCajero
            // 
            this.labelCajero.AutoSize = true;
            this.labelCajero.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelCajero.Location = new System.Drawing.Point(260, 73);
            this.labelCajero.Name = "labelCajero";
            this.labelCajero.Size = new System.Drawing.Size(64, 15);
            this.labelCajero.TabIndex = 4;
            this.labelCajero.Text = "ID Cajero:";
            // 
            // textBoxCaja
            // 
            this.textBoxCaja.Location = new System.Drawing.Point(460, 70);
            this.textBoxCaja.Name = "textBoxCaja";
            this.textBoxCaja.Size = new System.Drawing.Size(50, 22);
            this.textBoxCaja.TabIndex = 5;
            this.textBoxCaja.Text = "7";
            this.textBoxCaja.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxNumeros_KeyPress);
            // 
            // labelCaja
            // 
            this.labelCaja.AutoSize = true;
            this.labelCaja.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelCaja.Location = new System.Drawing.Point(410, 73);
            this.labelCaja.Name = "labelCaja";
            this.labelCaja.Size = new System.Drawing.Size(38, 15);
            this.labelCaja.TabIndex = 6;
            this.labelCaja.Text = "Caja:";
            // 
            // buttonBuscar
            // 
            this.buttonBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.buttonBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBuscar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonBuscar.ForeColor = System.Drawing.Color.White;
            this.buttonBuscar.Location = new System.Drawing.Point(550, 70);
            this.buttonBuscar.Name = "buttonBuscar";
            this.buttonBuscar.Size = new System.Drawing.Size(100, 30);
            this.buttonBuscar.TabIndex = 7;
            this.buttonBuscar.Text = "Buscar";
            this.buttonBuscar.UseVisualStyleBackColor = false;
            this.buttonBuscar.Click += new System.EventHandler(this.ButtonBuscar_Click);
            // 
            // buttonLimpiar
            // 
            this.buttonLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(87)))));
            this.buttonLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLimpiar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonLimpiar.ForeColor = System.Drawing.Color.White;
            this.buttonLimpiar.Location = new System.Drawing.Point(660, 70);
            this.buttonLimpiar.Name = "buttonLimpiar";
            this.buttonLimpiar.Size = new System.Drawing.Size(100, 30);
            this.buttonLimpiar.TabIndex = 8;
            this.buttonLimpiar.Text = "Limpiar";
            this.buttonLimpiar.UseVisualStyleBackColor = false;
            this.buttonLimpiar.Click += new System.EventHandler(this.ButtonLimpiar_Click);
            // 
            // dataGridViewCierre
            // 
            // Dentro de InitializeComponent(), ajustar dataGridViewCierre:
            this.dataGridViewCierre.AllowUserToAddRows = false;
            this.dataGridViewCierre.AllowUserToDeleteRows = false;
            this.dataGridViewCierre.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewCierre.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCierre.GridColor = System.Drawing.Color.LightGray;
            this.dataGridViewCierre.Location = new System.Drawing.Point(20, 120);
            this.dataGridViewCierre.Name = "dataGridViewCierre";
            this.dataGridViewCierre.ReadOnly = false; // Cambiar a false para permitir edición
            this.dataGridViewCierre.RowHeadersVisible = false;
            this.dataGridViewCierre.Size = new System.Drawing.Size(760, 400);
            this.dataGridViewCierre.TabIndex = 10;
            this.dataGridViewCierre.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridViewCierre_CellFormatting);
            this.dataGridViewCierre.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewCierre_CellEndEdit);
            // 
            // labelTotalCantidad
            // 
            this.labelTotalCantidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.labelTotalCantidad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTotalCantidad.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelTotalCantidad.Location = new System.Drawing.Point(20, 530);
            this.labelTotalCantidad.Name = "labelTotalCantidad";
            this.labelTotalCantidad.Size = new System.Drawing.Size(240, 25);
            this.labelTotalCantidad.TabIndex = 10;
            this.labelTotalCantidad.Text = "Total Cantidad: $0.00";
            this.labelTotalCantidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTotalMonto
            // 
            this.labelTotalMonto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.labelTotalMonto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTotalMonto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelTotalMonto.Location = new System.Drawing.Point(270, 530);
            this.labelTotalMonto.Name = "labelTotalMonto";
            this.labelTotalMonto.Size = new System.Drawing.Size(240, 25);
            this.labelTotalMonto.TabIndex = 11;
            this.labelTotalMonto.Text = "Total Monto: $0.00";
            this.labelTotalMonto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTotalBauche
            // 
            this.labelTotalBauche.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.labelTotalBauche.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTotalBauche.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelTotalBauche.Location = new System.Drawing.Point(520, 530);
            this.labelTotalBauche.Name = "labelTotalBauche";
            this.labelTotalBauche.Size = new System.Drawing.Size(260, 25);
            this.labelTotalBauche.TabIndex = 12;
            this.labelTotalBauche.Text = "Total Bauche: $0.00";
            this.labelTotalBauche.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CierreCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.labelTotalBauche);
            this.Controls.Add(this.labelTotalMonto);
            this.Controls.Add(this.labelTotalCantidad);
            this.Controls.Add(this.dataGridViewCierre);
            this.Controls.Add(this.buttonLimpiar);
            this.Controls.Add(this.buttonBuscar);
            this.Controls.Add(this.labelCaja);
            this.Controls.Add(this.textBoxCaja);
            this.Controls.Add(this.labelCajero);
            this.Controls.Add(this.textBoxCajeroId);
            this.Controls.Add(this.labelFecha);
            this.Controls.Add(this.dateTimePickerFecha);
            this.Controls.Add(this.panelEncabezado);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CierreCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cierre de Caja";
            this.panelEncabezado.ResumeLayout(false);
            this.panelEncabezado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCierre)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelEncabezado;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFecha;
        private System.Windows.Forms.Label labelFecha;
        private System.Windows.Forms.TextBox textBoxCajeroId;
        private System.Windows.Forms.Label labelCajero;
        private System.Windows.Forms.TextBox textBoxCaja;
        private System.Windows.Forms.Label labelCaja;
        private System.Windows.Forms.Button buttonBuscar;
        private System.Windows.Forms.Button buttonLimpiar;
        private System.Windows.Forms.DataGridView dataGridViewCierre;
        private System.Windows.Forms.Label labelTotalCantidad;
        private System.Windows.Forms.Label labelTotalMonto;
        private System.Windows.Forms.Label labelTotalBauche;
    }
}