namespace DesignerPlus
{
    partial class FormDesigner
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDesigner));
            this.ppCtlDisplay = new System.Windows.Forms.PrintPreviewControl();
            this.pDoc = new System.Drawing.Printing.PrintDocument();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCopies = new System.Windows.Forms.TextBox();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.cmbPrinter = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ppCtlDisplay
            // 
            this.ppCtlDisplay.AutoZoom = false;
            this.ppCtlDisplay.Document = this.pDoc;
            this.ppCtlDisplay.Location = new System.Drawing.Point(21, 15);
            this.ppCtlDisplay.Name = "ppCtlDisplay";
            this.ppCtlDisplay.Size = new System.Drawing.Size(469, 827);
            this.ppCtlDisplay.TabIndex = 9;
            this.ppCtlDisplay.Zoom = 0.8D;
            // 
            // pDoc
            // 
            this.pDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PdControl_PrintPage);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(932, 39);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(81, 68);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "刷新显示";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(920, 521);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "份数：";
            // 
            // txtCopies
            // 
            this.txtCopies.Location = new System.Drawing.Point(962, 517);
            this.txtCopies.Name = "txtCopies";
            this.txtCopies.Size = new System.Drawing.Size(36, 21);
            this.txtCopies.TabIndex = 12;
            this.txtCopies.Text = "1";
            // 
            // txtTemplate
            // 
            this.txtTemplate.Location = new System.Drawing.Point(512, 15);
            this.txtTemplate.Multiline = true;
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(393, 818);
            this.txtTemplate.TabIndex = 11;
            this.txtTemplate.Text = resources.GetString("txtTemplate.Text");
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(1031, 504);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(81, 45);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Text = "打印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // cmbPrinter
            // 
            this.cmbPrinter.FormattingEnabled = true;
            this.cmbPrinter.Location = new System.Drawing.Point(922, 458);
            this.cmbPrinter.Name = "cmbPrinter";
            this.cmbPrinter.Size = new System.Drawing.Size(190, 20);
            this.cmbPrinter.TabIndex = 14;
            // 
            // FormDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 857);
            this.Controls.Add(this.cmbPrinter);
            this.Controls.Add(this.ppCtlDisplay);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCopies);
            this.Controls.Add(this.txtTemplate);
            this.Controls.Add(this.btnPrint);
            this.Name = "FormDesign";
            this.Text = "打印模板设计器";
            this.Load += new System.EventHandler(this.FormDesign_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PrintPreviewControl ppCtlDisplay;
        private System.Drawing.Printing.PrintDocument pDoc;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCopies;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ComboBox cmbPrinter;
    }
}