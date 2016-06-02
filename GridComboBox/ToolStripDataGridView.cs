using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace GridComboBox
{
    public class ToolStripDataGridView : ToolStripControlHost
    {

        private bool _CloseOnSingleClick = true;
        private int _MinDropDownWidth;

        private int _DropDownHeight;
        // Call the base constructor passing in a MonthCalendar instance.
        public ToolStripDataGridView(DataGridView nDataGridView, bool nCloseOnSingleClick) : base(nDataGridView)
        {
            this.AutoSize = false;
            this._MinDropDownWidth = nDataGridView.Width;
            this._CloseOnSingleClick = nCloseOnSingleClick;
            this._DropDownHeight = nDataGridView.Height;
        }

        public bool CloseOnSingleClick
        {
            get { return _CloseOnSingleClick; }
            set { _CloseOnSingleClick = value; }
        }

        public DataGridView DataGridViewControl
        {
            get { return Control as DataGridView; }
        }


        public int MinDropDownWidth
        {
            get { return _MinDropDownWidth; }
        }

        public int DropDownHeight
        {
            get { return _DropDownHeight; }
        }

        private void OnDataGridViewCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (!(e.RowIndex < 0) && DataGridViewControl.Rows[e.RowIndex].Displayed)
                DataGridViewControl.CurrentCell = DataGridViewControl.Rows[e.RowIndex].Cells[0];
            if (!DataGridViewControl.Focused)
                DataGridViewControl.Focus();
        }

        private void OnDataGridViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ((ToolStripDropDown)this.Owner).Close(ToolStripDropDownCloseReason.ItemClicked);
                e.Handled = true;
            }
        }

        private void myDataGridView_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ((ToolStripDropDown)this.Owner).Close(ToolStripDropDownCloseReason.ItemClicked);
        }

        private void myDataGridView_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (_CloseOnSingleClick)
                ((ToolStripDropDown)this.Owner).Close(ToolStripDropDownCloseReason.ItemClicked);
        }

        // Subscribe and unsubscribe the control events you wish to expose.
        protected override void OnSubscribeControlEvents(Control c)
        {
            // Call the base so the base events are connected.
            base.OnSubscribeControlEvents(c);

            // Cast the control to a MonthCalendar control.
            DataGridView nDataGridView = (DataGridView)c;

            // Add the event.
            nDataGridView.CellMouseEnter += OnDataGridViewCellMouseEnter;
            nDataGridView.KeyDown += OnDataGridViewKeyDown;
            nDataGridView.CellDoubleClick += myDataGridView_DoubleClick;
            nDataGridView.CellClick += myDataGridView_Click;

        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            // Call the base method so the basic events are unsubscribed.
            base.OnUnsubscribeControlEvents(c);

            // Cast the control to a MonthCalendar control.
            DataGridView nDataGridView = (DataGridView)c;

            // Remove the event.
            nDataGridView.CellMouseEnter -= OnDataGridViewCellMouseEnter;
            nDataGridView.KeyDown -= OnDataGridViewKeyDown;
            nDataGridView.CellDoubleClick -= myDataGridView_DoubleClick;
            nDataGridView.CellClick -= myDataGridView_Click;

        }

        protected override void OnBoundsChanged()
        {
            base.OnBoundsChanged();
            if ((Control != null))
            {
                ((DataGridView)Control).Size = this.Size;
                ((DataGridView)Control).AutoResizeColumns();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if ((Control != null) && !((DataGridView)Control).IsDisposed)
                Control.Dispose();
        }

    }

}
