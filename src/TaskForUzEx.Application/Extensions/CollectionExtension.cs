using Newtonsoft.Json;
using TaskForUzEx.Application.Helpers;
using TaskForUzEx.Domain.Commons;
using TaskForUzEx.Domain.Configurations;
using TaskForUzEx.Domain.Exceptions;

namespace TaskForUzEx.Application.Extensions;

public static class CollectionExtension
{
    public static IQueryable<T> ToPagedList<T>(this IQueryable<T> sources,
        PaginationParams @params = null) where T : Auditable
    {

        var metaData = new PaginationMetaData(sources.Count(), @params);

        var json = JsonConvert.SerializeObject(metaData);

        if (HttpContextHelper.ResponseHeaders != null)
        {
            HttpContextHelper.ResponseHeaders.Remove("Pagination");
            HttpContextHelper.ResponseHeaders.Add("Pagination", json);
            HttpContextHelper.ResponseHeaders.Add("Access-Control-Expose-Headers", "Pagination");
        }

        return @params.PageIndex > 0 && @params.PageSize > 0 ?
            sources.OrderByDescending(p => p.CreatedAt)
                .Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize) :
            throw new CustomException(405, "Please, enter valid numbers");
    }
}