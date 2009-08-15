using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Core
{
    /// <summary> Store a dictionary of byte[] that represent images, items are returned as a bitmap
    /// items are kept as stream as they are compressed in this format and can be serialised </summary>
    class ImageCache
    {
        Dictionary<string, byte[]> _ImageCache = new Dictionary<string, byte[]>();

        public void StoreImage(string sURL, byte[] ImageBytes) {
            _ImageCache[sURL] = ImageBytes;
        }

        public Bitmap GetImage(string sURL) {
            Stream ImageStream = new MemoryStream(_ImageCache[sURL]);
            return new Bitmap(ImageStream);
        }

        public bool ContainsKey(string sURL) {
            return _ImageCache.ContainsKey(sURL);
        }

        public void Save(string sFileName) {
            if(_ImageCache.Count > 0)
                Utility.SerializeDictionary<string, byte[]>(sFileName, _ImageCache); ;
        }

        public void Load(string sFileName) {
            _ImageCache = Utility.DeserializeDictionary<string, byte[]>(sFileName);
        }
    }
}
