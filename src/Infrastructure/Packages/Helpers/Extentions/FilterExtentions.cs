using Helpers.FilterSearch;
using System.Text.Json;
namespace Helpers.Extentions
{
    public static class FilterExtentions
    {
        public static string GetJsonString(this List<FilterModel> sample)
        {
            return JsonSerializer.Serialize(sample);
        }
        public static string GetJsonString(this List<OrderByModel> sample)
        {
            return JsonSerializer.Serialize(sample);
        }
        public static string GetQueryString(this FilterForSampleRequestModel sample)
        {
            var filter = sample.Filters.GetJsonString();
            var selection = string.Join(", ", sample.ResultsFields);
            var groupBy = string.Join(", ", sample.GroupByFields);
            var orderBy = sample.OrderByFields.GetJsonString();

            var queryString = $"?{nameof(SearchRequestModel<object, object>.Filter)}={Uri.EscapeDataString(filter)}" +
                $"&{nameof(SearchRequestModel<object, object>.Selections)}={Uri.EscapeDataString(selection)}" +
                $"&{nameof(SearchRequestModel<object, object>.GroupBies)}={Uri.EscapeDataString(groupBy)}&orderBy={Uri.EscapeDataString(orderBy)}";
            return queryString;
        }
        public static FilterForSampleResponseModel GetResponseModelForSample(this FilterForSampleRequestModel sample)
        {
            var filter = sample.Filters.GetJsonString();
            var selection = string.Join(", ", sample.ResultsFields);
            var groupBy = string.Join(", ", sample.GroupByFields);
            var orderBy = sample.OrderByFields.GetJsonString();

            var queryString = $"?{nameof(SearchRequestModel<object, object>.Filter)}={Uri.EscapeDataString(filter)}" +
                $"&{nameof(SearchRequestModel<object, object>.Selections)}={Uri.EscapeDataString(selection)}" +
                $"&{nameof(SearchRequestModel<object, object>.GroupBies)}={Uri.EscapeDataString(groupBy)}&orderBy={Uri.EscapeDataString(orderBy)}";
            return new FilterForSampleResponseModel
            {
                Filters = filter,
                GroupBy = groupBy,
                OrderBy = orderBy,
                Selection = selection,
                TotalQueryString = queryString

            };
        }
    }

}


