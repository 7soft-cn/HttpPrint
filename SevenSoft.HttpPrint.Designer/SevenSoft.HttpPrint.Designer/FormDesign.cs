
using SevenSoft.HttpPrint.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenSoft.HttpPrint.Designer
{
    public partial class FormDesign : Form
    {

        private PrintHelper printHelper = new PrintHelper();
        
        public FormDesign()
        {
            InitializeComponent();
        }

        private void FormDesign_Load(object sender, EventArgs e)
        {
            ///首先加载打印机列表
            foreach (string fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!cmbPrinter.Items.Contains(fPrinterName))

                    cmbPrinter.Items.Add(fPrinterName);
            }
            if (cmbPrinter.Items != null && cmbPrinter.Items.Count > 0)
            {
                cmbPrinter.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("未能获取打印机列表");
                this.Close();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTemplate();
            txtTemplate.Text = PrintHelper.ConvertJsonString(txtTemplate.Text);
            this.ppCtlDisplay.InvalidatePreview();
        }

        /// <summary>
        /// 加载模板
        /// </summary>
        private void LoadTemplate()
        {
            var template = SevenSoft.HttpPrint.Utils.PrintHelper.String2Template(txtTemplate.Text);
            printHelper.width = template.width;
            printHelper.height = template.height;
            printHelper.PrintInfos = template.content;
            this.pDoc.DefaultPageSettings.PaperSize = new PaperSize("Custum", printHelper.width, printHelper.height);//A5纸尺寸换算成 百分之一英寸
        }

        private void pdControl_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageScale = 1;
            g.PageUnit = GraphicsUnit.Document;//单位
            //g.FillRectangle(Brushes.Linen, 0, 0, 900, 600);
            printHelper.Print(g);
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                pDoc.PrinterSettings.PrinterName = cmbPrinter.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(pDoc.PrinterSettings.PrinterName))
                {
                    MessageBox.Show("未能指定打印机名称");
                    return ;
                }
                if (!short.TryParse(txtCopies.Text, out short copies))
                {
                    MessageBox.Show("输入信息不正确：打印份数");
                    return;
                }
                //pDoc.DefaultPageSettings.PaperSize = new PaperSize("Custum", printHelper.width, printHelper.height);//A5纸尺寸换算成 百分之一英寸
                pDoc.PrinterSettings.Copies = copies;
                this.pDoc.Print();
                return ;
            }
            catch (Exception ex)
            {
                return;
            }
        }

    }
}
