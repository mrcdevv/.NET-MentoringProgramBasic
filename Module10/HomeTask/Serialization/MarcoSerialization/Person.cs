using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MarcoSerialization
{
    [Serializable]
    [MessagePackObject]
    public class SerializablePerson : ISerializable
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public int Age { get; set; }

        public SerializablePerson() { }

        public SerializablePerson(string name, int age)
        {
            Name = name;
            Age = age;
        }

        protected SerializablePerson(SerializationInfo info, StreamingContext context)
        {
            var data = (byte[])info.GetValue("Data", typeof(byte[]));
            var deserialized = MessagePackSerializer.Deserialize<SerializablePerson>(data);

            Name = deserialized.Name;
            Age = deserialized.Age;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            byte[] data = MessagePackSerializer.Serialize(this);
            info.AddValue("Data", data);
        }

        public override string ToString()
        {
            return $"Name: {Name}, Age: {Age}";
        }
    }
}
