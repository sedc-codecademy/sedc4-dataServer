using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataServer.Entities
{
    public class HeaderCollection : IEnumerable<Header>
    {
        private Dictionary<string, Header> innerHeaders = new Dictionary<string, Header>();

        public IEnumerator<Header> GetEnumerator()
        {
            return innerHeaders.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerHeaders.Values.GetEnumerator();
        }

        public Header Get(string name)
        {
            name = name.ToLower();
            if (!innerHeaders.ContainsKey(name))
                return null;
            return innerHeaders[name];
        }

        public bool Add(Header header)
        {
            var name = header.Name.ToLower();
            if (innerHeaders.ContainsKey(name))
                return false;

            innerHeaders.Add(name, header);
            return true;
        }

        public bool Add(string name, string value)
        {
            var header = new Header { Name = name, Value = value };
            return Add(header);
        }

        public void AddRange(IEnumerable<Header> headers)
        {
            foreach (var header in headers)
            {
                Add(header);
            }
        }

    }
}