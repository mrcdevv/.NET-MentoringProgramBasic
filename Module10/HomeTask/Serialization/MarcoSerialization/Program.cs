namespace MarcoSerialization
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(basePath, "person.msgpack");

            var person = new SerializablePerson("Marco", 23);

            await MessagePackSerializerService.SerializeAsync(person, filePath);
            Console.WriteLine($"Department serialized to binary format and saved at: {filePath}");

            SerializablePerson deserializedPerson = await MessagePackSerializerService.DeserializeAsync<SerializablePerson>(filePath);
            Console.WriteLine("Object Deserialized: ");
            Console.WriteLine(deserializedPerson);
        }
    }
}
