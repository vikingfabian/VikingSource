
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//#if PCGAME
//using System.Windows.Forms;
//#endif

//namespace VikingEngine.HUD
//{
//#if PCGAME
//    class TextInputForm : Form
//    {
//        const int ControlHeight = 23;
//        System.Windows.Forms.Button OkButton, cancelbutton;
//        System.Windows.Forms.TextBox textbox;

//        public TextInputForm(string text)
//            : base()
//        {
//            this.OkButton = new System.Windows.Forms.Button();
//            this.cancelbutton = new System.Windows.Forms.Button();
//            this.SuspendLayout();


//            //textbox
//            textbox = new System.Windows.Forms.TextBox();
//            textbox.Location = new System.Drawing.Point(13, 13);
//            textbox.Size = new System.Drawing.Size(150, ControlHeight);
//            textbox.TabIndex = 0;
//            textbox.Text = text;
//            textbox.SelectionStart = 0;
//            textbox.SelectionLength = textbox.Text.Length;
//            // 
//            // button1
//            // 
//            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
//            this.OkButton.Location = new System.Drawing.Point(13, 40);
//            //this.OkButton.Name = "OK";
//            this.OkButton.Size = new System.Drawing.Size(75, ControlHeight);
//            this.OkButton.TabIndex = 1;
//            this.OkButton.Text = "OK";
//            this.OkButton.UseVisualStyleBackColor = true;
//            // 
//            // button2
//            // 
//            this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
//            this.cancelbutton.Location = new System.Drawing.Point(OkButton.Location.X + OkButton.Size.Width + 10, OkButton.Location.Y);
//            //this.cancelbutton.Name = "button2";
//            this.cancelbutton.Size = new System.Drawing.Size(75, ControlHeight);
//            this.cancelbutton.TabIndex = 2;
//            this.cancelbutton.Text = "Cancel";
//            this.cancelbutton.UseVisualStyleBackColor = true;
//            // 
//            // Form2
//            // 
//            this.AcceptButton = this.OkButton;
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.CancelButton = this.cancelbutton;
//            this.ClientSize = new System.Drawing.Size(298, 120);
//            this.Controls.Add(textbox);
//            this.Controls.Add(this.cancelbutton);
//            this.Controls.Add(this.OkButton);
//            this.Name = "Lootfest Text Input Form";
//            this.Text = "Lootfest Text Input";
//            this.Focus();
//            this.CenterToParent();
//            //this.Load += new System.EventHandler(this.fo);
//            this.ResumeLayout(false);
//        }

//        public string Result
//        {
//            get { return textbox.Text; }
//        }
//    }
//#endif
//}