namespace GOST_R_34._12_15
{
    partial class FormTest
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxInitialValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxIterationStep = new System.Windows.Forms.TextBox();
            this.textBoxNumberIterations = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(93, 198);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "ОК";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxInitialValue
            // 
            this.textBoxInitialValue.Location = new System.Drawing.Point(15, 29);
            this.textBoxInitialValue.Name = "textBoxInitialValue";
            this.textBoxInitialValue.Size = new System.Drawing.Size(234, 22);
            this.textBoxInitialValue.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Начальное значение (<100 мб)";
            // 
            // textBoxIterationStep
            // 
            this.textBoxIterationStep.Location = new System.Drawing.Point(15, 91);
            this.textBoxIterationStep.Name = "textBoxIterationStep";
            this.textBoxIterationStep.Size = new System.Drawing.Size(234, 22);
            this.textBoxIterationStep.TabIndex = 3;
            // 
            // textBoxNumberIterations
            // 
            this.textBoxNumberIterations.Location = new System.Drawing.Point(15, 157);
            this.textBoxNumberIterations.Name = "textBoxNumberIterations";
            this.textBoxNumberIterations.Size = new System.Drawing.Size(234, 22);
            this.textBoxNumberIterations.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Шаг итерации (<100 мб)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Количество итераций (<10)";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(174, 198);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 232);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxNumberIterations);
            this.Controls.Add(this.textBoxIterationStep);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxInitialValue);
            this.Controls.Add(this.buttonOK);
            this.Name = "FormTest";
            this.Text = "Тест скорости";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxInitialValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxIterationStep;
        private System.Windows.Forms.TextBox textBoxNumberIterations;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCancel;
    }
}