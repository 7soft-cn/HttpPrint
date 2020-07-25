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

namespace SevenSoft.HttpPrint.Service
{
    public partial class FormService : Form
    {
        public FormService()
        {
            InitializeComponent();

            ///加载打印机列表
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

        private void FormService_Load(object sender, EventArgs e)
        {
            new RunHttp();//运行HTTP服务
            //pDoc.PrinterSettings.PrinterName = "ZDesigner GK888t (EPL)";

        }

        private void FormDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private PrintHelper printHelper = new PrintHelper();

        /// <summary>
        /// 接受HTTP打印任务
        /// </summary>
        /// <param name="strPrintData"></param>
        /// <returns></returns>
        public bool HttpPrintStart(string strPrintData)
        {
            try
            {

                var printData = PrintHelper.String2PrintObj(strPrintData);
                var lstPrintInfos = printData.template.content;

                pDoc.PrinterSettings.PrinterName = cmbPrinter.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(pDoc.PrinterSettings.PrinterName))
                {
                    MessageBox.Show("未能指定打印机名称");
                    return false;
                }
                pDoc.DefaultPageSettings.PaperSize = new PaperSize("Custum", printData.template.width, printData.template.height);//尺寸换算成 百分之一英寸
                printHelper.PrintInfos = lstPrintInfos;
                printHelper.values = printData.values;
                pDoc.PrinterSettings.Copies = printData.copies;
                pDoc.Print();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pdControl_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageScale = 1;
            g.PageUnit = GraphicsUnit.Document;//单位

            //g.FillRectangle(Brushes.Linen, 0, 0, 900, 600);
            printHelper.Print(g);
        }
    }
}
