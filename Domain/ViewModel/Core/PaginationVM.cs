namespace Domain.ViewModel.Core;

public class PaginationVM
{
    public string Controller { get; set; }
    public string Action { get; set; }
    public int Page { get; set; }
    public int PageCount { get; set; }
}
