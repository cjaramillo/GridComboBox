namespace GridComboBox
{
    partial class AccGridComboBox
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (DisposeToolStripDataGridView)
                {
                    if ((myDropDown != null) && !myDropDown.IsDisposed)
                        myDropDown.Dispose();
                    if ((myDataGridView != null) && (myDataGridView.DataGridViewControl != null) && !myDataGridView.DataGridViewControl.IsDisposed)
                        myDataGridView.DataGridViewControl.Dispose();
                    if ((myDataGridView != null) && !myDataGridView.IsDisposed)
                        myDataGridView.Dispose();
                }
                else if (!DisposeToolStripDataGridView && (myDropDown != null) && !myDropDown.IsDisposed)
                {
                    if ((myDataGridView != null))
                        myDropDown.Items.Remove(myDataGridView);
                    myDropDown.Dispose();
                }
            }
            base.Dispose(disposing);

            //if (disposing && (components != null))
            //{
            //    components.Dispose();
            //}
            //base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {

            this.SuspendLayout();
            this.ResumeLayout(false);

            //components = new System.ComponentModel.Container();
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion
    }
}
