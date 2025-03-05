namespace apos_gestor_caja.Forms
{
    partial class CajeroForm
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
            this.botonEstado = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.closedEye = new System.Windows.Forms.Button();
            this.cajLabelTitulo = new System.Windows.Forms.Label();
            this.cajInputUsuario = new System.Windows.Forms.TextBox();
            this.cajInputClave = new System.Windows.Forms.TextBox();
            this.cajInputNombre = new System.Windows.Forms.TextBox();
            this.cajComboNivel = new System.Windows.Forms.ComboBox();
            this.cajBotonGuardar = new System.Windows.Forms.Button();
            this.cajBotonCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cajListadoGrid)).BeginInit();
            this.cajPanelEdicion.SuspendLayout();
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
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.cajPanelEdicion);
            this.splitContainer.Size = new System.Drawing.Size(1704, 844);
            this.splitContainer.SplitterDistance = 1136;
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
            this.cajListadoGrid.Location = new System.Drawing.Point(14, 50);
            this.cajListadoGrid.Name = "cajListadoGrid";
            this.cajListadoGrid.ReadOnly = true;
            this.cajListadoGrid.RowHeadersWidth = 62;
            this.cajListadoGrid.RowTemplate.Height = 28;
            this.cajListadoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cajListadoGrid.Size = new System.Drawing.Size(1109, 782);
            this.cajListadoGrid.TabIndex = 2;
            this.cajListadoGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cajListadoGrid_CellContentClick);
            // 
            // cajBusquedaCombo
            // 
            this.cajBusquedaCombo.Location = new System.Drawing.Point(14, 46);
            this.cajBusquedaCombo.Name = "cajBusquedaCombo";
            this.cajBusquedaCombo.Size = new System.Drawing.Size(872, 28);
            this.cajBusquedaCombo.TabIndex = 1;
            this.cajBusquedaCombo.Visible = false;
            // 
            // cajSearchTextBox
            // 
            this.cajSearchTextBox.ForeColor = System.Drawing.Color.Gray;
            this.cajSearchTextBox.Location = new System.Drawing.Point(14, 12);
            this.cajSearchTextBox.Name = "cajSearchTextBox";
            this.cajSearchTextBox.Size = new System.Drawing.Size(1109, 26);
            this.cajSearchTextBox.TabIndex = 0;
            this.cajSearchTextBox.Text = "Buscar por usuario o #ID";
            // 
            // cajPanelEdicion
            // 
            this.cajPanelEdicion.Controls.Add(this.botonEstado);
            this.cajPanelEdicion.Controls.Add(this.textBox1);
            this.cajPanelEdicion.Controls.Add(this.closedEye);
            this.cajPanelEdicion.Controls.Add(this.cajLabelTitulo);
            this.cajPanelEdicion.Controls.Add(this.cajInputUsuario);
            this.cajPanelEdicion.Controls.Add(this.cajInputClave);
            this.cajPanelEdicion.Controls.Add(this.cajInputNombre);
            this.cajPanelEdicion.Controls.Add(this.cajComboNivel);
            this.cajPanelEdicion.Controls.Add(this.cajBotonGuardar);
            this.cajPanelEdicion.Controls.Add(this.cajBotonCancelar);
            this.cajPanelEdicion.Location = new System.Drawing.Point(9, 16);
            this.cajPanelEdicion.Name = "cajPanelEdicion";
            this.cajPanelEdicion.Size = new System.Drawing.Size(549, 901);
            this.cajPanelEdicion.TabIndex = 0;
            // 
            // botonEstado
            // 
            this.botonEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.botonEstado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botonEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.botonEstado.ForeColor = System.Drawing.Color.White;
            this.botonEstado.Location = new System.Drawing.Point(213, 380);
            this.botonEstado.Name = "botonEstado";
            this.botonEstado.Size = new System.Drawing.Size(124, 47);
            this.botonEstado.TabIndex = 9;
            this.botonEstado.Text = "Desactivar";
            this.botonEstado.UseVisualStyleBackColor = false;
            this.botonEstado.Click += new System.EventHandler(this.BotonEstado_click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBox1.ForeColor = System.Drawing.Color.Gray;
            this.textBox1.Location = new System.Drawing.Point(17, 316);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(516, 34);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "Barra de 6 pa arriba";
            // 
            // closedEye
            // 
            this.closedEye.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.closedEye.BackgroundImage = global::apos_gestor_caja.Properties.Resources.Closed_Eye;
            this.closedEye.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.closedEye.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closedEye.Location = new System.Drawing.Point(491, 147);
            this.closedEye.Name = "closedEye";
            this.closedEye.Size = new System.Drawing.Size(42, 36);
            this.closedEye.TabIndex = 3;
            this.closedEye.UseVisualStyleBackColor = false;
            this.closedEye.Click += new System.EventHandler(this.Closed_eye_click);
            // 
            // cajLabelTitulo
            // 
            this.cajLabelTitulo.AutoSize = true;
            this.cajLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.cajLabelTitulo.Location = new System.Drawing.Point(17, 16);
            this.cajLabelTitulo.Name = "cajLabelTitulo";
            this.cajLabelTitulo.Size = new System.Drawing.Size(232, 38);
            this.cajLabelTitulo.TabIndex = 0;
            this.cajLabelTitulo.Text = "Datos del Cajero";
            // 
            // cajInputUsuario
            // 
            this.cajInputUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cajInputUsuario.ForeColor = System.Drawing.Color.Gray;
            this.cajInputUsuario.Location = new System.Drawing.Point(17, 96);
            this.cajInputUsuario.Name = "cajInputUsuario";
            this.cajInputUsuario.Size = new System.Drawing.Size(516, 34);
            this.cajInputUsuario.TabIndex = 1;
            this.cajInputUsuario.Text = "Usuario";
            // 
            // cajInputClave
            // 
            this.cajInputClave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cajInputClave.ForeColor = System.Drawing.Color.Gray;
            this.cajInputClave.Location = new System.Drawing.Point(17, 149);
            this.cajInputClave.Name = "cajInputClave";
            this.cajInputClave.PasswordChar = '*';
            this.cajInputClave.Size = new System.Drawing.Size(516, 34);
            this.cajInputClave.TabIndex = 2;
            this.cajInputClave.Text = "Clave";
            // 
            // cajInputNombre
            // 
            this.cajInputNombre.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cajInputNombre.ForeColor = System.Drawing.Color.Gray;
            this.cajInputNombre.Location = new System.Drawing.Point(17, 203);
            this.cajInputNombre.Name = "cajInputNombre";
            this.cajInputNombre.Size = new System.Drawing.Size(516, 34);
            this.cajInputNombre.TabIndex = 3;
            this.cajInputNombre.Text = "Nombre";
            // 
            // cajComboNivel
            // 
            this.cajComboNivel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cajComboNivel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cajComboNivel.FormattingEnabled = true;
            this.cajComboNivel.Location = new System.Drawing.Point(17, 256);
            this.cajComboNivel.Name = "cajComboNivel";
            this.cajComboNivel.Size = new System.Drawing.Size(516, 36);
            this.cajComboNivel.TabIndex = 4;
            // 
            // cajBotonGuardar
            // 
            this.cajBotonGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(53)))), ((int)(((byte)(84)))));
            this.cajBotonGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cajBotonGuardar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cajBotonGuardar.ForeColor = System.Drawing.Color.White;
            this.cajBotonGuardar.Location = new System.Drawing.Point(24, 380);
            this.cajBotonGuardar.Name = "cajBotonGuardar";
            this.cajBotonGuardar.Size = new System.Drawing.Size(124, 47);
            this.cajBotonGuardar.TabIndex = 6;
            this.cajBotonGuardar.Text = "Guardar";
            this.cajBotonGuardar.UseVisualStyleBackColor = false;
            // 
            // cajBotonCancelar
            // 
            this.cajBotonCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.cajBotonCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cajBotonCancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cajBotonCancelar.ForeColor = System.Drawing.Color.White;
            this.cajBotonCancelar.Location = new System.Drawing.Point(411, 380);
            this.cajBotonCancelar.Name = "cajBotonCancelar";
            this.cajBotonCancelar.Size = new System.Drawing.Size(122, 47);
            this.cajBotonCancelar.TabIndex = 7;
            this.cajBotonCancelar.Text = "Cancelar";
            this.cajBotonCancelar.UseVisualStyleBackColor = false;
            this.cajBotonCancelar.Click += new System.EventHandler(this.CajBotonCancelar_Click);
            // 
            // CajeroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1704, 844);
            this.Controls.Add(this.splitContainer);
            this.MinimumSize = new System.Drawing.Size(1726, 900);
            this.Name = "CajeroForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Cajeros";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cajListadoGrid)).EndInit();
            this.cajPanelEdicion.ResumeLayout(false);
            this.cajPanelEdicion.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView cajListadoGrid;
        private System.Windows.Forms.Panel cajPanelEdicion;
        private System.Windows.Forms.Label cajLabelTitulo;
        private System.Windows.Forms.TextBox cajInputUsuario;
        private System.Windows.Forms.TextBox cajInputClave;
        private System.Windows.Forms.TextBox cajInputNombre;
        private System.Windows.Forms.ComboBox cajComboNivel;
        private System.Windows.Forms.Button cajBotonGuardar;
        private System.Windows.Forms.Button cajBotonCancelar;
        private System.Windows.Forms.TextBox cajSearchTextBox;
        private System.Windows.Forms.ComboBox cajBusquedaCombo;
        private System.Windows.Forms.Button closedEye;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button botonEstado;
    }
}