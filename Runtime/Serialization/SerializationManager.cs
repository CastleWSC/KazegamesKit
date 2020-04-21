using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace KazegamesKit.Serialization
{
    public class SerializationManager
    {

        public static bool Serialize(string saveName, object saveData)
        {
            var formatter = GetBinaryFormatter();

            if(!Directory.Exists(Path.Combine(Application.persistentDataPath, "saves")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "saves"));
            }

            string path = Path.Combine(Application.persistentDataPath, "saves", saveName + ".sav");
            FileStream file = File.Create(path);
            formatter.Serialize(file, saveData);
            file.Close();

            return true;
        }

        public static object Deserialize(string path)
        {
            if (!File.Exists(path))
                return null;

            var formatter = GetBinaryFormatter();

            FileStream file = File.Open(path, FileMode.Open);

            try
            {
                object saveData = formatter.Deserialize(file);
                file.Close();

                return saveData;
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
                file.Close();

                return null;
            }
        }

        public static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            SurrogateSelector selector = new SurrogateSelector();

            Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
            QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();

            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}
