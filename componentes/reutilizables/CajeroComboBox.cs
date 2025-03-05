using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.ApplicationLayer.Services;
using apos_gestor_caja.Helpers;
using apos_gestor_caja.Events;

namespace apos_gestor_caja.Components
{
    public class CajerosComboBox : ComboBox
    {
        private readonly CajeroService _cajeroService;
        private readonly DebounceHelper _debounceHelper;
        private bool _isLoading;
        private ToolTip _toolTip;
        private Label _loadingLabel;
        private Label _noResultsLabel;
        private bool _isSettingText = false;
        private string _searchText = string.Empty;

        public event EventHandler<CajeroSelectionEventArgs> CajeroSelected;

        public CajerosComboBox()
        {
            _cajeroService = new CajeroService();
            _debounceHelper = new DebounceHelper();
            InitializeComponent();
            SetupEventHandlers();
        }

        private void InitializeComponent()
        {
            DropDownStyle = ComboBoxStyle.DropDown;
            AutoCompleteMode = AutoCompleteMode.None;
            AutoCompleteSource = AutoCompleteSource.None;

            _toolTip = new ToolTip
            {
                InitialDelay = 500,
                ReshowDelay = 100,
                ShowAlways = true
            };

            _loadingLabel = new Label
            {
                Text = "Cargando...",
                AutoSize = true,
                Visible = false,
                ForeColor = Color.FromArgb(0, 102, 204)
            };

            _noResultsLabel = new Label
            {
                Text = "No se encontraron resultados",
                AutoSize = true,
                Visible = false,
                ForeColor = Color.FromArgb(192, 0, 0)
            };

            Parent?.Controls.Add(_loadingLabel);
            Parent?.Controls.Add(_noResultsLabel);
        }

        private void SetupEventHandlers()
        {
            TextChanged += async (s, e) =>
            {
                if (!_isSettingText)
                {
                    _searchText = Text;
                    // Usar el método Debounce de DebounceHelper
                    await _debounceHelper.Debounce(async () => await BuscarCajeros(), 300);
                }
            };

            KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    await BuscarCajeros();
                }
            };

            SelectedIndexChanged += OnSelectedIndexChanged;
            DrawItem += OnDrawItem;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                _loadingLabel.Location = new Point(0, Height + 5);
                _noResultsLabel.Location = new Point(0, Height + 5);
            }
        }

        private async Task BuscarCajeros()
        {
            SetLoadingState(true);
            try
            {
                List<Cajero> cajeros;
                if (string.IsNullOrWhiteSpace(_searchText))
                {
                    cajeros = await _cajeroService.ObtenerCajerosAsync();
                }
                else if (_searchText.StartsWith("#") && int.TryParse(_searchText.Substring(1), out int id))
                {
                    var cajero = await _cajeroService.ObtenerCajeroPorIdAsync(id);
                    cajeros = cajero != null ? new List<Cajero> { cajero } : new List<Cajero>();
                }
                else
                {
                    cajeros = await _cajeroService.BuscarCajerosPorUsuarioAsync(_searchText);
                }
                UpdateItems(cajeros);
            }
            catch (Exception ex)
            {
                ShowError("Error al buscar cajeros: " + ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedItem is Cajero selectedCajero)
            {
                CajeroSelected?.Invoke(this, new CajeroSelectionEventArgs(selectedCajero));
                _toolTip.SetToolTip(this, $"ID: {selectedCajero.Id}\nNivel: {selectedCajero.NivelAcceso}");
            }
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            if (Items[e.Index] is Cajero cajero)
            {
                var itemText = $"{cajero.Id} - {cajero.Usuario} ({cajero.Nombre})";
                var textColor = e.State.HasFlag(DrawItemState.Selected) ?
                    SystemColors.HighlightText :
                    SystemColors.WindowText;

                using (var brush = new SolidBrush(textColor))
                {
                    e.Graphics.DrawString(itemText, e.Font, brush, e.Bounds);
                }
            }

            e.DrawFocusRectangle();
        }

        public async Task LoadAllCajeros()
        {
            SetLoadingState(true);
            try
            {
                var cajeros = await _cajeroService.ObtenerCajerosAsync();
                UpdateItems(cajeros);
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ShowError("Error al cargar cajeros: " + ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        public void ClearSelection()
        {
            _isSettingText = true;
            Text = string.Empty;
            _searchText = string.Empty;
            SelectedIndex = -1;
            _isSettingText = false;
            DroppedDown = false;
        }

        private void UpdateItems(List<Cajero> cajeros)
        {
            Items.Clear();
            if (cajeros.Any())
            {
                Items.AddRange(cajeros.ToArray());
                _noResultsLabel.Visible = false;
                if (!string.IsNullOrWhiteSpace(_searchText))
                {
                    DroppedDown = true;
                }
            }
            else
            {
                _noResultsLabel.Visible = true;
            }
        }

        private void SetLoadingState(bool loading)
        {
            _isLoading = loading;
            _loadingLabel.Visible = loading;
            Enabled = !loading;

            if (loading)
            {
                _noResultsLabel.Visible = false;
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _toolTip?.Dispose();
                _loadingLabel?.Dispose();
                _noResultsLabel?.Dispose();
                _debounceHelper?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}