using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEngineSharp_Editor
{
    public partial class EditorSuite : Form
    {
        private MapEditor _mapEditor;

        public EditorSuite()
        {
            InitializeComponent();

            this.IsMdiContainer = true;
        }

        private void _mapEditor_Disposed(object sender, EventArgs e)
        {
            _mapEditor = null;
        }

        private void mapEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_mapEditor == null)
            {
                _mapEditor = new MapEditor();
                _mapEditor.Disposed += _mapEditor_Disposed;
                _mapEditor.MdiParent = this;
                _mapEditor.Show();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
                (this.ActiveMdiChild as IChildFormActions).SaveData();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
                (this.ActiveMdiChild as IChildFormActions).LoadData();
        }
    }
}