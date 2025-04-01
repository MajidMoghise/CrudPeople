namespace Helpers.FilterSearch
{
    public class FilterModel
    {
        
        public string Field { get; set; }
        
        public FilterOperator Operator { get; set; }
        
        public string Value { get; set; }
        
        public OperatorBetweenFilter? OperatorBetweenFilters { get; set; }
    }
}
