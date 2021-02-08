namespace Advanced3DModels
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbQuality = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLightSource = new System.Windows.Forms.ComboBox();
            this.cmbRenderStatus = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBack = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxPerspective = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.cmbLookAt = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxCamera = new System.Windows.Forms.CheckBox();
            this.checkBoxFog = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 640);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(857, 658);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Выход";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbModel
            // 
            this.cmbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new System.Drawing.Point(771, 12);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(197, 21);
            this.cmbModel.TabIndex = 4;
            this.cmbModel.SelectedIndexChanged += new System.EventHandler(this.cmbModel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(719, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Модель";
            // 
            // cmbQuality
            // 
            this.cmbQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQuality.FormattingEnabled = true;
            this.cmbQuality.Location = new System.Drawing.Point(771, 39);
            this.cmbQuality.Name = "cmbQuality";
            this.cmbQuality.Size = new System.Drawing.Size(197, 21);
            this.cmbQuality.TabIndex = 6;
            this.cmbQuality.SelectedIndexChanged += new System.EventHandler(this.cmbQuality_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(711, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Качество";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(734, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Свет";
            // 
            // cmbLightSource
            // 
            this.cmbLightSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLightSource.FormattingEnabled = true;
            this.cmbLightSource.Location = new System.Drawing.Point(771, 66);
            this.cmbLightSource.Name = "cmbLightSource";
            this.cmbLightSource.Size = new System.Drawing.Size(197, 21);
            this.cmbLightSource.TabIndex = 9;
            this.cmbLightSource.SelectedIndexChanged += new System.EventHandler(this.cmbLightSource_SelectedIndexChanged);
            // 
            // cmbRenderStatus
            // 
            this.cmbRenderStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRenderStatus.FormattingEnabled = true;
            this.cmbRenderStatus.Location = new System.Drawing.Point(771, 93);
            this.cmbRenderStatus.Name = "cmbRenderStatus";
            this.cmbRenderStatus.Size = new System.Drawing.Size(197, 21);
            this.cmbRenderStatus.TabIndex = 10;
            this.cmbRenderStatus.SelectedIndexChanged += new System.EventHandler(this.cmbRenderStatus_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(689, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Отображение";
            // 
            // cmbBack
            // 
            this.cmbBack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBack.FormattingEnabled = true;
            this.cmbBack.Location = new System.Drawing.Point(771, 120);
            this.cmbBack.Name = "cmbBack";
            this.cmbBack.Size = new System.Drawing.Size(197, 21);
            this.cmbBack.TabIndex = 12;
            this.cmbBack.SelectedIndexChanged += new System.EventHandler(this.cmbBack_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(734, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Фон";
            // 
            // checkBoxPerspective
            // 
            this.checkBoxPerspective.AutoSize = true;
            this.checkBoxPerspective.Location = new System.Drawing.Point(771, 147);
            this.checkBoxPerspective.Name = "checkBoxPerspective";
            this.checkBoxPerspective.Size = new System.Drawing.Size(93, 17);
            this.checkBoxPerspective.TabIndex = 14;
            this.checkBoxPerspective.Text = "Перспектива";
            this.checkBoxPerspective.UseVisualStyleBackColor = true;
            this.checkBoxPerspective.CheckedChanged += new System.EventHandler(this.checkBoxPerspective_CheckedChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(658, 352);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(309, 300);
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            // 
            // cmbLookAt
            // 
            this.cmbLookAt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLookAt.FormattingEnabled = true;
            this.cmbLookAt.Location = new System.Drawing.Point(771, 325);
            this.cmbLookAt.Name = "cmbLookAt";
            this.cmbLookAt.Size = new System.Drawing.Size(196, 21);
            this.cmbLookAt.TabIndex = 16;
            this.cmbLookAt.SelectedIndexChanged += new System.EventHandler(this.cmbLookAt_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(719, 328);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Камера";
            // 
            // checkBoxCamera
            // 
            this.checkBoxCamera.AutoSize = true;
            this.checkBoxCamera.Location = new System.Drawing.Point(765, 302);
            this.checkBoxCamera.Name = "checkBoxCamera";
            this.checkBoxCamera.Size = new System.Drawing.Size(202, 17);
            this.checkBoxCamera.TabIndex = 18;
            this.checkBoxCamera.Text = "Включить дополнительную камеру";
            this.checkBoxCamera.UseVisualStyleBackColor = true;
            this.checkBoxCamera.CheckedChanged += new System.EventHandler(this.checkBoxCamera_CheckedChanged);
            // 
            // checkBoxFog
            // 
            this.checkBoxFog.AutoSize = true;
            this.checkBoxFog.Location = new System.Drawing.Point(771, 170);
            this.checkBoxFog.Name = "checkBoxFog";
            this.checkBoxFog.Size = new System.Drawing.Size(58, 17);
            this.checkBoxFog.TabIndex = 19;
            this.checkBoxFog.Text = "Туман";
            this.checkBoxFog.UseVisualStyleBackColor = true;
            this.checkBoxFog.CheckedChanged += new System.EventHandler(this.checkBoxFog_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 694);
            this.Controls.Add(this.checkBoxFog);
            this.Controls.Add(this.checkBoxCamera);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbLookAt);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.checkBoxPerspective);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbBack);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbRenderStatus);
            this.Controls.Add(this.cmbLightSource);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbQuality);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbModel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "3D Модели";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbQuality;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLightSource;
        private System.Windows.Forms.ComboBox cmbRenderStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBack;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxPerspective;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ComboBox cmbLookAt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxCamera;
        private System.Windows.Forms.CheckBox checkBoxFog;
    }
}

