using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaborProject
{
    public partial class PenSettingsForm : Form
    {
        private Pen selectedPen;

        private ComboBox styleComboBox;

        public PenSettingsForm(Pen currentPen)
        {
            InitializeComponent();

            selectedPen = currentPen;

            Label styleLabel = new Label();
            styleLabel.Text = "Стиль пера:";
            styleLabel.Location = new Point(10, 10);
            Controls.Add(styleLabel);

            styleComboBox = new ComboBox();
            styleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            styleComboBox.Items.Add("Solid");
            styleComboBox.Items.Add("Dot");
            styleComboBox.Items.Add("DashDotDot");
            styleComboBox.SelectedIndex = styleComboBox.Items.IndexOf(Enum.GetName(typeof(DashStyle), currentPen.DashStyle));
            styleComboBox.Location = new Point(120, 10);
            Controls.Add(styleComboBox);

            Button applyButton = new Button();
            applyButton.Text = "Применить";
            applyButton.Location = new Point(10, 40);
            applyButton.Click += ApplyButton_Click;
            Controls.Add(applyButton);

        }
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            selectedPen.Color = Color.Black; // Вы можете использовать другой способ выбора цвета.
            selectedPen.Width = 2; // Вы можете использовать другой способ выбора толщины.

            if (styleComboBox.SelectedItem != null)
            {
                selectedPen.DashStyle = (DashStyle)Enum.Parse(typeof(DashStyle), styleComboBox.SelectedItem.ToString());
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        public Pen SelectedPen
        {
            get { return selectedPen; }
        }
    }
}
