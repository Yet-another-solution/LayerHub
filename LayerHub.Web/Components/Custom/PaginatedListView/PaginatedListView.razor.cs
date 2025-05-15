using LayerHub.Shared.Dto;
using LayerHub.Shared.Utils;
using LayerHub.Web.Application.Services;
using LayerHub.Web.Components.Custom.Base;
using Microsoft.AspNetCore.Components;
using IToastService = Blazored.Toast.Services.IToastService;

namespace LayerHub.Web.Components.Custom.PaginatedListView;

public partial class PaginatedListView<T> : CustomComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IHttpService HttpService { get; set; } = null!;
    [Inject] public IToastService ToastService { get; set; } = null!;

    /// <summary>
    /// Data query result
    /// </summary>
    private IQueryable<T>? _dataQuery;

    /// <summary>
    /// Paginated list of data returned from server
    /// </summary>
    private PaginatedListDto<T> _paginatedList = new PaginatedListDto<T>(new PaginatedList<T>(new List<T>(), 0, 0, 0, 0));

    /// <summary>
    /// Paginator for data
    /// </summary>
    [Parameter]
    public BasePaginator Paginator { get; set; } = new BasePaginator();

    /// <summary>
    /// Search query for data
    /// </summary>
    private string? _query;

    /// <summary>
    /// Number of items per page
    /// </summary>
    private string _numOfItems = "50";

    /// <summary>
    /// Uri for getting data from API
    /// </summary>
    [Parameter]
    public required string GetUrl { get; set; }

    /// <summary>
    /// Edit uri for redirecting to edit page
    /// </summary>
    [Parameter]
    public string? EditUrl { get; set; }

    /// <summary>
    /// Parameter for edit url
    /// </summary>
    [Parameter]
    public string RouteParam { get; set; } = "Id";

    /// <summary>
    /// Parameter for details url
    /// </summary>
    [Parameter]
    public string DetailsParam { get; set; } = "Id";

    /// <summary>
    /// If exists, Url for creating new item
    /// </summary>
    [Parameter]
    public string? CreateUrl { get; set; }

    /// <summary>
    /// Edit uri for redirecting to edit page
    /// </summary>
    [Parameter]
    public string? DeleteUrl { get; set; }

    /// <summary>
    /// Details uri for redirecting to details page
    /// </summary>
    [Parameter]
    public string? DetailsUrl { get; set; }

    /// <summary>
    /// Cancellation token for API call
    /// </summary>
    [Parameter]
    public CancellationToken CancellationToken { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the string representation of the grid template columns.
    /// </summary>
    /// <remarks>
    /// The grid template columns property allows you to define the columns of a CSS grid
    /// layout using a string representation. Each column can be defined using a specific
    /// size or a keyword.
    /// The format of the string representation is as follows:
    /// - Each column is defined using a value that represents the width or size of the column.
    /// - Multiple columns are separated by a whitespace or comma.
    /// - Columns can be defined using a fixed size value, such as pixels ("px"), percentage ("%"),
    /// or a keyword such as "auto" or "minmax()".
    /// Examples of valid grid template columns string representations include:
    /// - "100px 200px 300px" - Defines three columns with fixed sizes of 100 pixels, 200 pixels, and 300 pixels.
    /// - "20% auto" - Defines two columns, one with a width of 20% and the other taking up the remaining space.
    /// - "1fr 2fr 1fr" - Defines three columns with relative sizes of 1, 2, and 1 respectively.
    /// </remarks>
    [Parameter]
    public string? GridTemplateColumns { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    /// <summary>
    /// Load data from API
    /// </summary>
    private async Task LoadData()
    {
        if (_query != null)
            Paginator.Query = _query;
        Paginator.ItemsPerPage = int.Parse(_numOfItems);
        _paginatedList = await HttpService.Get<PaginatedListDto<T>>($"{GetUrl}?{Paginator.GetAsParams()}", CancellationToken) ??
                         new PaginatedListDto<T>(new PaginatedList<T>(new List<T>(), 0, 0, 0, 0));
        _dataQuery = _paginatedList.Items.AsQueryable();
    }

    /// <summary>
    /// Load next page of data
    /// </summary>
    private async Task NextPage()
    {
        Paginator.Page += 1;
        await LoadData();
    }

    /// <summary>
    /// Load previous page of data
    /// </summary>
    private async Task PrevPage()
    {
        Paginator.Page -= 1;
        await LoadData();
    }

    /// <summary>
    /// Load first page of data
    /// </summary>
    private async Task FirstPage()
    {
        Paginator.Page = 1;
        await LoadData();
    }

    /// <summary>
    /// Load last page of data
    /// </summary>
    private async Task LastPage()
    {
        Paginator.Page = _paginatedList.TotalPages;
        await LoadData();
    }

    /// <summary>
    /// Redirect to edit page
    /// </summary>
    /// <param name="item"></param>
    private void RedirectToEdit(T item)
    {
        var id = item?.GetType().GetProperty(RouteParam)?.GetValue(item, null);
        if (id is not null)
        {
            if (EditUrl.Contains("{" + RouteParam + "}"))
            {
                NavigationManager.NavigateTo(EditUrl.Replace("{" + RouteParam + "}", $"{id}"));
            } else
            {
                NavigationManager.NavigateTo($"{EditUrl}/{id}");
            }
        }
    }

    /// <summary>
    /// Redirect to details page
    /// </summary>
    /// <param name="item"></param>
    private void RedirectToDetails(T item)
    {
        var id = item?.GetType().GetProperty(DetailsParam)?.GetValue(item, null);
        if (id is not null)
        {
            NavigationManager.NavigateTo($"{DetailsUrl}/{id}");
        }
    }

    /// <summary>
    /// Redirect to create page
    /// </summary>
    private void RedirectToCreate()
    {
        NavigationManager.NavigateTo($"{CreateUrl}");
    }

    private async Task Delete(T item)
    {
        var id = item?.GetType().GetProperty(RouteParam)?.GetValue(item, null);
        await HttpService.Delete($"{DeleteUrl}/{id}");

        ToastService.ShowSuccess("Položka bola vymazaná");
        await LoadData();
    }
}
