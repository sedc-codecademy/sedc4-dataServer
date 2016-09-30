using System.Linq;
using System.Text;

namespace DemoServer
{
    internal class BinaryResponseData : ResponseData<byte[]>
    {
        public override byte[] GetBytes()
        {
            string headers = MakeHeader(Headers, Payload.Length);
            var headerBytes = Encoding.UTF8.GetBytes(headers);

            var resultBytes = headerBytes.ToList().Concat(Payload).ToArray();
            return resultBytes;
        }

    }
}