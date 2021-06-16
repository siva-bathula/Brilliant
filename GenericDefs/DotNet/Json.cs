using System.Web.Script.Serialization;

namespace GenericDefs.DotNet
{
    public class Json
    {
        public static object CreateDynamicObject(string json)
        {
            var serializer = new JavaScriptSerializer();
            dynamic value = serializer.DeserializeObject(json);

            return value;
        }
    }
}