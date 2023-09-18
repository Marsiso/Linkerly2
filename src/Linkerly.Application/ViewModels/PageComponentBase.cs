using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Linkerly.Application.ViewModels;

public class PageComponentBase<TViewModel> : ComponentBase where TViewModel : ViewModelBase
{
    [FromServices] public required TViewModel Model { get; set; }

    public void Dispose()
    {
        Model.PropertyChanged -= OnModelPropertyChanged;
    }

    protected override bool ShouldRender()
    {
        return Model.Busy is false;
    }

    protected override Task OnInitializedAsync()
    {
        Model.PropertyChanged += OnModelPropertyChanged;

        return Model.OnViewModelInitialized();
    }

    protected override Task OnParametersSetAsync()
    {
        return Model.OnViewModelParametersSet();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) return Model.OnViewModelAfterRender();

        return base.OnAfterRenderAsync(firstRender);
    }

    private async void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        await InvokeAsync(StateHasChanged);
    }
}