using Microsoft.AspNetCore.Mvc.Internal;

namespace Helpers.FilterSearch
{
    public class FilterForSampleRequestModel
    {
        private List<FilterModel> _filterModels;

        public List<FilterModel> Filters
        {
            get { return _filterModels; }
            set {
                ValidationFilter(value);
                _filterModels = value; 
            
            }
        }

        
        public List<string> ResultsFields { get; set; }
        public List<string> GroupByFields { get; set; }


        public List<OrderByModel> OrderByFields{ get; set; }
        private void ValidationFilter(List<FilterModel> filters)
        {
            if(filters.Count==1)
            {
                if (filters[0].OperatorBetweenFilters is not null) {
                    throw new ArgumentException($"Field '{nameof(FilterModel.OperatorBetweenFilters)}' must be null.");
                }
            }
            if(filters.Count>1)
            {
                if (filters[0].OperatorBetweenFilters is  null)
                {
                    throw new ArgumentException($"First field '{nameof(FilterModel.OperatorBetweenFilters)}' must be not null.");
                }
                if (filters[filters.Count-1].OperatorBetweenFilters is not null)
                {
                    throw new ArgumentException($"Last field '{nameof(FilterModel.OperatorBetweenFilters)}' must be null.");
                }
            }
        }
    }
    public class FilterForSampleResponseModel
    {
        public string Filters { get; set; }
        public string OrderBy { get; set; }
        public string Selection { get; set; }
        public string GroupBy { get; set; }
        public string TotalQueryString { get; set; }
    }
    
    
}

