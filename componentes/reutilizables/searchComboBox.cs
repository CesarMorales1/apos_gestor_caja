using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace apos_gestor_caja.componentes.reutilizables
{
    [DefaultEvent("SelectedIndexChanged")]
    public class RegexFilterComboBox<T> : ComboBox
    {
        private List<T> _originalItems;
        private Func<T, string> _displaySelector;
        private Func<T, string, bool> _filterPredicate;
        private string _placeholderText;
        private Color _placeholderColor = Color.Gray;
        private bool _isFiltering;
        private bool _suppressTextUpdate;

        public RegexFilterComboBox()
        {
            this.DropDownStyle = ComboBoxStyle.DropDown;
            this.AutoCompleteMode = AutoCompleteMode.None;
            _originalItems = new List<T>();

            this.TextChanged += RegexFilterComboBox_TextChanged;
            this.GotFocus += RegexFilterComboBox_GotFocus;
            this.LostFocus += RegexFilterComboBox_LostFocus;
            this.SelectedIndexChanged += RegexFilterComboBox_SelectedIndexChanged;
        }

        [Category("Appearance")]
        [Description("Texto que se muestra cuando no hay selección")]
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value;
                if (string.IsNullOrEmpty(this.Text))
                {
                    this.Text = value;
                    this.ForeColor = _placeholderColor;
                }
            }
        }

        public void Configure(Func<T, string> displaySelector, Func<T, string, bool> filterPredicate)
        {
            _displaySelector = displaySelector ?? throw new ArgumentNullException(nameof(displaySelector));
            _filterPredicate = filterPredicate ?? throw new ArgumentNullException(nameof(filterPredicate));
        }

        public void SetItems(IEnumerable<T> items)
        {
            if (_displaySelector == null)
                throw new InvalidOperationException("Must call Configure before setting items.");

            _originalItems = items?.ToList() ?? new List<T>();
            UpdateItems(_originalItems);
        }

        private void UpdateItems(IEnumerable<T> items)
        {
            this.BeginUpdate();
            try
            {
                this.Items.Clear();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var comboItem = new ComboBoxItem<T>(item, _displaySelector(item));
                        this.Items.Add(comboItem);
                    }
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        private void RegexFilterComboBox_TextChanged(object sender, EventArgs e)
        {
            if (_isFiltering || _suppressTextUpdate || this.Text == _placeholderText) return;

            _isFiltering = true;
            try
            {
                string input = this.Text.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    UpdateItems(_originalItems);
                    return;
                }

                var filteredItems = _originalItems
                    .Where(item => _filterPredicate(item, input))
                    .ToList();

                UpdateItems(filteredItems);

                if (this.Items.Count > 0)
                {
                    this.DroppedDown = true;
                }
            }
            finally
            {
                _isFiltering = false;
            }
        }

        private void RegexFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedItem is ComboBoxItem<T> selected)
            {
                _suppressTextUpdate = true;
                try
                {
                    this.Text = selected.ToString();
                    this.ForeColor = Color.Black;
                }
                finally
                {
                    _suppressTextUpdate = false;
                }
            }
        }

        private void RegexFilterComboBox_GotFocus(object sender, EventArgs e)
        {
            if (this.Text == _placeholderText)
            {
                this.Text = string.Empty;
                this.ForeColor = Color.Black;
            }
        }

        private void RegexFilterComboBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                this.Text = _placeholderText;
                this.ForeColor = _placeholderColor;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter && this.Items.Count > 0)
            {
                this.SelectedIndex = 0;
                e.Handled = true;
            }
        }

        public T SelectedItemData
        {
            get
            {
                return this.SelectedItem is ComboBoxItem<T> selected ? selected.Item : default;
            }
        }

        private class ComboBoxItem<TItem>
        {
            public TItem Item { get; }
            private readonly string _display;

            public ComboBoxItem(TItem item, string display)
            {
                Item = item;
                _display = display;
            }

            public override string ToString()
            {
                return _display;
            }
        }
    }
}