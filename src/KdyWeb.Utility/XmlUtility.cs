using System.IO;
using System.Xml.Serialization;

namespace KdyWeb.Utility
{
    /// <summary>
    /// Xml工具类
    /// </summary>
    public static class XmlUtility
    {
        /// <summary>
        /// Xml转实体类（反序列化）
        /// </summary>
        /// <typeparam name="T">泛型实体类 需要有xmlroot属性</typeparam>
        /// <param name="xmlStr">Xml字符串</param>
        /// <returns></returns>
        public static T XmlToModel<T>(this string xmlStr)
        {
            using var reader = new StringReader(xmlStr);
            var xmlNode = new XmlSerializer(typeof(T));
            var model = (T)xmlNode.Deserialize(reader);
            return model;
        }

        /// <summary>
        /// 实体类转Xml（序列化）
        /// </summary>
        /// <returns></returns>
        public static string ModelToXml(this object model)
        {
            using var stream = new MemoryStream();
            var xml = new XmlSerializer(model.GetType());

            //移除xmlns：xsd命名空间
            var xsn = new XmlSerializerNamespaces();
            //添加一个空的值
            xsn.Add("", "");

            //序列化对象  
            xml.Serialize(stream, model, xsn);
            stream.Position = 0;

            using var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }
    }
}
