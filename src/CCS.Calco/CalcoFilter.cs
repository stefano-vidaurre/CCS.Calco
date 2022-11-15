using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using CCS.Calco.Configurations;

namespace CCS.Calco
{
    public abstract class CalcoFilter<T>
    {
        private IDictionary<string, Expression<Func<T, object>>> _orderMaps;
        private PageOrder _pageOrder;

        public string PageOrder
        {
            get => _pageOrder.ToString();
            set => _pageOrder = value;
        }

        public string PageIndex { get; set; }
        public string PageSize { get; set; }

        public bool IsValid => _pageOrder.IsValid;
        public string ErrorMessage => GetErrorMessage();

        public CalcoFilter()
        {
            _orderMaps = new Dictionary<string, Expression<Func<T, object>>>();
        }

        protected CalcoFilter<T> MapOrder<U>(Expression<Func<U, object>> filterProperty, Expression<Func<T, object>> entityProperty)
        {
            if (!(filterProperty.Body is MemberExpression))
            {
                return this;
            }

            string filterPropertyName = (filterProperty.Body as MemberExpression).Member.Name;
            return MapOrder(filterPropertyName, entityProperty);
        }

        protected CalcoFilter<T> MapOrder(string orderPropertyName, Expression<Func<T, object>> entityProperty)
        {
            if (!(entityProperty.Body is MemberExpression))
            {
                return this;
            }

            _orderMaps.Add(orderPropertyName, entityProperty);

            return this;
        }

        public IQueryable<T> CreateQuery(IEnumerable<T> data)
        {
            IQueryable<T> query = data.AsQueryable<T>();
            query = OrderQuery(query);

            if (!(PageSize is null))
            {
                query = query.Skip(int.Parse(PageIndex) * int.Parse(PageSize)).Take(int.Parse(PageSize));
            }

            return query;
        }

        private IQueryable<T> OrderQuery(IQueryable<T> query)
        {
            if (_pageOrder is null)
            {
                return query;
            }

            IEnumerable<OrderDirection> orders = _pageOrder.GetOrders();

            foreach (OrderDirection order in orders)
            {
                Expression<Func<T, object>> property = _orderMaps[order.PropertyName];
                query = order.IsDescending ? query.OrderByDescending(property) : query.OrderBy(property);
            }

            return query;
        }

        private bool GetIsValid()
        {
            return _pageOrder.IsValid && AreOrdersMapped();
        }

        private bool AreOrdersMapped() {
            foreach (OrderDirection order in _pageOrder.GetOrders())
            {
                if (!_orderMaps.ContainsKey(order.PropertyName))
                {
                    return false;
                }
            }

            return true;
        }

        private string GetErrorMessage()
        {
            string message = "";

            if (IsValid)
            {
                return null;
            }

            if (!_pageOrder.IsValid)
            {
                message += "The page order format is not valid.";
            }

            return message;
        }
    }
}
