namespace CPS1.ViewModel
{
    public class ViewModelLocator
    {
        private static MainViewModel viewModel;

        public static MainViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new MainViewModel();
                }

                return viewModel;
            }
        }
    }
}