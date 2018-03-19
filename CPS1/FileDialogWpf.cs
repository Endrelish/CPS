namespace CPS1
{
    using Microsoft.Win32;

    public class FileDialogWpf : IFileDialog
    {
        public string GetOpenFilePath()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "Binary files (*.bin)|*.bin";
            dialog.Multiselect = false;
            dialog.ShowDialog();
            return dialog.FileName;
        }

        public string GetSaveFilePath()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "bin";
            dialog.CreatePrompt = false;
            dialog.OverwritePrompt = true;
            dialog.ShowDialog();
            return dialog.FileName;
        }
    }
}