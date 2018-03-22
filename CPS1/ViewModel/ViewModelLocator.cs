namespace CPS1.ViewModel
{
    public class ViewModelLocator
    {
        private static MainViewModel viewModel;

        public MainViewModel ViewModel
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