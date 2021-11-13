using DistributedChatServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedChatServer.Services
{
	public class GroupChatService
	{
		private static List<ChatItem> chats = new List<ChatItem>();

		public List<ChatItem> getChatHistory()
		{
			return chats;
		}

		public void addMessage(ChatItem chatItem)
		{
			chats.Add(chatItem);
		}
	}
}
