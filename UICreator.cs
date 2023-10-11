namespace VideoAudioTransfer
{
    public class UICreator
    {
        public static TextBox CreateTextBox(int sizew, int sizeh, int posx, int posy)
        {
            TextBox textBox = new TextBox();
            textBox.Text = "Selected File...";
            textBox.Size = new System.Drawing.Size(sizew, sizeh);
            textBox.Location = new System.Drawing.Point(posx, posy);
            textBox.ReadOnly = true;
            textBox.BorderStyle = BorderStyle.None;
            textBox.BackColor = System.Drawing.Color.White;
            textBox.ForeColor = System.Drawing.Color.DarkGray;
            textBox.TabStop = false;

            return textBox;
        }

        public static Panel CreatePanel(int sizew, int sizeh, int posx, int posy)
        {
            Panel panel = new Panel();
            panel.Size = new System.Drawing.Size(sizew, sizeh);
            panel.Location = new System.Drawing.Point(posx, posy);
            panel.BackColor = System.Drawing.Color.LightGray;
            return panel;
        }

        public static PictureBox CreatePictureBox(int sizew, int sizeh, int posx, int posy)
        {
            PictureBox picBox = new PictureBox();
            picBox.Size = new System.Drawing.Size(sizew, sizeh);
            picBox.Location = new System.Drawing.Point(posx, posy);
            picBox.BackColor = System.Drawing.Color.White;
            return picBox;
        }
    }
}
