namespace apos_gestor_caja.componentes
{
    partial class UsuariosForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.cajListadoGrid = new System.Windows.Forms.DataGridView();
            this.cajBusquedaCombo = new System.Windows.Forms.ComboBox();
            this.cajSearchTextBox = new System.Windows.Forms.TextBox();
            this.cajPanelEdicion = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ActivoStatus = new System.Windows.Forms.ComboBox();
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
            this.splitContainer.Location = new System.Drawing.Point(10, 10);
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
            this.splitContainer.Size = new System.Drawing.Size(1570, 877);
            this.splitContainer.SplitterDistance = 1045;
            this.splitContainer.TabIndex = 1;
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
            this.cajListadoGrid.Size = new System.Drawing.Size(1017, 814);
            this.cajListadoGrid.TabIndex = 2;
            // 
            // cajBusquedaCombo
            // 
            this.cajBusquedaCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajBusquedaCombo.Location = new System.Drawing.Point(14, 46);
            this.cajBusquedaCombo.Name = "cajBusquedaCombo";
            this.cajBusquedaCombo.Size = new System.Drawing.Size(1015, 28);
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
            this.cajSearchTextBox.Size = new System.Drawing.Size(1015, 26);
            this.cajSearchTextBox.TabIndex = 0;
            this.cajSearchTextBox.Text = "Buscar por nombre o #ID";
            // 
            // cajPanelEdicion
            // 
            this.cajPanelEdicion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajPanelEdicion.Controls.Add(this.textBox2);
            this.cajPanelEdicion.Controls.Add(this.textBox1);
            this.cajPanelEdicion.Controls.Add(this.ActivoStatus);
            this.cajPanelEdicion.Controls.Add(this.tableLayoutPanel1);
            this.cajPanelEdicion.Controls.Add(this.cajLabelTitulo);
            this.cajPanelEdicion.Controls.Add(this.cajInputNombre);
            this.cajPanelEdicion.Location = new System.Drawing.Point(9, 15);
            this.cajPanelEdicion.Name = "cajPanelEdicion";
            this.cajPanelEdicion.Size = new System.Drawing.Size(507, 846);
            this.cajPanelEdicion.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBox2.ForeColor = System.Drawing.Color.Gray;
            this.textBox2.Location = new System.Drawing.Point(16, 206);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(474, 34);
            this.textBox2.TabIndex = 13;
            this.textBox2.Text = "Password";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBox1.ForeColor = System.Drawing.Color.Gray;
            this.textBox1.Location = new System.Drawing.Point(16, 148);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(474, 34);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "Usuario";
            // 
            // ActivoStatus
            // 
            this.ActivoStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ActivoStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ActivoStatus.FormattingEnabled = true;
            this.ActivoStatus.Items.AddRange(new object[] {
            "activo",
            "inactivo"});
            this.ActivoStatus.Location = new System.Drawing.Point(16, 259);
            this.ActivoStatus.Name = "ActivoStatus";
            this.ActivoStatus.Size = new System.Drawing.Size(474, 36);
            this.ActivoStatus.TabIndex = 11;
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 314);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(476, 62);
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
            this.cajBotonGuardar.Size = new System.Drawing.Size(152, 56);
            this.cajBotonGuardar.TabIndex = 6;
            this.cajBotonGuardar.Text = "Guardar";
            this.cajBotonGuardar.UseVisualStyleBackColor = false;
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
            this.botonEstado.Location = new System.Drawing.Point(161, 3);
            this.botonEstado.Name = "botonEstado";
            this.botonEstado.Size = new System.Drawing.Size(152, 56);
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
            this.cajBotonCancelar.Location = new System.Drawing.Point(319, 3);
            this.cajBotonCancelar.Name = "cajBotonCancelar";
            this.cajBotonCancelar.Size = new System.Drawing.Size(154, 56);
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
            this.cajLabelTitulo.Size = new System.Drawing.Size(249, 38);
            this.cajLabelTitulo.TabIndex = 0;
            this.cajLabelTitulo.Text = "Datos del Usuario";
            this.cajLabelTitulo.Click += new System.EventHandler(this.cajLabelTitulo_Click);
            // 
            // cajInputNombre
            // 
            this.cajInputNombre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cajInputNombre.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cajInputNombre.ForeColor = System.Drawing.Color.Gray;
            this.cajInputNombre.Location = new System.Drawing.Point(16, 95);
            this.cajInputNombre.Name = "cajInputNombre";
            this.cajInputNombre.Size = new System.Drawing.Size(474, 34);
            this.cajInputNombre.TabIndex = 1;
            this.cajInputNombre.Text = "Nombre";
            this.cajInputNombre.TextChanged += new System.EventHandler(this.cajInputNombre_TextChanged);
            this.cajInputNombre.Enter += new System.EventHandler(this.cajInputNombre_Enter);
            // 
            // UsuariosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1590, 897);
            this.Controls.Add(this.splitContainer);
            this.Name = "UsuariosForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.UsuariosForm_Load);
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

        #endregion

        private void ApplyModernStyle()
        {
            // Configuración del formulario
            this.BackColor = System.Drawing.Color.FromArgb(240, 244, 248);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;







        }

        private void DrawBorder(object sender, System.Windows.Forms.PaintEventArgs e, System.Drawing.Color borderColor)
        {
            System.Windows.Forms.Control control = sender as System.Windows.Forms.Control;
            if (control != null)
            {
                using (System.Drawing.Pen pen = new System.Drawing.Pen(borderColor, 2))
                {
                    e.Graphics.DrawRectangle(pen, new System.Drawing.Rectangle(0, 0, control.Width - 1, control.Height - 1));
                }
            }
        }

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView cajListadoGrid;
        private System.Windows.Forms.ComboBox cajBusquedaCombo;
        private System.Windows.Forms.TextBox cajSearchTextBox;
        private System.Windows.Forms.Panel cajPanelEdicion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button cajBotonGuardar;
        private System.Windows.Forms.Button botonEstado;
        private System.Windows.Forms.Button cajBotonCancelar;
        private System.Windows.Forms.Label cajLabelTitulo;
        private System.Windows.Forms.ComboBox ActivoStatus;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox cajInputNombre;
    }
}