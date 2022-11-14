using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace CCS.Calco
{
    public abstract class CalcoFilter<T>
    {
        private IDictionary<string, Expression<Func<T, object>>> _orderMaps;

        public string PageOrder { get; set; }
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }

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

            if (!(PageOrder is null))
            {
                IEnumerable<string> orders = PageOrder.Split(';');


                foreach (string order in orders)
                {
                    string[] orderAndDirection = order.Split(':');
                    Expression<Func<T, object>> property = _orderMaps[orderAndDirection[0]];

                    if (orderAndDirection.Length > 1)
                    {
                        query = query.OrderByDescending(property);
                    }
                    else
                    {
                        query = query.OrderBy(property);
                    }
                }
            }

            if (!(PageSize is null))
            {
                query = query.Skip(PageIndex * PageSize.Value).Take(PageSize.Value);
            }

            return query;
        }
    }
}
