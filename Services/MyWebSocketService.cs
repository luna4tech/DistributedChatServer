using DistributedChatServer.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedChatServer.Services
{
	public class MyWebSocketService
	{
        private static int id = 0;
		private Dictionary<string, WebSocket> webSocketMap;
        private readonly ILogger<MyWebSocketService> _logger;
        private readonly GroupChatService groupChatService;

        public MyWebSocketService(ILogger<MyWebSocketService> logger, GroupChatService groupChatService)
		{
            this._logger = logger;
			this.webSocketMap = new Dictionary<string, WebSocket>();
            this.groupChatService = groupChatService;
		}

		public void AddWebSocketConnection(WebSocket webSocket)
		{
            id++;
			webSocketMap.Add(id.ToString(), webSocket);
		}

        public void BroadcastMessage(string message)
		{
            var messageBytes = Encoding.UTF8.GetBytes(message);
            IEnumerable<Task> tasks = webSocketMap.Values.ToList<WebSocket>()
                .Where(webSocket => webSocket.State.Equals(WebSocketState.Open))
                .Select(webSocket => webSocket.SendAsync(
                    new ArraySegment<byte>(messageBytes, 0, messageBytes.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            
            Task.WaitAll(tasks.ToArray());
		}

        public async Task Manage(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            _logger.Log(LogLevel.Information, "Message received from Client");

            while (!result.CloseStatus.HasValue)
            {
                var chatMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var chatItem = JsonConvert.DeserializeObject<ChatItem>(chatMessage);
                groupChatService.addMessage(chatItem);

                BroadcastMessage(chatMessage);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                _logger.Log(LogLevel.Information, "Message received from Client");

            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _logger.Log(LogLevel.Information, "WebSocket connection closed");
        }
    }
}
