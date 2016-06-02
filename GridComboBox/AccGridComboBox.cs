using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace GridComboBox
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ComboBox))]
    [System.ComponentModel.DefaultBindingProperty("SelectedValue")]
    [System.ComponentModel.LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")]

    public partial class AccGridComboBox : ComboBox
    {
        private const UInt32 WM_LBUTTONDOWN = 0x201;
        private const UInt32 WM_LBUTTONDBLCLK = 0x203;

        private const UInt32 WM_KEYF4 = 0x134;
        private ToolStripDataGridView myDataGridView = null;
        private ToolStripDropDown myDropDown;
        private object _SelectedValue = null;
        private bool _CloseOnSingleClick = true;

        private bool _InstantBinding = true;
        #region "Disabled properties"

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new ComboBox.ObjectCollection Items
        {
            get { return base.Items; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new object DataSource
        {
            get { return null; }
        }

        #endregion

        public AccGridComboBox()
        {
            InitializeComponent();
            myDropDown = new ToolStripDropDown();
            myDropDown.AutoSize = false;
            myDropDown.Closed += ToolStripDropDown_Closed;
        }


        public bool HasAttachedGrid
        {
            get { return (myDataGridView != null); }
        }

        public DataGridView AttachedGrid
        {
            get
            {
                if ((myDataGridView != null))
                    return myDataGridView.DataGridViewControl;
                return null;
            }
        }

        protected virtual bool DisposeToolStripDataGridView
        {
            get { return true; }
        }

        public bool InstantBinding
        {
            get { return _InstantBinding; }
            set { _InstantBinding = value; }
        }

        public object SelectedValue
        {
            get { return _SelectedValue; }
            set
            {
                SetValue(value, true);
                base.OnSelectedValueChanged(new EventArgs());
            }
        }


        public void AddDataGridView(DataGridView nDataGridView, bool nCloseOnSingleClick)
        {
            if ((myDataGridView != null))
                throw new Exception("Error. DataGridView is already assigned to the AccGridComboBox.");
            myDataGridView = new ToolStripDataGridView(nDataGridView, nCloseOnSingleClick);
            myDropDown.Width = Math.Max(this.Width, myDataGridView.MinDropDownWidth);
            myDropDown.Height = nDataGridView.Height;
            myDropDown.Items.Clear();
            myDropDown.Items.Add(this.myDataGridView);
        }

        internal void AddToolStripDataGridView(ToolStripDataGridView nToolStripDataGridView)
        {
            if (nToolStripDataGridView == null || ((myDataGridView != null) && object.ReferenceEquals(myDataGridView, nToolStripDataGridView)))
                return;
            myDataGridView = nToolStripDataGridView;
            myDropDown.Width = Math.Max(this.Width, myDataGridView.MinDropDownWidth);
            myDropDown.Height = myDataGridView.DropDownHeight;
            myDropDown.Items.Clear();
            myDropDown.Items.Add(this.myDataGridView);
        }

        private void ToolStripDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                if (!base.Focused)
                    base.Focus();
                if (myDataGridView.DataGridViewControl.CurrentRow == null)
                {
                    SetValue(null, false);
                }
                else
                {
                    SetValue(myDataGridView.DataGridViewControl.CurrentRow.DataBoundItem, false);
                }
                base.OnSelectedValueChanged(new EventArgs());
                if (_InstantBinding)
                {
                    foreach (Binding b in base.DataBindings)
                    {
                        b.WriteValue();
                    }
                }
            }
        }


        private void SetValue(object value, bool IsValueMemberValue)
        {
            if (value == null)
            {
                this.Text = "";
                _SelectedValue = null;
            }
            else
            {

                if (this.ValueMember == null || string.IsNullOrEmpty(this.ValueMember.ToString().Trim()) || IsValueMemberValue)
                {
                    this.Text = value.ToString();
                    _SelectedValue = value;
                }
                else
                {
                    object newValue = GetValueMemberValue(value);

                    if (newValue == null)
                    {
                        this.Text = value.ToString();
                        _SelectedValue = value;
                    }
                    else
                    {
                        this.Text = newValue.ToString();
                        _SelectedValue = newValue;
                    }
                }
            }
        }

        private Point CalculatePoz()
        {
            Point point = new Point(0, this.Height);
            if ((this.PointToScreen(new Point(0, 0)).Y + this.Height + this.myDataGridView.Height) > Screen.PrimaryScreen.WorkingArea.Height)
            {
                point.Y = -this.myDataGridView.Height - 7;
            }
            return point;
        }


        protected override void WndProc(ref Message m)
        {
            //#Region "WM_KEYF4"
            if (m.Msg == WM_KEYF4)
            {
                this.Focus();
                this.myDropDown.Refresh();
                if (!this.myDropDown.Visible)
                    ShowDropDown();
                else
                    myDropDown.Close();
                return;
            }
            //#End Region

            //#Region "WM_LBUTTONDBLCLK"
            if (m.Msg == WM_LBUTTONDBLCLK || m.Msg == WM_LBUTTONDOWN)
            {
                if (!this.myDropDown.Visible)
                    ShowDropDown();
                else
                    myDropDown.Close();
                return;
            }
            //#End Region
            base.WndProc(ref m);
        }

        protected override void OnResize(System.EventArgs e)
        {
            int minWidth = 0;
            if ((this.myDataGridView != null))
                minWidth = this.myDataGridView.MinDropDownWidth;
            myDropDown.Width = Math.Max(this.Width, minWidth);
            if ((myDataGridView != null))
            {
                myDataGridView.Width = Math.Max(this.Width, minWidth);
                myDataGridView.DataGridViewControl.Width = Math.Max(this.Width, minWidth);
                myDataGridView.DataGridViewControl.AutoResizeColumns();
            }
            base.OnResize(e);
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ShowDropDown();
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        private void ShowDropDown()
        {

            if ((this.myDataGridView != null))
            {
                if (!myDropDown.Items.Contains(this.myDataGridView))
                {
                    myDropDown.Items.Clear();
                    myDropDown.Items.Add(this.myDataGridView);
                }

                myDropDown.Width = Math.Max(this.Width, this.myDataGridView.MinDropDownWidth);
                myDataGridView.Size = myDropDown.Size;
                myDataGridView.DataGridViewControl.Size = myDropDown.Size;
                myDataGridView.DataGridViewControl.AutoResizeColumns();

                if (_SelectedValue == null || Convert.IsDBNull(_SelectedValue))
                {
                    myDataGridView.DataGridViewControl.CurrentCell = null;


                }
                else if ((this.ValueMember != null) && !string.IsNullOrEmpty(this.ValueMember.ToString().Trim()))
                {

                    if (myDataGridView.DataGridViewControl.Rows.Count < 1
                        || myDataGridView.DataGridViewControl.Rows[0].DataBoundItem == null
                        || myDataGridView.DataGridViewControl.Rows[0].DataBoundItem.GetType().GetProperty(this.ValueMember.Trim(), BindingFlags.Public) == null
                        || myDataGridView.DataGridViewControl.Rows[0].DataBoundItem.GetType().GetProperty(this.ValueMember.Trim(), BindingFlags.Instance) == null
                        )

                    {
                        myDataGridView.DataGridViewControl.CurrentCell = null;
                    }
                    else
                    {
                        object CurrentValue = null;
                        foreach (DataGridViewRow r in myDataGridView.DataGridViewControl.Rows)
                        {
                            if ((r.DataBoundItem != null))
                            {
                                CurrentValue = GetValueMemberValue(r.DataBoundItem);
                                if (_SelectedValue == CurrentValue)
                                {
                                    myDataGridView.DataGridViewControl.CurrentCell = myDataGridView.DataGridViewControl.Rows[r.Index].Cells[0];
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                        }

                    }


                }
                else
                {
                    bool SelectionFound = false;
                    foreach (DataGridViewRow r in myDataGridView.DataGridViewControl.Rows)
                    {
                        try
                        {
                            if (_SelectedValue == r.DataBoundItem)
                            {
                                myDataGridView.DataGridViewControl.CurrentCell = myDataGridView.DataGridViewControl.Rows[r.Index].Cells[0];
                                SelectionFound = true;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                if (object.ReferenceEquals(_SelectedValue, r.DataBoundItem))
                                {
                                    myDataGridView.DataGridViewControl.CurrentCell = myDataGridView.DataGridViewControl.Rows[r.Index].Cells[0];
                                    SelectionFound = true;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }
                    if (!SelectionFound)
                        myDataGridView.DataGridViewControl.CurrentCell = null;

                }

                myDropDown.Show(this, CalculatePoz());
                //New Point(0, Me.Height)
            }

        }

        private object GetValueMemberValue(object DataboundItem)
        {
            object newValue = null;
            try
            {
                newValue = DataboundItem.GetType().GetProperty(this.ValueMember.Trim(), BindingFlags.Public).GetValue(DataboundItem, null);
                if (newValue == null)
                    newValue = DataboundItem.GetType().GetProperty(this.ValueMember.Trim(), BindingFlags.Instance).GetValue(DataboundItem, null);
            }
            catch (Exception ex)
            {
            }
            return newValue;
        }

    }
}
