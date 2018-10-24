using System.Net.Http;
using Flurl.Http.Configuration;

namespace JohnsonControls.Net.Http
{
    public class HttpClientRedirectFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientRedirectHandler();
        }

    }
}
