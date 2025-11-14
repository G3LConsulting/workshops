namespace TaskApi.Models.DTOs;

/// <summary>
/// Paginated response wrapper for task lists
/// </summary>
/// <typeparam name="T">Type of items in the response</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>
    /// List of items for the current page
    /// </summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Current page number (1-indexed)
    /// </summary>
    /// <example>1</example>
    public int PageNumber { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    /// <example>10</example>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    /// <example>45</example>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    /// <example>5</example>
    public int TotalPages { get; set; }

    /// <summary>
    /// Indicates if there is a previous page
    /// </summary>
    /// <example>false</example>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Indicates if there is a next page
    /// </summary>
    /// <example>true</example>
    public bool HasNextPage { get; set; }
}
