using System.Windows.Forms;
using apos_gestor_caja.Domain.Models;

namespace apos_gestor_caja.Extensions
{
    public static class ComboBoxExtensions
    {
        public static void SetSelectedCajero(this ComboBox comboBox, Cajero cajero)
        {
            if (cajero == null)
            {
                comboBox.SelectedIndex = -1;
                return;
            }

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i] is Cajero item && item.Id == cajero.Id)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        public static Cajero GetSelectedCajero(this ComboBox comboBox)
        {
            return comboBox.SelectedItem as Cajero;
        }
    }
}
