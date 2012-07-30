using System;
using System.Collections.Generic;

namespace OAuth
{
    /// <summary>
    /// Comparer class used to perform the sorting of the query parameters
    /// </summary>
    internal class QueryParameterComparer : IComparer<QueryParameter>
    {
        public int Compare(QueryParameter x, QueryParameter y)
        {
            if (x.Name == y.Name)
            {
                return String.Compare(x.Value, y.Value);
            }
            return String.Compare(x.Name, y.Name);
        }
    }
}