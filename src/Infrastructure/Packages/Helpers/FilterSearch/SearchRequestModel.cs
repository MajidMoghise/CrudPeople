using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Helpers.FilterSearch
{
    public class SearchRequestModel<TRequestModel, TResultModel>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 5;
        
        
        private string _selection;
        public string Selections
        {
            get => _selection;
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    SelectionValidation(value);
                    _selection = value;
                }


            }
        }
        
        
        private string _filter;
        public string Filter
        {
            get => _filter;
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    FilterValidation(value);
                    _filter = value;
                }

            }
        }
        
        
        private string _groupBies;
        public string GroupBies
        {
            get => _groupBies;
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    GroupByValidation(value);
                    _groupBies = value;
                }

            }
        }


        private string _orderBies;

        public string OrderBies
        {
            get => _orderBies;
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    OrderByValidation(value);
                    _orderBies = value;
                }

            }
        }

        private List<FilterModel> _filters;
        private List<string> _resultsFields;
        private List<string> _groupByFields;
        private List<OrderByModel> _orderByModels;
        private void GroupByValidation(string _groupBys)
        {
            var sels = new List<string>();
            if (String.IsNullOrEmpty(_groupBys))
            {
                throw new ArgumentException($"Field of '{nameof(GroupBies)}' must be not null  .");
            }
            if (_groupBys.Contains(","))
            {
                sels = _groupBys.Split(',').ToList();
            }
            else
            {
                sels.Add(_groupBys);
            }
            var requestProperties = typeof(TResultModel).GetProperties().Select(p => p.Name).ToList();
            foreach (var select in sels)
            {
                if (!requestProperties.Contains(select))
                {
                    throw new ArgumentException($"Field '{select}' does not exist in type '{typeof(TResultModel).Name}'.");
                }
            }
            _groupByFields = sels;
        }
        private void SelectionValidation(string _selection)
        {
            var sels = new List<string>();
            if (String.IsNullOrEmpty(_selection))
            {
                return;
            }
            if (_selection != "*" && _selection.Contains(","))
            {
                sels = _selection.Split(',').ToList();
            }
            else if (_selection != "*" && !_selection.Contains(","))
            {
                sels.Add(_selection);
            }
            var requestProperties = typeof(TResultModel).GetProperties().Select(p => p.Name).ToList();
            foreach (var select in sels)
            {
                if (!requestProperties.Contains(select))
                {
                    throw new ArgumentException($"Field '{select}' does not exist in type '{typeof(TResultModel).Name}'.");
                }
            }
            _resultsFields=sels;    
        }
        private void FilterValidation(string _filter)
        {
            var sels = new List<FilterModel>();
            if(_filter.Contains("'"))
            {
                throw new ArgumentException("Dangerios charecter");
            }
            _filter = _filter.Replace("\\\"", "\"").Trim('"'); 
            if (!_filter.StartsWith("[{"))
            {
                throw new ArgumentException("Filter must be start with '[{'.");
            }
            if (!_filter.EndsWith("}]"))
            {
                throw new ArgumentException("Filter must be end with '}]'.");
            }
            try
            {
                sels = System.Text.Json.JsonSerializer.Deserialize<List<FilterModel>>(_filter);
            }
            catch (Exception)
            {
                var samples = new List<FilterModel>() {
                    new FilterModel {Field="Property1",Operator=FilterOperator.Equal,Value="Value1",OperatorBetweenFilters=OperatorBetweenFilter.AND                }, 
                    new FilterModel {Field="Property2",Operator=FilterOperator.Equal,Value="Value2",OperatorBetweenFilters=OperatorBetweenFilter.AND                }, 
                };
                throw new ArgumentException($"Filter must be Json Array like {JsonSerializer.Serialize(samples)}");
            }
            if (sels.Count == 1)
            {
                if (sels[0].OperatorBetweenFilters is not null)
                {
                    throw new ArgumentException($"Field '{nameof(FilterModel.OperatorBetweenFilters)}' must be null.");
                }
            }
            if (sels.Count > 1)
            {
                if (sels[0].OperatorBetweenFilters is null)
                {
                    throw new ArgumentException($"First field '{nameof(FilterModel.OperatorBetweenFilters)}' must be not null.");
                }
                if (sels[sels.Count - 1].OperatorBetweenFilters is not null)
                {
                    throw new ArgumentException($"Last field '{nameof(FilterModel.OperatorBetweenFilters)}' must be null.");
                }
            }
            var requestProperties = typeof(TRequestModel).GetProperties().Select(p => p.Name).ToList();
            foreach (var select in sels)
            {
                if (!requestProperties.Contains(select.Field))
                {
                    throw new ArgumentException($"Field '{select}' does not exist in type '{typeof(TRequestModel).Name}'.");
                }
            }
            _filters = sels;
        }
        private void OrderByValidation(string _orderBy)
        {
            var sels = new List<OrderByModel>();

            if (!_orderBy.StartsWith("[{"))
            {
                throw new ArgumentException("Filter must be start with '[{'.");
            }
            if (!_orderBy.EndsWith("}]"))
            {
                throw new ArgumentException("Filter must be end with '}]'.");
            }
            try
            {
                sels = JsonSerializer.Deserialize<List<OrderByModel>>(_orderBy);
            }
            catch (Exception)
            {
                var samples = new List<OrderByModel>() {
                    new OrderByModel {Field="Property1",OrderByType=OrderByType.Ascending}, 
                    new OrderByModel {Field="Property2",OrderByType = OrderByType.Descending}, 
                };
                throw new ArgumentException($"Filter must be Json Array like {JsonSerializer.Serialize(samples)}");
            }

            var requestProperties = typeof(TRequestModel).GetProperties().Select(p => p.Name).ToList();
            foreach (var select in sels)
            {
                if (!requestProperties.Contains(select.Field))
                {
                    throw new ArgumentException($"Field '{select}' does not exist in type '{typeof(TRequestModel).Name}'.");
                }
            }
            _orderByModels = sels;
        }
        private string GetSqlOperator(FilterOperator filterOperator, string value)
        {
            return filterOperator switch
            {
                FilterOperator.Equal => $"= '{value}'",
                FilterOperator.Greater => $"> '{value}'",
                FilterOperator.Smaller => $"< '{value}'",
                FilterOperator.GreaterEqual => $">= '{value}'",
                FilterOperator.SmallerEqual => $"<= '{value}'",
                FilterOperator.Unequal => $"!= '{value}'",
                FilterOperator.LikeFromBothSides => $"LIKE '%{value}%'",
                FilterOperator.LikeFromBeginning => $"LIKE '{value}%'",
                FilterOperator.LikeFromEnd => $"LIKE '%{value}'",
                _ => throw new NotImplementedException()
            };
        }

        private string GetMongoDbOperator(FilterOperator filterOperator, string value)
        {
            return filterOperator switch
            {
                FilterOperator.Equal => $"$eq: '{value}'",
                FilterOperator.Greater => $"$gt: '{value}'",
                FilterOperator.Smaller => $"$lt: '{value}'",
                FilterOperator.GreaterEqual => $"$gte: '{value}'",
                FilterOperator.SmallerEqual => $"$lte: '{value}'",
                FilterOperator.Unequal => $"$ne: '{value}'",
                FilterOperator.LikeFromBothSides => $"$regex: '.*{value}.*'",
                FilterOperator.LikeFromBeginning => $"$regex: '^{value}.*'",
                FilterOperator.LikeFromEnd => $"$regex: '.*{value}$'",
                _ => throw new NotImplementedException()
            };
        }

        public FormattableString ToSqlQuery()
        {
            var selectFields = string.Join(", ", _resultsFields);
            var whereClause = string.Join(" ", _filters.Select((f, i) => $"{f.Field} {GetSqlOperator(f.Operator, f.Value)}" + (i < _filters.Count - 1 ? $" {f.OperatorBetweenFilters}" : "")));
            var orderByClause = string.Join(", ", _orderByModels.Select(o => $"{o.Field} {o.OrderByType}"));
            return FormattableStringFactory.Create($"SELECT {selectFields} FROM TableName WHERE {whereClause} ORDER BY {orderByClause}");
        }

        public string ToMongoDbQuery()
        {
            var filters = _filters.Select(f => $"{{ {f.Field}: {{ {GetMongoDbOperator(f.Operator, f.Value)} }} }}");
            var query = string.Join(", ", filters);
            var orderByClause = string.Join(", ", _orderByModels.Select(o => $"{{ {o.Field}: {(o.OrderByType == OrderByType.Ascending ? 1 : -1)} }}"));
            return $"{{ $and: [{query}], $orderby: [{orderByClause}] }}";
        }

        public static SearchRequestModel<TRequestModel, TResultModel> Create(string selection,List<FilterModel> filters,List<OrderByModel> orderBies,List<string> groupBies,int page=1,int size=5)
        {


            return new SearchRequestModel<TRequestModel, TResultModel>
            {
                Filter = System.Text.Json.JsonSerializer.Serialize(filters),
                GroupBies = string.Join(",", orderBies),
                OrderBies = JsonSerializer.Serialize(orderBies),
                Page = page,
                Size = size,
                Selections = selection
            };
        }

    }
}
