using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JohnsonControls.Net.Http
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class HttpClientRedirectHandler : DelegatingHandler
    {

        public HttpClientRedirectHandler() : base(new HttpClientHandler() { AllowAutoRedirect = false })
        {
            EnforceHostNameMatching = true;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.TemporaryRedirect || response.StatusCode == HttpStatusCode.MovedPermanently)
            {
                var location = response.Headers.Location;
                if (location == null)
                {
                    return response;
                }

                if (EnforceHostNameMatching && request.RequestUri.Host != response.Headers.Location.Host)
                {
                    return response;
                }

                request.RequestUri = location;
                return await base.SendAsync(request, cancellationToken);

            }
            return response;
        }

        public bool EnforceHostNameMatching { get; set; }
    }
}
