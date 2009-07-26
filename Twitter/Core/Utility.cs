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

        /// <summary> DeSerialize a dictionary from an xml file </summary>
        public static Dictionary<TKey, TValue> DeSerializeDictionary<TKey, TValue>(string sFileName) {
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

        //public static void CloseAlert() {
        //    Int16 a = default(Int16);u

        //    for (a = 0; a <= 50; a++) {
        //        System.Threading.Thread.Sleep(5);
        //        m_frmStatusInst.Top = m_frmStatusInst.Top + 2;
        //        m_frmStatusInst.Height = m_frmStatusInst.Height - 2;
        //        m_frmStatusInst.Refresh();
        //    }

        //    m_frmStatusInst.Hide();
        //}

        //public static void CheckVersionForUpgrade() {
        //    System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
        //    string sAppVersion = a.GetName().Version.ToString();

        //    if (Properties.Settings.Default.ApplicationVersion != sAppVersion) {
        //        Properties.Settings.Default.Upgrade();
        //        Properties.Settings.Default.ApplicationVersion = sAppVersion;
        //    }
        //}
    }
}

