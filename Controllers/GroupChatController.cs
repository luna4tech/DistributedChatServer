using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedChatServer.Models;
using DistributedChatServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace DistributedChatServer.Controllers
{
	//[Authorize]
	[ApiController]
    [Route("[controller]")]
    public class GroupChatController : ControllerBase
    {
        private readonly ILogger<GroupChatController> _logger;
        private readonly GroupChatService groupChatService;
        private readonly MyWebSocketService webSocketService;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public GroupChatController(ILogger<GroupChatController> logger, 
            GroupChatService groupChatService, 
            MyWebSocketService webSocketService)
        {
            this.groupChatService = groupChatService;
            this.webSocketService = webSocketService;
            _logger = logger;
        }

        [HttpGet]
        public IList<ChatItem> Get()
        {
            
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return groupChatService.getChatHistory();
        }

        [HttpGet("/ws")] 
        public async Task WebSocketConnection()
		{
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                HttpContext.Response.StatusCode = 400;
            }
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            _logger.Log(LogLevel.Information, "WebSocket connection established");

            webSocketService.AddWebSocketConnection(webSocket);
            await webSocketService.Manage(webSocket);
        }
    }
}
