using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tekstieditori
{
    public partial class Form1 : Form
    {
        Thread t;
        private string[] lines;
        private int linesPrinted;
        bool clockFlag = true;
        private PrintDocument printDocument = new PrintDocument();
        List<String> fontit = new List<string>();

        public Form1()
        {
            InitializeComponent();

            t = new Thread(new ThreadStart(clockProcedure));
            t.Start();
        }

        private void startAbout()
        {
            Application.Run(new about());
        }

        #region Kellon metodit
        private void clockProcedure()
        {
            while (clockFlag)
            {
                invoker(DateTime.Now.ToString("H:mm:ss"));
                Thread.Sleep(1000);
            }
        }
        void invoker(string time)
        {
            Invoke((MethodInvoker)delegate
            {
                clockBar.Text = time;
            });
        }
        #endregion

        #region Form and thread closing
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            clockFlag = false;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clockFlag = false;
            Application.Exit();
        }
        #endregion

        #region Change background color
        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.BackColor = Color.Green;
        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.BackColor = Color.Yellow;
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.BackColor = Color.White;
        }

        private void greyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.BackColor = Color.Gray;
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.BackColor = Color.Red;
        }

        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.BackColor = Color.Orange;
        }
        #endregion

        #region Chenge text color
        private void greenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.Green;
        }

        private void yellowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.Yellow;
        }

        private void whiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.White;
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.Gray;
        }

        private void redToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.Red;
        }

        private void orangeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox.ForeColor = Color.Orange;
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                Fonts.Items.Add(font.Name);
                fontit.Add(font.Name);

            }
            Fonts.SelectedIndex = 1;

        }

        private void Fonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int x = Fonts.SelectedIndex;
            var cvt = new FontConverter();
            textBox.Font = cvt.ConvertFromString(fontit[x]) as Font;
        }

        #region File menu buttons
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Create new file?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                textBox.Clear();
            }

        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                textBox.LoadFile(openFile.FileName, RichTextBoxStreamType.PlainText);
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBox.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                textBox.SaveFile(saveNotAs.FileName, RichTextBoxStreamType.PlainText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Create new file?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                textBox.Clear();
            }
        }
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                textBox.LoadFile(openFile.FileName, RichTextBoxStreamType.PlainText);
            }
        }
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBox.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion
        #region Printing
        private void EditorPrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {

        }

        private void EditorPrintDocument_EndPrint(object sender, PrintEventArgs e)
        {

        }

        private void EditorPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;

            while (linesPrinted < lines.Length)
            {
                e.Graphics.DrawString(lines[linesPrinted++], textBox.Font, new SolidBrush(textBox.ForeColor), x, y);

                y += Convert.ToInt32(
                    textBox.Font.GetHeight() * 1.5);

                if (y >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
            linesPrinted = 0;
            e.HasMorePages = false;
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            char[] param = { '\n' };

            if (EditorPrintDialog.ShowDialog() == DialogResult.OK)
            {
                lines = textBox.SelectedText.Split(param);
            }
            else
            {
                lines = textBox.Text.Split(param);
            }

            int i = 0;
            char[] trimParam = { '\r' };

            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorPrintPreviewDialog.ShowDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorPrintDialog.AllowSelection = false;
            if (EditorPrintDialog.ShowDialog() == DialogResult.OK)
            {
                EditorPrintDocument.Print();
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            EditorPrintDialog.AllowSelection = false;
            if (EditorPrintDialog.ShowDialog() == DialogResult.OK)
            {
                EditorPrintDocument.Print();
            }
        }
        #endregion

        private void aboutTextEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(startAbout));
            t.Start();
        }
        
    }
}
