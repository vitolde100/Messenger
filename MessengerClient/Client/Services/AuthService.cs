using MessengerClient.Client.Protocol;
using MessengerShared.Requests.Data;
using MessengerShared.Requests;
using System.Text.Json;
namespace MessengerClient.Client.Services
{
    public class AuthService
    {
        private IProtocol _protocol;
        public AuthService(IProtocol protocol)
        {
            _protocol = protocol;
        }

        public async Task<Responce> SendWithAuth(Request request)
        {
            var response = await _protocol.SendAndReciveAsync(request);

            if (response.Error == ServerCodes.SessionExpired)
            {
                bool refreshed = await TryRefresh();

                if (!refreshed)
                    throw new Exception("Need relogin");

                response = await _protocol.SendAndReciveAsync(request);
            }

            return response;
        }

        private async Task<bool> TryRefresh()
        {
            var refreshRequest = new Request(State.Session.accessToken, "Refresh", new RefreshData { RefreshToken = State.Session.refreshToken });
            Responce response = await _protocol.SendAndReciveAsync(refreshRequest);

            return response.Success;
        }
    }
}
