using System;
using System.IO;

namespace NetProtocol {
    public static class ProtocolUtils {

        public static T Deserialize<T> (byte[] bytes) {
            using (MemoryStream st = new MemoryStream(bytes)) {
                return (T)ProtoBuf.Serializer.Deserialize<T>(st);
            }
        }

        public static byte[] Serialize<T> (T msg) {
            using (MemoryStream ms = new MemoryStream()) {
                try {
                    ProtoBuf.Serializer.Serialize(ms, msg);

                    byte[] bytes = new byte[ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(), 0, bytes, 0, (int)ms.Length);
                    return bytes;
                }
                catch (Exception ex) {
                    LogError(ex.ToString());
                }

                return null;
            }
        }


        private static void LogError (string msg) {
            var _color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(msg);
            Console.ForegroundColor = _color;
        }
    }
}

