using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using SalesBookApp;
using apos_gestor_caja.applicationLayer.interfaces;
using apos_gestor_caja.ApplicationLayer.Services;
using apos_gestor_caja.Infrastructure.Repositories;
using apos_gestor_caja.Forms;
using System.Threading.Tasks;

namespace apos_gestor_caja.componentes
{
    public partial class principal : Form
    {
        private Panel permissionsPanel;
        private bool isPanelExpanded = false;
        private DataGridView dataGridViewSales;
        private readonly BancoService _bancoService;

        public principal()
        {
            InitializeComponent();
            this.BackColor = Color.WhiteSmoke;
            StyleMenuAndNavElements();
            _bancoService = new BancoService(new BancoRepository());

            // Creación del DataGridView programáticamente
            this.dataGridViewSales = new DataGridView();
            this.dataGridViewSales.ReadOnly = true;
            this.dataGridViewSales.Dock = DockStyle.Fill;
            this.dataGridViewSales.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dataGridViewSales.Location = new Point(0, navbar.Bottom);
            this.dataGridViewSales.Name = "dataGridViewSales";
            this.dataGridViewSales.Size = new Size(ClientSize.Width, ClientSize.Height - navbar.Bottom);
            this.dataGridViewSales.TabIndex = 0;
            this.dataGridViewSales.Visible = false;
            this.Controls.Add(this.dataGridViewSales);

            SetWatermarkBackground();

            // Añadiendo estilos al DataGridView
            dataGridViewSales.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dataGridViewSales.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridViewSales.DefaultCellStyle.ForeColor = Color.FromArgb(73, 80, 87);
            dataGridViewSales.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewSales.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridViewSales.BorderStyle = BorderStyle.None;
            dataGridViewSales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewSales.EnableHeadersVisualStyles = false;
            dataGridViewSales.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewSales.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            dataGridViewSales.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
            dataGridViewSales.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewSales.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridViewSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dataGridViewSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewSales.BackgroundColor = Color.WhiteSmoke;

            dataGridViewSales.CellMouseEnter += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    dataGridViewSales.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(220, 220, 220);
                }
            };
            dataGridViewSales.CellMouseLeave += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    dataGridViewSales.Rows[e.RowIndex].DefaultCellStyle.BackColor = dataGridViewSales.DefaultCellStyle.BackColor;
                }
            };
            this.Controls.Add(this.dataGridViewSales);

            // Suscribir el evento Resize para actualizar la marca de agua
            this.Resize += new EventHandler(Principal_Resize);

            // Habilitar captura de teclas para atajos
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(principal_KeyDown);
        }

        private void SetWatermarkBackground()
        {
            UpdateWatermarkBackground();
        }

        private void UpdateWatermarkBackground()
        {
            Image originalImage = Properties.Resources.logo3;
            if (originalImage == null) return;

            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height - navbar.Height;
            float aspectRatio = (float)originalImage.Width / originalImage.Height;

            int newWidth, newHeight;
            if (formWidth / aspectRatio <= formHeight)
            {
                newWidth = formWidth;
                newHeight = (int)(formWidth / aspectRatio);
            }
            else
            {
                newHeight = formHeight;
                newWidth = (int)(formHeight * aspectRatio);
            }

            Bitmap watermarkImage = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(watermarkImage))
            {
                ColorMatrix colorMatrix = new ColorMatrix();
                colorMatrix.Matrix33 = 0.3f;
                ImageAttributes imgAttributes = new ImageAttributes();
                imgAttributes.SetColorMatrix(colorMatrix);

                g.DrawImage(originalImage,
                    new Rectangle(0, 0, newWidth, newHeight),
                    0, 0, originalImage.Width, originalImage.Height,
                    GraphicsUnit.Pixel, imgAttributes);
            }

            this.BackgroundImage = watermarkImage;
            this.BackgroundImageLayout = ImageLayout.Center;
        }

        private void Principal_Resize(object sender, EventArgs e)
        {
            UpdateWatermarkBackground();
        }

        private void StyleMenuAndNavElements()
        {
            menuStrip1.BackColor = Color.White;
            menuStrip1.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            menuStrip1.ForeColor = Color.FromArgb(73, 80, 87);
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable());

            menuStrip1.Padding = new Padding(6, 2, 0, 2);

            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                StyleMenuItem(item);
            }
        }

        private class CustomColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected => Color.FromArgb(240, 240, 240);
            public override Color MenuItemBorder => Color.FromArgb(230, 230, 230);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(240, 240, 240);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(240, 240, 240);
            public override Color MenuBorder => Color.FromArgb(230, 230, 230);
            public override Color MenuItemPressedGradientBegin => Color.FromArgb(230, 230, 230);
            public override Color MenuItemPressedGradientEnd => Color.FromArgb(230, 230, 230);
        }

        private void StyleMenuItem(ToolStripMenuItem item)
        {
            item.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            foreach (ToolStripItem subItem in item.DropDownItems)
            {
                if (subItem is ToolStripMenuItem menuItem)
                {
                    menuItem.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                }
            }
        }

        private void principal_Load(object sender, EventArgs e) { }

        private void principal_FormClosing(object sender, FormClosedEventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Estás seguro de que quieres cerrar la aplicación?",
                "Confirmar cierre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No) return;
            Application.Exit();
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }

        private void subirVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (permissionsPanel != null && isPanelExpanded)
            {
                isPanelExpanded = false;
                AnimatePanel();
            }

            using (var fechaSubirVentas = new VentasForm())
            {
                fechaSubirVentas.ShowDialog();
            }
        }

        private void generarLibroDeVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (permissionsPanel != null && isPanelExpanded)
            {
                isPanelExpanded = false;
                AnimatePanel();
            }

            using (var salesBookForm = new SalesBookForm())
            {
                salesBookForm.ShowDialog();
            }
        }

        private void bancosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BancoForm formBanco = new BancoForm(_bancoService);
            formBanco.ShowDialog();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UsuariosForm usuariosForm = new UsuariosForm();
            usuariosForm.ShowDialog();
        }

        private void emisoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmisorForm formEmisor = new EmisorForm();
            formEmisor.ShowDialog();
        }

        private void cajerosMenuItem2_Click(object sender, EventArgs e)
        {
            CajeroForm cajeroForm = new CajeroForm();
            cajeroForm.ShowDialog();
        }

        private Panel CreateModulePanel(string moduleName, string[] permissions)
        {
            Panel modulePanel = new Panel
            {
                Width = 280,
                Height = 200,
                Margin = new Padding(50, 30, 50, 30),
                BackColor = Color.White
            };

            modulePanel.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, modulePanel.ClientRectangle,
                    Color.FromArgb(200, 200, 200), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(200, 200, 200), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(200, 200, 200), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(200, 200, 200), 1, ButtonBorderStyle.Solid);
            };

            Label titleLabel = new Label
            {
                Text = moduleName,
                Font = new Font("Segoe UI Semibold", 12),
                ForeColor = Color.FromArgb(64, 64, 64),
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            modulePanel.Controls.Add(titleLabel);

            FlowLayoutPanel permissionsContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(15, 10, 15, 10),
                AutoScroll = true
            };

            foreach (string permission in permissions)
            {
                Panel permissionRow = new Panel
                {
                    Width = 240,
                    Height = 35,
                    Margin = new Padding(0, 2, 0, 2)
                };

                CheckBox checkbox = new CheckBox
                {
                    Text = permission,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(64, 64, 64),
                    Location = new Point(5, 8),
                    AutoSize = true,
                    Cursor = Cursors.Hand
                };

                permissionRow.MouseEnter += (s, e) =>
                {
                    permissionRow.BackColor = Color.FromArgb(248, 249, 250);
                };
                permissionRow.MouseLeave += (s, e) =>
                {
                    permissionRow.BackColor = Color.Transparent;
                };

                permissionRow.Controls.Add(checkbox);
                permissionsContainer.Controls.Add(permissionRow);
            }

            modulePanel.Controls.Add(permissionsContainer);
            return modulePanel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (permissionsPanel != null)
            {
                isPanelExpanded = !isPanelExpanded;
                AnimatePanel();
                return;
            }

            permissionsPanel = new Panel
            {
                Location = new Point(0, navbar.Bottom),
                Width = ClientRectangle.Width,
                Height = 0,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 0, 20, 0)
            };

            headerPanel.Paint += (s, pe) =>
            {
                pe.Graphics.DrawLine(new Pen(Color.FromArgb(230, 230, 230)),
                    0, headerPanel.Height - 1,
                    headerPanel.Width, headerPanel.Height - 1);
            };

            Label headerLabel = new Label
            {
                Text = "Gestión de Permisos",
                Font = new Font("Segoe UI Semibold", 14),
                ForeColor = Color.FromArgb(52, 58, 64),
                AutoSize = true,
                Location = new Point(20, 17)
            };

            Button saveButton = new Button
            {
                Text = "Guardar Cambios",
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Size = new Size(130, 35),
                Location = new Point(permissionsPanel.Width - 150, 12),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.MouseEnter += (s, ev) =>
            {
                saveButton.BackColor = Color.FromArgb(0, 105, 217);
            };
            saveButton.MouseLeave += (s, ev) =>
            {
                saveButton.BackColor = Color.FromArgb(0, 123, 255);
            };

            saveButton.Click += (s, ev) =>
            {
                using (var successDialog = new Form())
                {
                    successDialog.Text = "Éxito";
                    successDialog.Size = new Size(300, 150);
                    successDialog.StartPosition = FormStartPosition.CenterParent;
                    successDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                    successDialog.MaximizeBox = false;
                    successDialog.MinimizeBox = false;

                    var messageLabel = new Label
                    {
                        Text = "Cambios guardados exitosamente",
                        AutoSize = true,
                        Location = new Point(20, 20),
                        Font = new Font("Segoe UI", 10)
                    };

                    var okButton = new Button
                    {
                        Text = "Aceptar",
                        DialogResult = DialogResult.OK,
                        Location = new Point(100, 70),
                        Size = new Size(100, 30),
                        BackColor = Color.FromArgb(0, 123, 255),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat
                    };
                    okButton.FlatAppearance.BorderSize = 0;

                    successDialog.Controls.Add(messageLabel);
                    successDialog.Controls.Add(okButton);
                    successDialog.ShowDialog(this);
                }
            };

            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(saveButton);
            permissionsPanel.Controls.Add(headerPanel);

            FlowLayoutPanel modulesContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            var moduleDefinitions = new Dictionary<string, string[]>
            {
                { "Banco", new[] { "Ver", "Editar", "Eliminar" } },
                { "Emisor", new[] { "Ver", "Editar", "Eliminar" } }
            };

            foreach (var module in moduleDefinitions)
            {
                modulesContainer.Controls.Add(CreateModulePanel(module.Key, module.Value));
            }

            permissionsPanel.Controls.Add(modulesContainer);
            Controls.Add(permissionsPanel);

            isPanelExpanded = true;
            AnimatePanel();
        }

        private async void AnimatePanel()
        {
            const int targetHeight = 500;
            const int animationSteps = 20;
            const int animationDelay = 10;

            if (isPanelExpanded)
            {
                for (int i = 0; i <= animationSteps; i++)
                {
                    permissionsPanel.Height = (i * targetHeight) / animationSteps;
                    await Task.Delay(animationDelay);
                }
            }
            else
            {
                for (int i = animationSteps; i >= 0; i--)
                {
                    permissionsPanel.Height = (i * targetHeight) / animationSteps;
                    await Task.Delay(animationDelay);
                }
            }
        }

        private void maestrasToolStripMenuItem_Click(object sender, EventArgs e) { }

        // Evento para manejar atajos de teclado
        private void principal_KeyDown(object sender, KeyEventArgs e)
        {
            // First-level menu shortcuts
            if (e.Control && e.KeyCode == Keys.P) // Control + P para Procesos
            {
                procesosToolStripMenuItem.ShowDropDown();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.M) // Control + M para Maestras
            {
                maestrasToolStripMenuItem.ShowDropDown();
                e.Handled = true;
            }
            // Global shortcuts for specific forms
            else if (e.Control && e.KeyCode == Keys.G) // Control + G para Generar libro de venta
            {
                generarLibroDeVentaToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.B) // Control + B para Bancos
            {
                bancosToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.E) // Control + E para Emisores
            {
                emisoresToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.U) // Control + U para Usuarios
            {
                usuariosToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.C) // Control + C para Cajeros
            {
                cajerosMenuItem2_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}