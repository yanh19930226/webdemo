namespace webdemo.Views.Shared.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        public FooterViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
