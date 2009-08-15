using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Core
{
    public class Utility
    {
        public class DictItem<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;
        }

        /// <summary> Serialize a dictionary to XML and save to disk </summary>
        public static void SerializeDictionary<TKey, TValue>(string sFileName, Dictionary<TKey, TValue> Dict) {
            var SaveList = new List<DictItem<TKey, TValue>>();

            foreach (TKey Key in Dict.Keys)
                SaveList.Add(new DictItem<TKey, TValue> { Key = Key, Value = Dict[Key] });

            XmlSerializer serializer = new XmlSerializer(typeof(List<DictItem<TKey, TValue>>));
            TextWriter Writer = new StreamWriter(sFileName);
            serializer.Serialize(Writer, SaveList);
            Writer.Close();
        }

        /// <summary> Deserialize a dictionary from an xml file </summary>
        public static Dictionary<TKey, TValue> DeserializeDictionary<TKey, TValue>(string sFileName) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DictItem<TKey, TValue>>));
            FileStream fs = new FileStream(sFileName, FileMode.Open);
            var LoadList = (List<DictItem<TKey, TValue>>)serializer.Deserialize(fs);
            fs.Close();

            var LoadDict = new Dictionary<TKey, TValue>();

            foreach (DictItem<TKey, TValue> Item in LoadList)
                LoadDict.Add(Item.Key, Item.Value);

            return LoadDict;
        }

        /// <summary> Access an object using Invoke if required </summary>
        public static void AccessInvoke(ISynchronizeInvoke ThisObject, Action action) {
            if (ThisObject.InvokeRequired)
                ThisObject.Invoke(action, null);
            else
                action();
        }
    }
}

