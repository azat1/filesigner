﻿namespace filesigner2
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cbCerts = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lbErrs = new System.Windows.Forms.ListBox();
            this.cbExtSignZIP = new System.Windows.Forms.CheckBox();
            this.tbCertInfo = new System.Windows.Forms.TextBox();
            this.cbNoEntryZIPSIGDelete = new System.Windows.Forms.CheckBox();
            this.cbPDFSignDelete = new System.Windows.Forms.CheckBox();
            this.cbCoSign = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Сертификат";
            // 
            // cbCerts
            // 
            this.cbCerts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCerts.FormattingEnabled = true;
            this.cbCerts.Location = new System.Drawing.Point(95, 6);
            this.cbCerts.Name = "cbCerts";
            this.cbCerts.Size = new System.Drawing.Size(394, 21);
            this.cbCerts.TabIndex = 1;
            this.cbCerts.SelectedIndexChanged += new System.EventHandler(this.cbCerts_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Файлы";
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(12, 204);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFiles.Size = new System.Drawing.Size(477, 82);
            this.lbFiles.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 412);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(214, 53);
            this.button1.TabIndex = 4;
            this.button1.Text = "Добавить...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(257, 412);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(232, 53);
            this.button2.TabIndex = 5;
            this.button2.Text = "Подписать";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbErrs
            // 
            this.lbErrs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbErrs.FormattingEnabled = true;
            this.lbErrs.Location = new System.Drawing.Point(12, 409);
            this.lbErrs.Name = "lbErrs";
            this.lbErrs.Size = new System.Drawing.Size(0, 43);
            this.lbErrs.TabIndex = 6;
            // 
            // cbExtSignZIP
            // 
            this.cbExtSignZIP.AutoSize = true;
            this.cbExtSignZIP.Checked = true;
            this.cbExtSignZIP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExtSignZIP.Location = new System.Drawing.Point(257, 312);
            this.cbExtSignZIP.Name = "cbExtSignZIP";
            this.cbExtSignZIP.Size = new System.Drawing.Size(188, 17);
            this.cbExtSignZIP.TabIndex = 7;
            this.cbExtSignZIP.Text = "Делать внешнюю подпись у ZIP";
            this.cbExtSignZIP.UseVisualStyleBackColor = true;
            // 
            // tbCertInfo
            // 
            this.tbCertInfo.Location = new System.Drawing.Point(12, 33);
            this.tbCertInfo.Multiline = true;
            this.tbCertInfo.Name = "tbCertInfo";
            this.tbCertInfo.ReadOnly = true;
            this.tbCertInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCertInfo.Size = new System.Drawing.Size(477, 139);
            this.tbCertInfo.TabIndex = 8;
            // 
            // cbNoEntryZIPSIGDelete
            // 
            this.cbNoEntryZIPSIGDelete.AutoSize = true;
            this.cbNoEntryZIPSIGDelete.Checked = true;
            this.cbNoEntryZIPSIGDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNoEntryZIPSIGDelete.Location = new System.Drawing.Point(257, 335);
            this.cbNoEntryZIPSIGDelete.Name = "cbNoEntryZIPSIGDelete";
            this.cbNoEntryZIPSIGDelete.Size = new System.Drawing.Size(156, 17);
            this.cbNoEntryZIPSIGDelete.TabIndex = 9;
            this.cbNoEntryZIPSIGDelete.Text = "Не удалять подписи у ZIP";
            this.cbNoEntryZIPSIGDelete.UseVisualStyleBackColor = true;
            // 
            // cbPDFSignDelete
            // 
            this.cbPDFSignDelete.AutoSize = true;
            this.cbPDFSignDelete.Location = new System.Drawing.Point(257, 358);
            this.cbPDFSignDelete.Name = "cbPDFSignDelete";
            this.cbPDFSignDelete.Size = new System.Drawing.Size(232, 17);
            this.cbPDFSignDelete.TabIndex = 9;
            this.cbPDFSignDelete.Text = "Не удалять существующие подписи PDF";
            this.cbPDFSignDelete.UseVisualStyleBackColor = true;
            // 
            // cbCoSign
            // 
            this.cbCoSign.AutoSize = true;
            this.cbCoSign.Location = new System.Drawing.Point(257, 381);
            this.cbCoSign.Name = "cbCoSign";
            this.cbCoSign.Size = new System.Drawing.Size(240, 17);
            this.cbCoSign.TabIndex = 7;
            this.cbCoSign.Text = "Добавлять подпись к уже существующим";
            this.cbCoSign.UseVisualStyleBackColor = true;
            this.cbCoSign.CheckedChanged += new System.EventHandler(this.cbCoSign_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(11, 335);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 21);
            this.button3.TabIndex = 4;
            this.button3.Text = "Добавить папку...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(11, 377);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(118, 21);
            this.button4.TabIndex = 4;
            this.button4.Text = "Удалить из списка";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(457, 306);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(32, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "...";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 475);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.cbPDFSignDelete);
            this.Controls.Add(this.cbNoEntryZIPSIGDelete);
            this.Controls.Add(this.tbCertInfo);
            this.Controls.Add(this.cbCoSign);
            this.Controls.Add(this.cbExtSignZIP);
            this.Controls.Add(this.lbErrs);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCerts);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подписывание файлов";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCerts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lbErrs;
        private System.Windows.Forms.CheckBox cbExtSignZIP;
        private System.Windows.Forms.TextBox tbCertInfo;
        private System.Windows.Forms.CheckBox cbNoEntryZIPSIGDelete;
        private System.Windows.Forms.CheckBox cbPDFSignDelete;
        private System.Windows.Forms.CheckBox cbCoSign;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

