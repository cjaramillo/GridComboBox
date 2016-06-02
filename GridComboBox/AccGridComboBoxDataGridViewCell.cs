using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridComboBox
{
    public class AccGridComboBoxDataGridViewCell : DataGridViewTextBoxCell
    {
        internal AccGridComboBox EditingAccGridComboBox
        {
            get { return this.DataGridView.EditingControl as AccGridComboBoxEditingControl; }
        }

        public override Type EditType
        {
            get { return typeof(AccGridComboBoxEditingControl); }
        }

        public override Type ValueType
        {
            get { return typeof(object); }
        }


        public override void InitializeEditingControl(int nRowIndex, object nInitialFormattedValue, DataGridViewCellStyle nDataGridViewCellStyle)
        {
            base.InitializeEditingControl(nRowIndex, nInitialFormattedValue, nDataGridViewCellStyle);
            AccGridComboBox cEditBox = this.DataGridView.EditingControl as AccGridComboBox;
            if (cEditBox != null)
            {
                if ((base.OwningColumn != null) && (((DataGridViewAccGridComboBoxColumn)base.OwningColumn).ComboDataGridView != null))
                {
                    cEditBox.AddToolStripDataGridView(((DataGridViewAccGridComboBoxColumn)base.OwningColumn).GetToolStripDataGridView());
                    cEditBox.ValueMember = ((DataGridViewAccGridComboBoxColumn)base.OwningColumn).ValueMember;
                    cEditBox.InstantBinding = ((DataGridViewAccGridComboBoxColumn)base.OwningColumn).InstantBinding;
                }
                try
                {
                    cEditBox.SelectedValue = Value;
                }
                catch (Exception ex)
                {
                    cEditBox.SelectedValue = null;
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void DetachEditingControl()
        {
            DataGridView cDataGridView = this.DataGridView;
            if (cDataGridView == null || cDataGridView.EditingControl == null)
            {
                throw new InvalidOperationException("Cell is detached or its grid has no editing control.");
            }

            AccGridComboBox EditBox = cDataGridView.EditingControl as AccGridComboBox;
            if (EditBox != null)
            {
                // avoid interferences between the editing sessions
                //EditBox.ClearUndo()
            }

            base.DetachEditingControl();
        }

        public override string ToString()
        {
            return "DataGridViewAccGridComboBoxCell{ColIndex=" + this.ColumnIndex.ToString() + ", RowIndex=" + this.RowIndex.ToString() + "}";
        }

        private void OnCommonChange()
        {
            if ((this.DataGridView != null) && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);
                }
                else
                {
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }

        private bool OwnsEditingControl(int nRowIndex)
        {
            if (nRowIndex == -1 || this.DataGridView == null || this.DataGridView.IsDisposed || this.DataGridView.Disposing)
                return false;

            AccGridComboBoxEditingControl cEditingControl = this.DataGridView.EditingControl as AccGridComboBoxEditingControl;
            return (cEditingControl != null && nRowIndex == ((IDataGridViewEditingControl)cEditingControl).EditingControlRowIndex);
        }

        protected override bool SetValue(int rowIndex, object value)
        {
            if ((this.DataGridView != null) && (this.DataGridView.EditingControl != null) && this.DataGridView.EditingControl is AccGridComboBox)
            {
                return base.SetValue(rowIndex, ((AccGridComboBox)this.DataGridView.EditingControl).SelectedValue);
            }
            else
            {
                return base.SetValue(rowIndex, value);
            }
        }

    }
}
