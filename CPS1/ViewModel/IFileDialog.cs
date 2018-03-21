namespace CPS1.ViewModel
{
    public interface IFileDialog
    {
        string GetOpenFilePath(string format);

        string GetSaveFilePath(string format);

    }
}