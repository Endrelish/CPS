namespace CPS1
{
    public class ViewModelLocator
    {
        private MainViewModel viewModel;

        public MainViewModel ViewModel
        {
            get
            {
                if (this.viewModel == null)
                {
                    this.viewModel = new MainViewModel();
                }

                return this.viewModel;
            }
        }
    }
}