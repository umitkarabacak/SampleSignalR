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

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            if (!_connectionIds.Contains(connectionId))
                _connectionIds.Add(connectionId);

            _logger.LogInformation($"Connect new device, \nDevice connection Id is {connectionId}");

            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            if (_connectionIds.Contains(connectionId))
                _connectionIds.Remove(connectionId);

            _logger.LogInformation($"Disconnect device, connection Id is {connectionId}");
            _logger.LogError($"Disconnect device, exception detail \t :{exception.Message} \n {exception.StackTrace}");

            return base.OnConnectedAsync();
        }

        public async Task SyncDeviceList()
        {
            await Clients.All.SendAsync("devices", _connectionIds);
        }
    }
}
