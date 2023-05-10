namespace ServerTCP01
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.clientsListBox = new System.Windows.Forms.ListBox();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblClientes = new System.Windows.Forms.Label();
            this.serverText = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clientsListBox
            // 
            this.clientsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientsListBox.FormattingEnabled = true;
            this.clientsListBox.ItemHeight = 16;
            this.clientsListBox.Location = new System.Drawing.Point(12, 12);
            this.clientsListBox.Name = "clientsListBox";
            this.clientsListBox.Size = new System.Drawing.Size(559, 372);
            this.clientsListBox.TabIndex = 0;
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(12, 462);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(455, 22);
            this.logTextBox.TabIndex = 1;
            this.logTextBox.Visible = false;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(488, 459);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(83, 32);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "Refrescar";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(401, 401);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Clientes conectados:";
            // 
            // lblClientes
            // 
            this.lblClientes.AutoSize = true;
            this.lblClientes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblClientes.Location = new System.Drawing.Point(553, 401);
            this.lblClientes.Name = "lblClientes";
            this.lblClientes.Size = new System.Drawing.Size(14, 16);
            this.lblClientes.TabIndex = 4;
            this.lblClientes.Text = "0";
            // 
            // serverText
            // 
            this.serverText.Location = new System.Drawing.Point(12, 398);
            this.serverText.Name = "serverText";
            this.serverText.Size = new System.Drawing.Size(179, 22);
            this.serverText.TabIndex = 5;
            this.serverText.Text = "192.168.100.89";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(206, 394);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 30);
            this.button1.TabIndex = 6;
            this.button1.Text = "Empezar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 432);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.serverText);
            this.Controls.Add(this.lblClientes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.clientsListBox);
            this.Name = "Form1";
            this.Text = "Servidor: Sistemas Distribuidos Grupo-003";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox clientsListBox;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblClientes;
        private System.Windows.Forms.TextBox serverText;
        private System.Windows.Forms.Button button1;
    }
}

