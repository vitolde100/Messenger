using MessengerClient.Client.Protocol;
using MessengerShared.Requests.Data;
using MessengerShared.Requests;
using MessengerShared.Requests.Enums;

namespace MessengerClient.Client.Services
{
    public class AuthService
    {
        public event Action? OnReloginRequired;
        private IProtocol _protocol = Program.AppContext.Protocol;
        public bool _authenticated;
        bool _isNeedRelogin;

        public AuthService() { }

        public async Task<Response> SendWithAuth(Request request)
        {
            _isNeedRelogin = false;
            await TryAuth();
            if (_isNeedRelogin)
            {
                return new Response
                {
                    Success = false,
                    Error = ServerCodes.Unauthorized
                };
            }

            var response = await _protocol.SendAndReciveAsync(request);

            if (response.Error == ServerCodes.SessionExpired) NeedRelogin();
            if (_isNeedRelogin)
            {
                return new Response
                {
                    Success = false,
                    Error = ServerCodes.Unauthorized
                };
            }

            if (response.Error == ServerCodes.AccessTokenExpired)
            {
                bool refreshed = await TryRefresh();

                if (!refreshed) NeedRelogin();  

                response = await _protocol.SendAndReciveAsync(request);
            }

            return response;
        }

        /// <summary>
        /// Wrapper over Hello for checks.
        /// </summary>
        /// <returns></returns>
        public async Task TryAuth()
        {
            if (!_authenticated && Program.state.isLoggedIn)
            {
                bool sayHello = await TrySayHello();
                if (!sayHello)
                {
                    NeedRelogin();
                }
                else _authenticated = true;
            }
            if (Program.state.isLoggedIn == false) NeedRelogin(); 
        }

        private void NeedRelogin()
        {
            _authenticated = false;
            Program.state.Clear();
            OnReloginRequired?.Invoke();
            _isNeedRelogin = true;
        }

        public async Task<bool> TrySayHello()
        {
            var helloRequest = new Request(Program.state.Session.accessToken, "Hello", null);
            Response response = await _protocol.SendAndReciveAsync(helloRequest);

            return response.Success;
        }

        private async Task<bool> TryRefresh()
        {
            var refreshRequest = new Request(Program.state.Session.accessToken, "Refresh", new StringData { StringStr = Program.state.Session.refreshToken });
            Response response = await _protocol.SendAndReciveAsync(refreshRequest);

            return response.Success;
        }
    }
}
