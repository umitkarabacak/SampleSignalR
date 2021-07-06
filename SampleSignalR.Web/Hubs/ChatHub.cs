using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleSignalR.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private static List<string> _connectionIds = new();


        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            if (!_connectionIds.Contains(connectionId))
                _connectionIds.Add(connectionId);

            _logger.LogInformation($"Connect new device, \nDevice connection Id is {connectionId}");

            await SyncDeviceList();

            await Task.FromResult(
                base.OnConnectedAsync()
            );
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            if (_connectionIds.Contains(connectionId))
                _connectionIds.Remove(connectionId);

            _logger.LogInformation($"Disconnect device, connection Id is {connectionId}");

            if (exception is not null)
                _logger.LogError($"Disconnect device, exception detail \t :{exception.Message} \n {exception.StackTrace}");

            await SyncDeviceList();

            await Task.FromResult(
                base.OnDisconnectedAsync(exception)
            );
        }

        public async Task SyncDeviceList()
        {
            _logger.LogDebug(string.Join(", ", _connectionIds));

            await Clients.All.SendAsync("devices", _connectionIds);
        }
    }
}
