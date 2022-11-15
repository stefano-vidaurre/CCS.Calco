namespace CCS.Calco.Configurations
{
    internal class OrderDirection
    {
        public string PropertyName { get; private set; }
        public bool IsDescending { get; private set; }

        public OrderDirection(string propertyName, bool isDescending)
        {
            PropertyName = propertyName;
            IsDescending = isDescending;
        }
    }
}