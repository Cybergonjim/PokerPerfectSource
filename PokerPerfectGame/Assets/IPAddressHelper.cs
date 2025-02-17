using System.Net;
using System.Net.Sockets;

public class IPAddressHelper
{
    public static string GetLocalIPAddress()
    {
        foreach (var netInterface in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (netInterface.AddressFamily == AddressFamily.InterNetwork) // IPv4
            {
                return netInterface.ToString();
            }
        }
        return "127.0.0.1"; // Fallback if no network found
    }
}
