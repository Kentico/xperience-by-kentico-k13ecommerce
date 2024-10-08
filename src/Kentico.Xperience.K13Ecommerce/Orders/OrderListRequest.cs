﻿namespace Kentico.Xperience.K13Ecommerce.Orders;

/// <summary>
/// Order list request.
/// </summary>
public class OrderListRequest
{
    /// <summary>
    /// Index of page starts from 1.
    /// </summary>
    public required int Page { get; set; }

    /// <summary>
    /// Max. value is 100.
    /// </summary>
    public required int PageSize { get; set; }

    /// <summary>
    /// Order by.
    /// </summary>
    public required string OrderBy { get; set; }

    /// <summary>
    /// Customer ID, leave null or zero for all customers.
    /// </summary>
    public int? CustomerId { get; set; }
}
