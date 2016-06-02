using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridComboBox
{
    public class AccGridComboBoxEditingControl : AccGridComboBox, IDataGridViewEditingControl
    {
        public AccGridComboBoxEditingControl()
        {
            SelectedValueChanged += SelectedValueChangedHandler;
            TabStop = false;
            // control must not be part of the tabbing loop ???
        }


        private DataGridView _dataGridView;
        public DataGridView EditingControlDataGridView
        {
            get { return _dataGridView; }
            set { _dataGridView = value; }
        }

        public object EditingControlFormattedValue
        {
            get { return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting); }
            set { this.SelectedItem = value; }
        }

        private int _rowIndex;
        public int EditingControlRowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        private bool _hasValueChanged = false;
        public bool EditingControlValueChanged
        {
            get { return _hasValueChanged; }
            set { _hasValueChanged = value; }
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        protected override bool DisposeToolStripDataGridView
        {
            get { return false; }
        }



        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.BackColor = dataGridViewCellStyle.BackColor;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
        }

        private void SelectedValueChangedHandler(object sender, EventArgs e)
        {
            if (!_hasValueChanged)
            {
                _hasValueChanged = true;
                _dataGridView.NotifyCurrentCellDirty(true);
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.Enter:
                case Keys.Escape:
                case Keys.Delete:
                    return true;
                default:
                    return false;
            }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Text;
        }


        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

    }
}
