namespace apos_gestor_caja.Forms
{
    partial class BancoForm
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.cajListadoGrid = new System.Windows.Forms.DataGridView();
            this.cajBusquedaCombo = new System.Windows.Forms.ComboBox();
            this.cajSearchTextBox = new System.Windows.Forms.TextBox();
            this.cajPanelEdicion = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cajBotonGuardar = new System.Windows.Forms.Button();
            this.botonEstado = new System.Windows.Forms.Button();
            this.cajBotonCancelar = new System.Windows.Forms.Button();
            this.cajLabelTitulo = new System.Windows.Forms.Label();
            this.cajInputNombre = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cajListadoGrid)).BeginInit();
            this.cajPanelEdicion.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.cajListadoGrid);
            this.splitContainer.Panel1.Controls.Add(this.cajBusquedaCombo);
            this.splitContainer.Panel1.Controls.Add(this.cajSearchTextBox);
            this.splitContainer.Panel1MinSize = 300;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.cajPanelEdicion);
            this.splitContainer.Panel2MinSize = 250;
            this.splitContainer.Size = new System.Drawing.Size(1704, 845);
            this.splitContainer.SplitterDistance = 1135;
            this.splitContainer.TabIndex = 0;
            // 
            // cajListadoGrid
            // 
            this.cajListadoGrid.AllowUserToAddRows = false;
            this.cajListadoGrid.AllowUserToDeleteRows = false;
            this.cajListadoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajListadoGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.cajListadoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cajListadoGrid.Location = new System.Drawing.Point(14, 49);
            this.cajListadoGrid.Name = "cajListadoGrid";
            this.cajListadoGrid.ReadOnly = true;
            this.cajListadoGrid.RowHeadersWidth = 62;
            this.cajListadoGrid.RowTemplate.Height = 28;
            this.cajListadoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cajListadoGrid.Size = new System.Drawing.Size(1107, 782);
            this.cajListadoGrid.TabIndex = 2;
            // 
            // cajBusquedaCombo
            // 
            this.cajBusquedaCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajBusquedaCombo.Location = new System.Drawing.Point(14, 46);
            this.cajBusquedaCombo.Name = "cajBusquedaCombo";
            this.cajBusquedaCombo.Size = new System.Drawing.Size(1105, 28);
            this.cajBusquedaCombo.TabIndex = 1;
            this.cajBusquedaCombo.Visible = false;
            // 
            // cajSearchTextBox
            // 
            this.cajSearchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajSearchTextBox.ForeColor = System.Drawing.Color.Gray;
            this.cajSearchTextBox.Location = new System.Drawing.Point(14, 12);
            this.cajSearchTextBox.Name = "cajSearchTextBox";
            this.cajSearchTextBox.Size = new System.Drawing.Size(1105, 26);
            this.cajSearchTextBox.TabIndex = 0;
            this.cajSearchTextBox.Text = "Buscar por nombre o #ID";
            // 
            // cajPanelEdicion
            // 
            this.cajPanelEdicion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajPanelEdicion.Controls.Add(this.tableLayoutPanel1);
            this.cajPanelEdicion.Controls.Add(this.cajLabelTitulo);
            this.cajPanelEdicion.Controls.Add(this.cajInputNombre);
            this.cajPanelEdicion.Location = new System.Drawing.Point(9, 15);
            this.cajPanelEdicion.Name = "cajPanelEdicion";
            this.cajPanelEdicion.Size = new System.Drawing.Size(551, 814);
            this.cajPanelEdicion.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.cajBotonGuardar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.botonEstado, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cajBotonCancelar, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 154);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(520, 62);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // cajBotonGuardar
            // 
            this.cajBotonGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajBotonGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(53)))), ((int)(((byte)(84)))));
            this.cajBotonGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cajBotonGuardar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cajBotonGuardar.ForeColor = System.Drawing.Color.White;
            this.cajBotonGuardar.Location = new System.Drawing.Point(3, 3);
            this.cajBotonGuardar.Name = "cajBotonGuardar";
            this.cajBotonGuardar.Size = new System.Drawing.Size(167, 56);
            this.cajBotonGuardar.TabIndex = 6;
            this.cajBotonGuardar.Text = "Guardar";
            this.cajBotonGuardar.UseVisualStyleBackColor = false;
            this.cajBotonGuardar.Click += new System.EventHandler(this.cajBotonGuardar_Click_1);
            // 
            // botonEstado
            // 
            this.botonEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.botonEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.botonEstado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botonEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.botonEstado.ForeColor = System.Drawing.Color.White;
            this.botonEstado.Location = new System.Drawing.Point(176, 3);
            this.botonEstado.Name = "botonEstado";
            this.botonEstado.Size = new System.Drawing.Size(167, 56);
            this.botonEstado.TabIndex = 9;
            this.botonEstado.Text = "Desactivar";
            this.botonEstado.UseVisualStyleBackColor = false;
            // 
            // cajBotonCancelar
            // 
            this.cajBotonCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajBotonCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.cajBotonCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cajBotonCancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cajBotonCancelar.ForeColor = System.Drawing.Color.White;
            this.cajBotonCancelar.Location = new System.Drawing.Point(349, 3);
            this.cajBotonCancelar.Name = "cajBotonCancelar";
            this.cajBotonCancelar.Size = new System.Drawing.Size(168, 56);
            this.cajBotonCancelar.TabIndex = 7;
            this.cajBotonCancelar.Text = "Cancelar";
            this.cajBotonCancelar.UseVisualStyleBackColor = false;
            // 
            // cajLabelTitulo
            // 
            this.cajLabelTitulo.AutoSize = true;
            this.cajLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.cajLabelTitulo.Location = new System.Drawing.Point(16, 15);
            this.cajLabelTitulo.Name = "cajLabelTitulo";
            this.cajLabelTitulo.Size = new System.Drawing.Size(229, 38);
            this.cajLabelTitulo.TabIndex = 0;
            this.cajLabelTitulo.Text = "Datos del Banco";
            // 
            // cajInputNombre
            // 
            this.cajInputNombre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajInputNombre.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cajInputNombre.ForeColor = System.Drawing.Color.Gray;
            this.cajInputNombre.Location = new System.Drawing.Point(16, 95);
            this.cajInputNombre.Name = "cajInputNombre";
            this.cajInputNombre.Size = new System.Drawing.Size(518, 34);
            this.cajInputNombre.TabIndex = 1;
            this.cajInputNombre.Text = "Nombre";
            // 
            // BancoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1704, 845);
            this.Controls.Add(this.splitContainer);
            this.MinimumSize = new System.Drawing.Size(1274, 661);
            this.Name = "BancoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Bancos";
            this.Load += new System.EventHandler(this.BancoForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cajListadoGrid)).EndInit();
            this.cajPanelEdicion.ResumeLayout(false);
            this.cajPanelEdicion.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView cajListadoGrid;
        private System.Windows.Forms.Panel cajPanelEdicion;
        private System.Windows.Forms.Label cajLabelTitulo;
        private System.Windows.Forms.TextBox cajInputNombre;
        private System.Windows.Forms.Button cajBotonGuardar;
        private System.Windows.Forms.Button cajBotonCancelar;
        private System.Windows.Forms.TextBox cajSearchTextBox;
        private System.Windows.Forms.ComboBox cajBusquedaCombo;
        private System.Windows.Forms.Button botonEstado;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private void BancoForm_Load(object sender, System.EventArgs e)
        {
            // Ajustar la posición del SplitContainer de acuerdo al tamaño de la ventana
            AjustarTamañoSplitContainer();

            // Suscribirse al evento de cambio de tamaño de la ventana
            this.Resize += new System.EventHandler(this.BancoForm_Resize);
        }

        private void BancoForm_Resize(object sender, System.EventArgs e)
        {
            AjustarTamañoSplitContainer();
        }

        private void AjustarTamañoSplitContainer()
        {
            // Ajustar el SplitContainer basado en el tamaño actual de la ventana
            int totalWidth = this.ClientSize.Width;

            // Si la ventana es pequeña, dar prioridad al panel de listado (panel1)
            if (totalWidth < 1000)
            {
                splitContainer.SplitterDistance = (int)(totalWidth * 0.65);
            }
            else
            {
                // En pantallas grandes, asignar un espacio más generoso al panel de edición
                splitContainer.SplitterDistance = (int)(totalWidth * 0.70);
            }
        }
    }
}