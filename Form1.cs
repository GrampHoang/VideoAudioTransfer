namespace VideoAudioTransfer;

public partial class Form1 : Form
{
    private PictureBox pictureBox1;
    private PictureBox pictureBox2;
    
    private int AREAW = 320;
    private int AREAH = 180;
    private TextBox fileNameTextBox1;
    private TextBox fileNameTextBox2;
    private string fileName1;
    private string fileName2;

    private Button transferAudioButton;
    public static RichTextBox logRichTextBox;

    public Form1()
    {
            
            // Set up the main form
            this.Text = "Audio Transfer";
            this.Size = new System.Drawing.Size(850, 720);
            this.StartPosition = FormStartPosition.CenterScreen;

            pictureBox1 = UICreator.CreatePictureBox(AREAW, AREAH, 20, 20);
            pictureBox1.Tag = 1;
            pictureBox1.Click += new EventHandler(ClickHandler);

            pictureBox2 = UICreator.CreatePictureBox(AREAW, AREAH, AREAW + 50, 20);
            pictureBox2.Tag = 2;
            pictureBox2.Click += new EventHandler(ClickHandler);

            fileNameTextBox1 = UICreator.CreateTextBox(AREAW, 175, 20,          AREAH + 30); 
            fileNameTextBox2 = UICreator.CreateTextBox(AREAW, 175, AREAW + 50, AREAH + 30);
            fileNameTextBox1.Text = "File to receive audio";
            fileNameTextBox2.Text = "File to share audio";

            
            // this.Controls.Add(clickableArea1);
            this.Controls.Add(fileNameTextBox1);
            this.Controls.Add(fileNameTextBox2);
            this.Controls.Add(pictureBox1);
            this.Controls.Add(pictureBox2);
            fileNameTextBox1.TabStop = false;
            fileNameTextBox2.TabStop = false;

            transferAudioButton = new Button();
            transferAudioButton.Text = "Transfer Audio";
            transferAudioButton.Location = new System.Drawing.Point(700, 20);
            transferAudioButton.Size = new System.Drawing.Size(100, 210);
            transferAudioButton.Click += (sender, e) =>{MainTask.TransferAudio(fileName1, fileName2);};

            // Create a log area (RichTextBox)
            logRichTextBox = new RichTextBox();
            logRichTextBox.Size = new System.Drawing.Size(775, 400);
            logRichTextBox.Location = new System.Drawing.Point(20, AREAH + 70);
            logRichTextBox.BackColor = System.Drawing.Color.Black;
            logRichTextBox.ForeColor = System.Drawing.Color.White;

            // Add controls to the form
            this.Controls.Add(transferAudioButton);
            this.Controls.Add(logRichTextBox);
        }
    
    private async void ClickHandler(object sender, EventArgs e)
    {   
        PictureBox picBox = (PictureBox)sender;
        WriteToLog("Starting video processing...");
        OpenFileDialog openFileDialog = new OpenFileDialog();
        // openFileDialog.Title = "Pick a video file";
        if ((int)picBox.Tag == 1){
            openFileDialog.Title = "Pick the video that receive audio";
        } else if ((int)picBox.Tag == 2) {
            openFileDialog.Title = "Pick the video that share the audio";
        }
        openFileDialog.Filter = "Video Files|*.mp4;*.mkv;*.avi;*.mov;*.wmv;*.flv;*.webm;*.3gp;*.ogg|All Files (*.*)|*.*";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            WriteToLog("Getting video name....");
            string filePath = openFileDialog.FileName;
            string fileName = Path.GetFileName(filePath);
            string logMessage = string.Format("Video found at: {0}", filePath);
            WriteToLog(logMessage);
            
            if ((int)picBox.Tag == 1){
                fileName1 = fileName;
                fileNameTextBox1.Text = fileName1;
                fileNameTextBox1.ForeColor = System.Drawing.Color.Black;
                fileName1 = filePath;
            } else if ((int)picBox.Tag == 2) {
                fileName2 = fileName;
                fileNameTextBox2.Text = Path.GetFileName(filePath);;
                fileNameTextBox2.ForeColor = System.Drawing.Color.Black;
                fileName2 = filePath;
            }
            // Show a loading indicator maybe
            WriteToLog("Getting video image...");
            Image thumbnailImage = await Utils.GenerateThumbnailAsync(filePath);
            
            if (thumbnailImage != null)
            {
                if ((int)picBox.Tag == 1){
                     pictureBox1.Image = thumbnailImage;
                } else if ((int)picBox.Tag == 2) {
                     pictureBox2.Image = thumbnailImage;
                }
                WriteToLog("Done");
            } else {
                WriteToLog("[HARMLESS] If the video name is correct, everything will run as intended");
            }
        }
    }

    public static void WriteToLog(string text)
    {
        // Append the text to the logRichTextBox and add a new line.
        logRichTextBox.AppendText(text + Environment.NewLine);

        // Optionally, you can scroll the logRichTextBox to the bottom to show the latest log entries.
        logRichTextBox.SelectionStart = logRichTextBox.Text.Length;
        logRichTextBox.ScrollToCaret();
    }
}