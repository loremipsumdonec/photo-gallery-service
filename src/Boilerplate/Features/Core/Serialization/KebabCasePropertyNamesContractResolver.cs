using Boilerplate.Features.Core.NamingConventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Boilerplate.Features.Core.Serialization
{
    public class KebabCasePropertyNamesContractResolver
        : DefaultContractResolver
    {
        private readonly INamingConvention _convetion;

        public KebabCasePropertyNamesContractResolver()
        {
            _convetion = new KebabCase();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var m = base.CreateProperty(member, memberSerialization);
            m.PropertyName = GetAsSnakeCase(m.PropertyName);
            return m;
        }

        protected override string ResolveExtensionDataName(string extensionDataName)
        {
            string dataName = base.ResolveExtensionDataName(extensionDataName);
            return GetAsSnakeCase(dataName);
        }

        private string GetAsSnakeCase(string text)
        {
            return _convetion.To(text);
        }
    }
}
