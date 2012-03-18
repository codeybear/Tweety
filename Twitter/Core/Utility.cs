using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Tweety.Core
{
    public static class Utility
    {
        private class DictItem<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;
        }

        /// <summary> Serialize a dictionary to XML and save to disk </summary>
        public static void SerializeDictionary<TKey, TValue>(string fileName, Dictionary<TKey, TValue> dict) {
            var saveList = new List<DictItem<TKey, TValue>>();

            foreach (TKey key in dict.Keys)
                saveList.Add(new DictItem<TKey, TValue> { Key = key, Value = dict[key] });

            XmlSerializer serializer = new XmlSerializer(typeof(List<DictItem<TKey, TValue>>));
            using (TextWriter writer = new StreamWriter(fileName)) {
                serializer.Serialize(writer, saveList);
            }
        }

        /// <summary> Deserialize a dictionary from an xml file </summary>
        public static Dictionary<TKey, TValue> DeserializeDictionary<TKey, TValue>(string sFileName) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DictItem<TKey, TValue>>));
            List<DictItem<TKey, TValue>> loadList;

            using (FileStream fs = new FileStream(sFileName, FileMode.Open)) {
                loadList = (List<DictItem<TKey, TValue>>)serializer.Deserialize(fs);
            }

                var loadDict = new Dictionary<TKey, TValue>();

                foreach (DictItem<TKey, TValue> item in loadList)
                    loadDict.Add(item.Key, item.Value);

                return loadDict;
        }

        /// <summary> Access an object using Invoke if required </summary>
        public static void AccessInvoke(ISynchronizeInvoke thisObject, Action action) {
            if (thisObject.InvokeRequired)
                thisObject.Invoke(action, null);
            else
                action();
        }
    }
}

