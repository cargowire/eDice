using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace eDice.TestHarness
{
    partial class DiceValueForm
    {
        private IContainer components = null;
        private IDiceRegistration registration;

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            this.registration = eDice.Register();
            // this.registration = eDice.Register(this.Handle);
            this.registration.DiceShaken += this.DiceShaken;
            this.registration.DiceRolled += this.DiceRolled;
            this.registration.DicePower += this.DicePower;
            this.registration.DiceConnect += this.DiceConnect;
            this.registration.DiceDisconnect += this.DiceDisconnect;

            MatchButton.Click += MatchButtonOnClick;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        private void MatchButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.registration.StartMatch();
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            this.Log.Items.Insert(0, DateTime.Now.ToShortTimeString() + " Unhandled exception");
        }

        private void DiceShaken(object sender, EventArgs args)
        {
            this.Log.Items.Insert(0, DateTime.Now.ToShortTimeString() + " Dice Shaken");
        }

        private void DicePower(object sender, DiceState diceState)
        {
            this.Log.Items.Insert(0, DateTime.Now.ToShortTimeString() + " Dice power");
        }

        private void DiceConnect(object sender, DiceState diceState)
        {
            this.Log.Items.Insert(0, DateTime.Now.ToShortTimeString() + " Dice Connected");
        }
        
        private void DiceDisconnect(object sender, DiceState diceState)
        {
            this.Log.Items.Insert(0, DateTime.Now.ToShortTimeString() + " Dice Disconnected");
        }

        private void DiceRolled(object sender, DiceState e)
        {
            DiceValueLabel.Text = e.Value.ToString();
            this.Log.Items.Insert(0, DateTime.Now.ToShortTimeString() + " Dice Rolled: " + e.Value);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                this.registration.DiceRolled -= this.DiceRolled;
                this.registration.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            /*if (this.registration != null && this.registration.HandleMessage(m.Msg, m.WParam, m.LParam))
            {
                return;
            }*/

            base.WndProc(ref m);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DiceValueLabel = new System.Windows.Forms.Label();
            this.Log = new System.Windows.Forms.ListBox();
            this.MatchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DiceValueLabel
            // 
            this.DiceValueLabel.AutoSize = true;
            this.DiceValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DiceValueLabel.Location = new System.Drawing.Point(89, 64);
            this.DiceValueLabel.Name = "DiceValueLabel";
            this.DiceValueLabel.Size = new System.Drawing.Size(98, 108);
            this.DiceValueLabel.TabIndex = 0;
            this.DiceValueLabel.Text = "0";
            // 
            // Log
            // 
            this.Log.FormattingEnabled = true;
            this.Log.Location = new System.Drawing.Point(-5, 175);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(293, 95);
            this.Log.TabIndex = 1;
            // 
            // MatchButton
            // 
            this.MatchButton.Location = new System.Drawing.Point(4, 148);
            this.MatchButton.Name = "MatchButton";
            this.MatchButton.Size = new System.Drawing.Size(75, 23);
            this.MatchButton.TabIndex = 2;
            this.MatchButton.Text = "Match";
            this.MatchButton.UseVisualStyleBackColor = true;
            // 
            // DiceValueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.MatchButton);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.DiceValueLabel);
            this.Name = "DiceValueForm";
            this.Text = "Dice Value";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DiceValueLabel;
        private System.Windows.Forms.ListBox Log;
        private System.Windows.Forms.Button MatchButton;
    }
}

