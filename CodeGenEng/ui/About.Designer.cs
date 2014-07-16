namespace CodeGenEng.ui
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.panel1 = new System.Windows.Forms.Panel();
            this.g_statement = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.l_contact = new System.Windows.Forms.Label();
            this.l_author = new System.Windows.Forms.Label();
            this.l_version = new System.Windows.Forms.Label();
            this.l_name = new System.Windows.Forms.Label();
            this.l_cc = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.g_statement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.g_statement);
            this.panel1.Controls.Add(this.l_contact);
            this.panel1.Controls.Add(this.l_author);
            this.panel1.Controls.Add(this.l_version);
            this.panel1.Controls.Add(this.l_name);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint_1);
            // 
            // g_statement
            // 
            this.g_statement.Controls.Add(this.l_cc);
            this.g_statement.Controls.Add(this.pictureBox1);
            resources.ApplyResources(this.g_statement, "g_statement");
            this.g_statement.Name = "g_statement";
            this.g_statement.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::CodeGenEng.Properties.Resources.cc;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // l_contact
            // 
            resources.ApplyResources(this.l_contact, "l_contact");
            this.l_contact.Name = "l_contact";
            // 
            // l_author
            // 
            resources.ApplyResources(this.l_author, "l_author");
            this.l_author.Name = "l_author";
            // 
            // l_version
            // 
            resources.ApplyResources(this.l_version, "l_version");
            this.l_version.Name = "l_version";
            // 
            // l_name
            // 
            resources.ApplyResources(this.l_name, "l_name");
            this.l_name.Name = "l_name";
            this.l_name.Click += new System.EventHandler(this.label1_Click);
            // 
            // l_cc
            // 
            resources.ApplyResources(this.l_cc, "l_cc");
            this.l_cc.Name = "l_cc";
            // 
            // About
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.g_statement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label l_name;
        private System.Windows.Forms.GroupBox g_statement;
        private System.Windows.Forms.Label l_contact;
        private System.Windows.Forms.Label l_author;
        private System.Windows.Forms.Label l_version;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label l_cc;
    }
}