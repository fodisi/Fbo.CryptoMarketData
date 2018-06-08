using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fbo.CryptoMarketData.UnitTest.Core
{
    public class IgnorePropertySerializerContractResolver : DefaultContractResolver
    {
        private HashSet<string> ignoredProperties;

        public IgnorePropertySerializerContractResolver(IEnumerable<string> ignoredPropertyNames)
        {
            if (ignoredPropertyNames != null)
            {
                ignoredProperties = new HashSet<string>(ignoredPropertyNames, StringComparer.OrdinalIgnoreCase);
            }
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            List<MemberInfo> serializableMembers = null;
            var allMembers = base.GetSerializableMembers(objectType);

            if (ignoredProperties != null && ignoredProperties.Count > 0)
            {
                serializableMembers = allMembers.Where(m => !ignoredProperties.Contains(m.Name)).ToList();
            }
            return serializableMembers != null && serializableMembers.Count > 0 ? serializableMembers : allMembers;
        }

    }
}
