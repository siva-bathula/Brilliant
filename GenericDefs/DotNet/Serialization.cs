using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace GenericDefs.DotNet
{
    public class Serialization
    {
        public static bool Serialize<T>(Answer<T> answer, SaveProgressContext context)
        {
            if (SerializeObject(answer, context.FileName)) { return true; }
            else return false;
        }

        public static Answer<T> DeSerialize<T>(SaveProgressContext context)
        {
            if (string.IsNullOrEmpty(context.FileName)) { throw new Exception("Filename cannot be empty."); }
            
            Answer<T> objectOut = DeSerializeObject<Answer<T>>(context.FileName);

            return objectOut;
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        internal static bool SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return false; }
            bool success = false;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (FileStream writer = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    // For XmlWriter, it uses the stream that we created: writer.
                    using (XmlWriter xmlWriter = XmlWriter.Create(writer))
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            serializer.Serialize(stream, serializableObject);
                            stream.Position = 0;
                            xmlDocument.Load(stream);
                            xmlDocument.Save(xmlWriter);
                            stream.Close();
                        }
                        xmlWriter.Flush();
                        xmlWriter.Close();
                    }
                    writer.Flush();
                    writer.Close();
                }
                success = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }
            return success;
        }

        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }

            return objectOut;
        }

        /// <summary>
        /// Deserializes an input xml into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static T DeSerializeObjectFromXml<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }

            GC.Collect();

            return objectOut;
        }
    }

    [Serializable]
    public class Batch
    {
        public int MaxSize { get; set; }
        public int CurrentMin { get; set; }
        public int CurrentMax { get; set; }
        public int CurrentRange { get; set; }
    }

    [Serializable]
    public class Answer<T>
    {
        public List<Batch> Batches;
        public T LastSavedAnswer;
    }

    [Serializable]
    public class ProcessedBatch<T> { 
        public T LastCalculated;
        public Batch Batch;
    }

    /// <summary>
    /// Always save progress to the same file.
    /// Save List of Answers, so chronologically saved progress can be seen.
    /// </summary>
    public class SaveProgressContext {
        public string Context = Guid.NewGuid().ToString();
        public FileMode Mode;
        public string FileName
        {
            get
            {
                return string.Format(@"{0}.txt", Context);
            }
        }

        internal bool ExtendingPreviousLog { get; set; }

        public SaveProgressContext(string existingFile)
        {
            Context = existingFile;
            ExtendingPreviousLog = true;
            Mode = FileMode.Append;
        }

        public SaveProgressContext()
        {
            Context = Guid.NewGuid().ToString();
            Mode = FileMode.OpenOrCreate;
        }
    }
}