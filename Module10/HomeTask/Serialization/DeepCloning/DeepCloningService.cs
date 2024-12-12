using MessagePack;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DeepCloning
{
    public class DeepCloningService
    {
        public static async Task<T> DeepCloneAsync<T>(T source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            await using var memoryStream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(memoryStream, source);

            memoryStream.Position = 0;

            return await MessagePackSerializer.DeserializeAsync<T>(memoryStream);
        }
    }
}
