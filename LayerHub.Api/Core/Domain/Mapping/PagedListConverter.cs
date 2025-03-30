using AutoMapper;
using LayerHub.Shared.Utils;

namespace LayerHub.Api.Core.Domain.Mapping;

public class PagedListConverter<TIn, TOut> : ITypeConverter<PaginatedList<TIn>, PaginatedList<TOut>>
{

    public PaginatedList<TOut> Convert(PaginatedList<TIn> source, PaginatedList<TOut> destination, ResolutionContext context)
    {
        var mapped = context.Mapper.Map<List<TOut>>(source);

        return new PaginatedList<TOut>(mapped, source.TotalPages, source.TotalItems, source.PageIndex, source.ItemsPerPage);
    }
}
