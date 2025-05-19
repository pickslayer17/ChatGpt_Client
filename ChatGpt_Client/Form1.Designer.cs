using ChatGpt_Client.Helpers;
using ChatGpt_Client.Models;

namespace ChatGpt_Client
{
    partial class Form1 : Form
    {

        private static readonly string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        private static ChatGptContext _analysisContext = new AnalysisChatGptContext();
        private static ChatGptContext _context = new MainChatGptContext();

        private static Action<RichTextBox, double> onRateLimitTemplate = (textBox, seconds) =>
        {
            textBox.Invoke(() =>
            {
                textBox.Text = $"{seconds:F1} сек нужно подождать";
            });
        };

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private async void button1_Click(object sender, EventArgs e)
        {
            string userMessage = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(userMessage)) return;
            Action<double> onRateLimit = seconds =>
            {
                onRateLimitTemplate(errorTextBox, seconds);
            };

            txtChat.AppendText($"Ты: {userMessage}\n");
            txtInput.Clear();

            string response = await ChatGptClientHelper.SendMessageWithContext(apiKey, _context, userMessage, onRateLimit);

            if (true)
            {
                txtChat.AppendText($"ChatGPT: {response}\n");
                errorTextBox.Clear();
            }
            else
            {
                _analysisContext.AddMessage(new UserChatMessage(userMessage));
                string finalResponse = await ChatGptClientHelper.SendMessageWithContext(apiKey, _analysisContext, response);
                _analysisContext.ClearMesssages();
                _context.ReplaceLastMessage(finalResponse);
                txtChat.AppendText($"ChatGPT({_context.Name}): {finalResponse}\n");
            }
        }

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtInput = new RichTextBox();
            txtChat = new RichTextBox();
            button1 = new Button();
            chatInstructions = new RichTextBox();
            errorTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // txtInput
            // 
            txtInput.Location = new Point(12, 318);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(654, 120);
            txtInput.TabIndex = 0;
            txtInput.Text = "";
            // 
            // txtChat
            // 
            txtChat.Location = new Point(11, 9);
            txtChat.Name = "txtChat";
            txtChat.Size = new Size(777, 303);
            txtChat.TabIndex = 1;
            txtChat.Text = "";
            // 
            // button1
            // 
            button1.Location = new Point(694, 318);
            button1.Name = "button1";
            button1.Size = new Size(94, 120);
            button1.TabIndex = 4;
            button1.Text = "Send";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // chatInstructions
            // 
            chatInstructions.Location = new Point(803, 12);
            chatInstructions.Name = "chatInstructions";
            chatInstructions.Size = new Size(555, 208);
            chatInstructions.TabIndex = 5;
            chatInstructions.Text = _context.MainChatInstructions;
            chatInstructions.TextChanged += richTextBox1_TextChanged;
            // 
            // errorTextBox
            // 
            errorTextBox.Location = new Point(802, 322);
            errorTextBox.Name = "errorTextBox";
            errorTextBox.Size = new Size(556, 116);
            errorTextBox.TabIndex = 6;
            errorTextBox.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1370, 453);
            Controls.Add(errorTextBox);
            Controls.Add(chatInstructions);
            Controls.Add(button1);
            Controls.Add(txtChat);
            Controls.Add(txtInput);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox txtInput;
        private RichTextBox txtChat;
        private Button button1;
        private RichTextBox chatInstructions;
        private RichTextBox errorTextBox;
    }
}
