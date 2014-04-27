using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeechManager
{
    public partial class TextDisplay : Form
    {
        public TextDisplay()
        {
            InitializeComponent();
        }

        private void TextDisplay_Load(object sender, EventArgs e)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int delta = screenWidth - this.Width;
            this.Location = new Point(delta / 2, 0);
        }
    }
}
