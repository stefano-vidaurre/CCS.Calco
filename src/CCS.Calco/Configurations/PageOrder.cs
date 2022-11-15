using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CCS.Calco.Configurations
{
    internal class PageOrder
    {
        private static readonly string _descendingValue = "des";
        private static readonly string _ascendingValue = "asc";
        private static readonly Regex _regex = new Regex(@"^[a-zA-Z_][\w-]*(:(des|asc))?(;[a-zA-Z_][\w-]*(:(des|asc))?)*;?$");
        private string _value;

        public bool IsValid { get; private set; }

        public PageOrder()
        {
            _value = string.Empty;
            IsValid = false;
        }

        public PageOrder(string value)
        {
            _value = value;
            IsValid = _regex.IsMatch(value);
        }

        public static implicit operator PageOrder(string value)
        {
            return new PageOrder(value);
        }

        public IEnumerable<OrderDirection> GetOrders()
        {
            if (!IsValid)
            {
                return Array.Empty<OrderDirection>();
            }

            IList<OrderDirection> result = new List<OrderDirection>();
            IEnumerable<string> orders = _value.Split(';');

            foreach (string order in orders)
            {
                if (string.IsNullOrWhiteSpace(order))
                {
                    continue;
                }

                OrderDirection orderDirection = ParseOrder(order);
                result.Add(orderDirection);
            }

            return result;
        }

        private OrderDirection ParseOrder(string order)
        {
            string[] orderAndDirection = order.Split(':');

            if (orderAndDirection.Length > 1)
            {
                bool isDescending = orderAndDirection[1].Equals(_descendingValue);
                return new OrderDirection(orderAndDirection[0], isDescending);
            }

            return new OrderDirection(orderAndDirection[0], false);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}