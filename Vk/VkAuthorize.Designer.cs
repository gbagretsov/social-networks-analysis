namespace SocialNetworksAnalysis.Vk
{
    partial class VKAuthorize
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
            this.vkAuthBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // vkAuthBrowser
            // 
            this.vkAuthBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vkAuthBrowser.Location = new System.Drawing.Point(0, 0);
            this.vkAuthBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.vkAuthBrowser.Name = "vkAuthBrowser";
            this.vkAuthBrowser.Size = new System.Drawing.Size(658, 383);
            this.vkAuthBrowser.TabIndex = 0;
            this.vkAuthBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.vkAuthBrowser_DocumentCompleted);
            // 
            // VKAuthorize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 383);
            this.Controls.Add(this.vkAuthBrowser);
            this.Name = "VKAuthorize";
            this.Text = "VK Authorize";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser vkAuthBrowser;
    }
}