﻿using System;
using System.Net;
using System.Net.Sockets;

namespace BackToTheFutureV.HUD.Core
{
    public static class HUDNetwork
    {
        private static UdpClient udp = new UdpClient(1985);

        private static HUDDisplay _display;

        private static void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 1985);

            byte[] ret = udp.EndReceive(ar, ref ip);

            HUDProperties properties = HUDProperties.FromData(ret);

            if (properties != null)
                _display.Properties = properties;

            Start(_display);
        }

        public static void Start(HUDDisplay display)
        {
            _display = display;

            if (udp == null)
                udp = new UdpClient(1985);

            udp.BeginReceive(Receive, new object());
        }

        public static void Stop()
        {
            udp.Dispose();
            udp = null;
        }
    }
}
