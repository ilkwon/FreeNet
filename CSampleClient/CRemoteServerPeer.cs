﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;

namespace CSampleClient
{
	using GameServer;

	class CRemoteServerPeer : IPeer
	{
		public CUserToken token { get; private set; }

		public CRemoteServerPeer(CUserToken token)
		{
			this.token = token;
			this.token.set_peer(this);
		}

		void IPeer.on_message(ArraySegment<byte> buffer)
		{
			CPacket msg = new CPacket(buffer.Array, this);
			PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id();
			switch (protocol_id)
			{
				case PROTOCOL.CHAT_MSG_ACK:
					{
						string text = msg.pop_string();
						Console.WriteLine(string.Format("text {0}", text));
					}
					break;
			}
		}

		void IPeer.on_removed()
		{
			Console.WriteLine("Server removed.");
		}

		void IPeer.send(CPacket msg)
		{
            msg.record_size();
			this.token.send(new ArraySegment<byte>(msg.buffer, 0, msg.position));
		}

		void IPeer.disconnect()
		{
			this.token.socket.Disconnect(false);
		}

		void IPeer.process_user_operation(CPacket msg)
		{
		}
	}
}
