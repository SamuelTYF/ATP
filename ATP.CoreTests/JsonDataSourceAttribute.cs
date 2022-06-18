using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ATP.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class JsonDataSourceAttribute : Attribute, ITestDataSource
    {
        public JArray JSON;
        public JsonDataSourceAttribute(string fname, string key) : base()
        {
            using StreamReader reader = new(fname);
            JSON = JObject.Parse(reader.ReadToEnd())[key] as JArray; 
        }
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();
            Console.WriteLine(parameters);
            JsonSerializer serializer = new();
            foreach (JObject node in JSON)
                yield return parameters.Select(parameter =>
                serializer.Deserialize(node[parameter.Name].CreateReader(),parameter.ParameterType)).ToArray();
        }
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return data[0] == null ? "null" : data[0].ToString();
        }
    }
}
