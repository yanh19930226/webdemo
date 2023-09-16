namespace webdemo.Views.Shared.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public HeaderViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
