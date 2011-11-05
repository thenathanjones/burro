using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Burro.Util
{
    public interface IWebRequest
    {
        void Create(string url);

        void SetCredentials(string username, string password);

        Stream GetResponseStream();
    }
}
