using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedChatServer.Models
{
	public class ChatItem
	{
		[JsonProperty("sender")] 
		public string Sender { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("timestamp")]
		public double Timestamp { get; set; }
	}
}
