using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridComboBox
{
    public class DataGridViewAccGridComboBoxColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewAccGridComboBoxColumn()
        {
            AccGridComboBoxDataGridViewCell cell = new AccGridComboBoxDataGridViewCell();
            base.CellTemplate = cell;
            base.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private AccGridComboBoxDataGridViewCell AccGridComboBoxDataGridViewCellTemplate
        {
            get
            {
                AccGridComboBoxDataGridViewCell cell = this.CellTemplate as AccGridComboBoxDataGridViewCell;
                if (cell == null)
                    throw new InvalidOperationException("DataGridViewAccGridComboBoxColumn does not have a CellTemplate.");
                return cell;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Windows.Forms.DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                AccGridComboBoxDataGridViewCell cell = value as AccGridComboBoxDataGridViewCell;
                if ((value != null) && cell == null)
                    throw new InvalidCastException("Value provided for CellTemplate must be of type DataGridViewAccTextBoxCell or derive from it.");
                base.CellTemplate = value;
            }
        }

        private ToolStripDataGridView myDataGridView = null;
        public DataGridView ComboDataGridView
        {
            get
            {
                if ((myDataGridView != null))
                    return myDataGridView.DataGridViewControl;
                return null;
            }
            set
            {
                if ((value != null))
                {
                    myDataGridView = new ToolStripDataGridView(value, _CloseOnSingleClick);
                }
                else
                {
                    myDataGridView = null;
                }
            }
        }

        private string _ValueMember = "";
        public string ValueMember
        {
            get { return _ValueMember; }
            set { _ValueMember = value; }
        }

        private bool _CloseOnSingleClick = true;
        public bool CloseOnSingleClick
        {
            get { return _CloseOnSingleClick; }
            set
            {
                _CloseOnSingleClick = value;
                if ((myDataGridView != null))
                    myDataGridView.CloseOnSingleClick = value;
            }
        }

        private bool _InstantBinding = true;
        public bool InstantBinding
        {
            get { return _InstantBinding; }
            set { _InstantBinding = value; }
        }

        public override string ToString()
        {
            return "DataGridViewAccGridComboBoxColumn{Name=" + this.Name + ", Index=" + this.Index.ToString() + "}";
        }

        internal ToolStripDataGridView GetToolStripDataGridView()
        {
            return myDataGridView;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if ((myDataGridView != null) && (myDataGridView.DataGridViewControl != null) && !myDataGridView.DataGridViewControl.IsDisposed)
                    myDataGridView.DataGridViewControl.Dispose();
                if ((myDataGridView != null) && !myDataGridView.IsDisposed)
                    myDataGridView.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
