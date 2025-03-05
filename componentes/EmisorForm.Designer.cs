namespace apos_gestor_caja.Forms
{
    partial class EmisorForm
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
            this.emisorListadoGrid = new System.Windows.Forms.DataGridView();
            this.emisorBusquedaCombo = new System.Windows.Forms.ComboBox();
            this.emisorSearchTextBox = new System.Windows.Forms.TextBox();
            this.emisorPanelEdicion = new System.Windows.Forms.Panel();
            this.botonEstado = new System.Windows.Forms.Button();
            this.emisorLabelTitulo = new System.Windows.Forms.Label();
            this.emisorInputNombre = new System.Windows.Forms.TextBox();
            this.emisorBotonGuardar = new System.Windows.Forms.Button();
            this.emisorBotonCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.emisorListadoGrid)).BeginInit();
            this.emisorPanelEdicion.SuspendLayout();
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
            this.splitContainer.Panel1.Controls.Add(this.emisorListadoGrid);
            this.splitContainer.Panel1.Controls.Add(this.emisorBusquedaCombo);
            this.splitContainer.Panel1.Controls.Add(this.emisorSearchTextBox);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.emisorPanelEdicion);
            this.splitContainer.Size = new System.Drawing.Size(1704, 844);
            this.splitContainer.SplitterDistance = 1136;
            this.splitContainer.TabIndex = 0;
            // 
            // emisorListadoGrid
            // 
            this.emisorListadoGrid.AllowUserToAddRows = false;
            this.emisorListadoGrid.AllowUserToDeleteRows = false;
            this.emisorListadoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emisorListadoGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.emisorListadoGrid.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.emisorListadoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.emisorListadoGrid.Location = new System.Drawing.Point(14, 50);
            this.emisorListadoGrid.Name = "emisorListadoGrid";
            this.emisorListadoGrid.ReadOnly = true;
            this.emisorListadoGrid.RowHeadersWidth = 62;
            this.emisorListadoGrid.RowTemplate.Height = 28;
            this.emisorListadoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.emisorListadoGrid.Size = new System.Drawing.Size(1109, 782);
            this.emisorListadoGrid.TabIndex = 2;
            // 
            // emisorBusquedaCombo
            // 
            this.emisorBusquedaCombo.Location = new System.Drawing.Point(14, 46);
            this.emisorBusquedaCombo.Name = "emisorBusquedaCombo";
            this.emisorBusquedaCombo.Size = new System.Drawing.Size(872, 28);
            this.emisorBusquedaCombo.TabIndex = 1;
            this.emisorBusquedaCombo.Visible = false;
            // 
            // emisorSearchTextBox
            // 
            this.emisorSearchTextBox.ForeColor = System.Drawing.Color.Gray;
            this.emisorSearchTextBox.Location = new System.Drawing.Point(14, 12);
            this.emisorSearchTextBox.Name = "emisorSearchTextBox";
            this.emisorSearchTextBox.Size = new System.Drawing.Size(1109, 26);
            this.emisorSearchTextBox.TabIndex = 0;
            this.emisorSearchTextBox.Text = "Buscar por nombre o #ID";
            // 
            // emisorPanelEdicion
            // 
            this.emisorPanelEdicion.Controls.Add(this.botonEstado);
            this.emisorPanelEdicion.Controls.Add(this.emisorLabelTitulo);
            this.emisorPanelEdicion.Controls.Add(this.emisorInputNombre);
            this.emisorPanelEdicion.Controls.Add(this.emisorBotonGuardar);
            this.emisorPanelEdicion.Controls.Add(this.emisorBotonCancelar);
            this.emisorPanelEdicion.Location = new System.Drawing.Point(9, 16);
            this.emisorPanelEdicion.Name = "emisorPanelEdicion";
            this.emisorPanelEdicion.Size = new System.Drawing.Size(549, 901);
            this.emisorPanelEdicion.TabIndex = 0;
            // 
            // botonEstado
            // 
            this.botonEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.botonEstado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botonEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.botonEstado.ForeColor = System.Drawing.Color.White;
            this.botonEstado.Location = new System.Drawing.Point(218, 220);
            this.botonEstado.Name = "botonEstado";
            this.botonEstado.Size = new System.Drawing.Size(124, 47);
            this.botonEstado.TabIndex = 9;
            this.botonEstado.Text = "Desactivar";
            this.botonEstado.UseVisualStyleBackColor = false;
            // 
            // emisorLabelTitulo
            // 
            this.emisorLabelTitulo.AutoSize = true;
            this.emisorLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.emisorLabelTitulo.Location = new System.Drawing.Point(17, 16);
            this.emisorLabelTitulo.Name = "emisorLabelTitulo";
            this.emisorLabelTitulo.Size = new System.Drawing.Size(238, 38);
            this.emisorLabelTitulo.TabIndex = 0;
            this.emisorLabelTitulo.Text = "Datos del Emisor";
            // 
            // emisorInputNombre
            // 
            this.emisorInputNombre.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.emisorInputNombre.ForeColor = System.Drawing.Color.Gray;
            this.emisorInputNombre.Location = new System.Drawing.Point(17, 96);
            this.emisorInputNombre.Name = "emisorInputNombre";
            this.emisorInputNombre.Size = new System.Drawing.Size(516, 34);
            this.emisorInputNombre.TabIndex = 1;
            this.emisorInputNombre.Text = "Nombre";
            // 
            // emisorBotonGuardar
            // 
            this.emisorBotonGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(53)))), ((int)(((byte)(84)))));
            this.emisorBotonGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emisorBotonGuardar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.emisorBotonGuardar.ForeColor = System.Drawing.Color.White;
            this.emisorBotonGuardar.Location = new System.Drawing.Point(24, 220);
            this.emisorBotonGuardar.Name = "emisorBotonGuardar";
            this.emisorBotonGuardar.Size = new System.Drawing.Size(124, 47);
            this.emisorBotonGuardar.TabIndex = 6;
            this.emisorBotonGuardar.Text = "Guardar";
            this.emisorBotonGuardar.UseVisualStyleBackColor = false;
            // 
            // emisorBotonCancelar
            // 
            this.emisorBotonCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.emisorBotonCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emisorBotonCancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.emisorBotonCancelar.ForeColor = System.Drawing.Color.White;
            this.emisorBotonCancelar.Location = new System.Drawing.Point(411, 220);
            this.emisorBotonCancelar.Name = "emisorBotonCancelar";
            this.emisorBotonCancelar.Size = new System.Drawing.Size(122, 47);
            this.emisorBotonCancelar.TabIndex = 7;
            this.emisorBotonCancelar.Text = "Cancelar";
            this.emisorBotonCancelar.UseVisualStyleBackColor = false;
            // 
            // EmisorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1704, 844);
            this.Controls.Add(this.splitContainer);
            this.MinimumSize = new System.Drawing.Size(1726, 900);
            this.Name = "EmisorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Emisores";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.emisorListadoGrid)).EndInit();
            this.emisorPanelEdicion.ResumeLayout(false);
            this.emisorPanelEdicion.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView emisorListadoGrid;
        private System.Windows.Forms.Panel emisorPanelEdicion;
        private System.Windows.Forms.Label emisorLabelTitulo;
        private System.Windows.Forms.TextBox emisorInputNombre;
        private System.Windows.Forms.Button emisorBotonGuardar;
        private System.Windows.Forms.Button emisorBotonCancelar;
        private System.Windows.Forms.TextBox emisorSearchTextBox;
        private System.Windows.Forms.ComboBox emisorBusquedaCombo;
        private System.Windows.Forms.Button botonEstado;
    }
}
