using CPS1.ViewModel;
using Microsoft.Win32;

namespace CPS1.View
{
    public class FileDialogWpf : IFileDialog
    {
        public string GetOpenFilePath(string filter)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = filter;
            dialog.Multiselect = false;
            dialog.ShowDialog();
            return dialog.FileName;
        }

        public string GetSaveFilePath(string filter)
        {
            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = filter;
            dialog.DefaultExt = filter.Substring(filter.LastIndexOf("."));
            dialog.CreatePrompt = false;
            dialog.OverwritePrompt = true;
            dialog.ShowDialog();
            return dialog.FileName;
        }
    }
}