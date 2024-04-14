using Microsoft.AspNetCore.Components;

namespace FruitSA.Web.Components.Pages
{
    public class ConfirmationBase: ComponentBase
    {
        protected bool ShowConfirmation {  get; set; }
        [Parameter]
        public string ConfirmationTitle { get; set; } = "Delete Confirmation";
        [Parameter]
        public string ConfirmationMessage { get; set; } = "Are you sure you want to Delete this Product?";
        [Parameter]
        public string ActionButton { get; set; } = "Delete";

        public void Show()
        {
            ShowConfirmation = true;
            StateHasChanged();
        }
        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; } 
        protected async Task OnConfirmationChange(bool value)
        {
            ShowConfirmation = false;
            await ConfirmationChanged.InvokeAsync(value);
        }
    }
}
