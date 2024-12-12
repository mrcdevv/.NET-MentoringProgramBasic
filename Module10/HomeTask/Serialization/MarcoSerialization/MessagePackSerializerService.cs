using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MarcoSerialization
{
    public static class MessagePackSerializerService
    {
        public static async Task SerializeAsync<T>(T obj, string filePath)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            byte[] data = MessagePackSerializer.Serialize(obj);
            await File.WriteAllBytesAsync(filePath, data);
        }

        public static async Task<T> DeserializeAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"El archivo {filePath} no existe.");

            byte[] data = await File.ReadAllBytesAsync(filePath);
            return MessagePackSerializer.Deserialize<T>(data);
        }
    }
}
