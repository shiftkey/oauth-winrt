namespace OAuth
{
    /// <summary>
    /// Provides an internal structure to sort the query parameter
    /// </summary>
    public sealed class QueryParameter
    {
        public QueryParameter(string key, string val)
        {
            Name = key;
            Value = val;
        }

        public string Name { get; private set; }

        public string Value { get; private set; }
    }
}