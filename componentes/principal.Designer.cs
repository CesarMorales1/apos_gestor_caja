namespace apos_gestor_caja.componentes
{
    partial class principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(principal));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.procesosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subirVentasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generarLibroDeVentaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maestrasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bancosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emisoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cajerosMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.navbar = new System.Windows.Forms.Panel();
            this.consultasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generarLibroDeVentasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cuadreDeCajaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.navbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.procesosToolStripMenuItem,
            this.maestrasToolStripMenuItem,
            this.consultasToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(7536, 33);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // procesosToolStripMenuItem
            // 
            this.procesosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subirVentasToolStripMenuItem,
            this.generarLibroDeVentaToolStripMenuItem,
            this.cuadreDeCajaToolStripMenuItem});
            this.procesosToolStripMenuItem.Name = "procesosToolStripMenuItem";
            this.procesosToolStripMenuItem.Size = new System.Drawing.Size(99, 29);
            this.procesosToolStripMenuItem.Text = "Procesos";
            // 
            // subirVentasToolStripMenuItem
            // 
            this.subirVentasToolStripMenuItem.Name = "subirVentasToolStripMenuItem";
            this.subirVentasToolStripMenuItem.Size = new System.Drawing.Size(289, 34);
            this.subirVentasToolStripMenuItem.Text = "Subir ventas";
            this.subirVentasToolStripMenuItem.Click += new System.EventHandler(this.subirVentasToolStripMenuItem_Click);
            // 
            // generarLibroDeVentaToolStripMenuItem
            // 
            this.generarLibroDeVentaToolStripMenuItem.Name = "generarLibroDeVentaToolStripMenuItem";
            this.generarLibroDeVentaToolStripMenuItem.Size = new System.Drawing.Size(289, 34);
            this.generarLibroDeVentaToolStripMenuItem.Text = "Generar libro de venta";
            this.generarLibroDeVentaToolStripMenuItem.Click += new System.EventHandler(this.generarLibroDeVentaToolStripMenuItem_Click);
            // 
            // maestrasToolStripMenuItem
            // 
            this.maestrasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bancosToolStripMenuItem,
            this.usuariosToolStripMenuItem,
            this.emisoresToolStripMenuItem,
            this.cajerosMenuItem2});
            this.maestrasToolStripMenuItem.Name = "maestrasToolStripMenuItem";
            this.maestrasToolStripMenuItem.Size = new System.Drawing.Size(99, 29);
            this.maestrasToolStripMenuItem.Text = "Maestras";
            this.maestrasToolStripMenuItem.Click += new System.EventHandler(this.maestrasToolStripMenuItem_Click);
            // 
            // bancosToolStripMenuItem
            // 
            this.bancosToolStripMenuItem.Name = "bancosToolStripMenuItem";
            this.bancosToolStripMenuItem.Size = new System.Drawing.Size(185, 34);
            this.bancosToolStripMenuItem.Text = "bancos";
            this.bancosToolStripMenuItem.Click += new System.EventHandler(this.bancosToolStripMenuItem_Click);
            // 
            // usuariosToolStripMenuItem
            // 
            this.usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            this.usuariosToolStripMenuItem.Size = new System.Drawing.Size(185, 34);
            this.usuariosToolStripMenuItem.Text = "usuarios";
            this.usuariosToolStripMenuItem.Click += new System.EventHandler(this.usuariosToolStripMenuItem_Click);
            // 
            // emisoresToolStripMenuItem
            // 
            this.emisoresToolStripMenuItem.Name = "emisoresToolStripMenuItem";
            this.emisoresToolStripMenuItem.Size = new System.Drawing.Size(185, 34);
            this.emisoresToolStripMenuItem.Text = "emisores";
            this.emisoresToolStripMenuItem.Click += new System.EventHandler(this.emisoresToolStripMenuItem_Click);
            // 
            // cajerosMenuItem2
            // 
            this.cajerosMenuItem2.Name = "cajerosMenuItem2";
            this.cajerosMenuItem2.Size = new System.Drawing.Size(185, 34);
            this.cajerosMenuItem2.Text = "cajeros";
            this.cajerosMenuItem2.Click += new System.EventHandler(this.cajerosMenuItem2_Click);
            // 
            // navbar
            // 
            this.navbar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.navbar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navbar.Controls.Add(this.menuStrip1);
            this.navbar.Location = new System.Drawing.Point(-1, 0);
            this.navbar.Name = "navbar";
            this.navbar.Size = new System.Drawing.Size(7536, 37);
            this.navbar.TabIndex = 1;
            this.navbar.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // consultasToolStripMenuItem
            // 
            this.consultasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generarLibroDeVentasToolStripMenuItem});
            this.consultasToolStripMenuItem.Name = "consultasToolStripMenuItem";
            this.consultasToolStripMenuItem.Size = new System.Drawing.Size(105, 29);
            this.consultasToolStripMenuItem.Text = "Consultas";
            // 
            // generarLibroDeVentasToolStripMenuItem
            // 
            this.generarLibroDeVentasToolStripMenuItem.Name = "generarLibroDeVentasToolStripMenuItem";
            this.generarLibroDeVentasToolStripMenuItem.Size = new System.Drawing.Size(297, 34);
            this.generarLibroDeVentasToolStripMenuItem.Text = "Generar libro de ventas";
            // 
            // cuadreDeCajaToolStripMenuItem
            // 
            this.cuadreDeCajaToolStripMenuItem.Name = "cuadreDeCajaToolStripMenuItem";
            this.cuadreDeCajaToolStripMenuItem.Size = new System.Drawing.Size(289, 34);
            this.cuadreDeCajaToolStripMenuItem.Text = "Cuadre de caja";
            // 
            // principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1928, 862);
            this.Controls.Add(this.navbar);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de gestion Geobit";
            this.Load += new System.EventHandler(this.principal_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.navbar.ResumeLayout(false);
            this.navbar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem procesosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subirVentasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generarLibroDeVentaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem maestrasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bancosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usuariosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emisoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cajerosMenuItem2;
        private System.Windows.Forms.Panel navbar;
        private System.Windows.Forms.ToolStripMenuItem consultasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generarLibroDeVentasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cuadreDeCajaToolStripMenuItem;
    }
}