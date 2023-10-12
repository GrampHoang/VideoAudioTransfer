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
        this.Text = "Audio Transfer";
        this.Size = new System.Drawing.Size(850, 720);
        this.StartPosition = FormStartPosition.CenterScreen;

        pictureBox1 = UICreator.CreatePictureBox(AREAW, AREAH, 20, 20);
        pictureBox1.Tag = 1;
        pictureBox1.Click += new EventHandler(ClickHandler);
        pictureBox1.DragDrop += new DragEventHandler( PictureBoxDragDrop);
        pictureBox1.DragEnter += new DragEventHandler(PictureBoxDragEnter);
        
        pictureBox2 = UICreator.CreatePictureBox(AREAW, AREAH, AREAW + 50, 20);
        pictureBox2.Tag = 2;
        pictureBox2.Click += new EventHandler(ClickHandler);
        pictureBox2.DragDrop += new DragEventHandler(PictureBoxDragDrop);
        pictureBox2.DragEnter += new DragEventHandler(PictureBoxDragEnter);
        
        this.Paint += (sender, e) =>
    {
        using (Graphics g = e.Graphics)
        using (Pen pen = new Pen(Color.Black, 2)) // Customize the pen as needed
        {
            // Calculate the starting and ending points of the arrow
            Point startPoint = new Point(pictureBox2.Left - 4, pictureBox2.Top + AREAH/2);
            Point endPoint = new Point(pictureBox1.Right + 4 , pictureBox1.Top + AREAH/2);

            // Calculate the arrowhead positions
            float arrowSize = 7; // Adjust the size of the arrowhead
            PointF arrow1 = new PointF(endPoint.X + arrowSize, endPoint.Y + arrowSize);
            PointF arrow2 = new PointF(endPoint.X + arrowSize, endPoint.Y - arrowSize);

            // Draw the arrow line
            g.DrawLine(pen, startPoint, endPoint);

            // Draw the arrowhead
            g.DrawLine(pen, endPoint, arrow1);
            g.DrawLine(pen, endPoint, arrow2);
        }
    };
    
        fileNameTextBox1 = UICreator.CreateTextBox(AREAW, 175, 20,          AREAH + 30); 
        fileNameTextBox2 = UICreator.CreateTextBox(AREAW, 175, AREAW + 50, AREAH + 30);
        fileNameTextBox1.Text = "File to receive audio";
        fileNameTextBox2.Text = "File to share audio";
        
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

        logRichTextBox = new RichTextBox();
        logRichTextBox.Size = new System.Drawing.Size(775, 400);
        logRichTextBox.Location = new System.Drawing.Point(20, AREAH + 70);
        logRichTextBox.BackColor = System.Drawing.Color.Black;
        logRichTextBox.ForeColor = System.Drawing.Color.White;

        this.Controls.Add(transferAudioButton);
        this.Controls.Add(logRichTextBox);
    }
    
    private void PictureBoxDragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effect = DragDropEffects.Copy;
        else
            e.Effect = DragDropEffects.None;
    }
    private async void ClickHandler(object sender, EventArgs e)
    {
        PictureBox picBox = (PictureBox)sender;
        WriteToLog("Starting video processing...");
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if ((int)picBox.Tag == 1)
        {
            openFileDialog.Title = "Pick the video that receive audio";
        }
        else if ((int)picBox.Tag == 2)
        {
            openFileDialog.Title = "Pick the video that share the audio";
        }
        openFileDialog.Filter = "Video Files|*.mp4;*.mkv;*.avi;*.mov;*.wmv;*.flv;*.webm;*.3gp;*.ogg|All Files (*.*)|*.*";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string filePath = openFileDialog.FileName;
            await HandleVideoSelection(picBox, filePath);
        }
    }

    private async void PictureBoxDragDrop(object sender, DragEventArgs e)
    {
        PictureBox picBox = (PictureBox)sender;

        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                string filePath = files[0];
                if (IsVideoFile(filePath))
                {
                    await HandleVideoSelection(picBox, filePath);
                }
                else
                {
                    WriteToLog("Invalid video file format.");
                }
            }
        }
    }

    private bool IsVideoFile(string filePath)
    {
        string[] validExtensions = { ".mp4", ".mkv", ".avi", ".mov", ".wmv", ".flv", ".webm", ".3gp", ".ogg" };
        string extension = Path.GetExtension(filePath);
        return validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
    }

    private async Task HandleVideoSelection(PictureBox picBox, string filePath)
    {
        WriteToLog("Getting video name....");
        string fileName = Path.GetFileName(filePath);
        string logMessage = string.Format("Video found at: {0}", filePath);
        WriteToLog(logMessage);

        if ((int)picBox.Tag == 1)
        {
            fileName1 = fileName;
            fileNameTextBox1.Text = fileName1;
            fileNameTextBox1.ForeColor = System.Drawing.Color.Black;
            fileName1 = filePath;
        }
        else if ((int)picBox.Tag == 2)
        {
            fileName2 = fileName;
            fileNameTextBox2.Text = Path.GetFileName(filePath);
            fileNameTextBox2.ForeColor = System.Drawing.Color.Black;
            fileName2 = filePath;
        }

        // Show a loading indicator maybe
        WriteToLog("Getting video image...");
        Image thumbnailImage = await Utils.GenerateThumbnailAsync(filePath);
        if (thumbnailImage != null)
        {
            if ((int)picBox.Tag == 1)
            {
                pictureBox1.Image = thumbnailImage;
            }
            else if ((int)picBox.Tag == 2)
            {
                pictureBox2.Image = thumbnailImage;
            }
            WriteToLog("Video image updated!");
        }
        else
        {
            WriteToLog("[HARMLESS] If the video name is correct, everything will run as intended");
        }
    }

    public static void WriteToLog(string text)
    {
        logRichTextBox.AppendText(text + Environment.NewLine);
        // Scroll the logRichTextBox to the bottom to show the latest log entries.
        logRichTextBox.SelectionStart = logRichTextBox.Text.Length;
        logRichTextBox.ScrollToCaret();
    }
}