﻿using MediatR;

using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.QueryParams;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to List All
    /// </summary>
    public record ListAllCoversQuery(CoverQueryParams QueryParams) : IRequest<IReadOnlyList<Cover>>;
}
