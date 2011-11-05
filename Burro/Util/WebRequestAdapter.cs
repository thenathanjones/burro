using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Burro.Util
{
    public class WebRequestAdapter : IWebRequest
    {
        private WebRequest _request;

        public void Create(string url)
        {
            _request = WebRequest.Create(url);
        }

        public void SetCredentials(string username, string password) 
        {
            var credentials = new NetworkCredential(username, password);

            _request.Credentials = credentials;
        }

        public Stream GetResponseStream()
        {
            return _request.GetResponse().GetResponseStream();
        }
    }
}
