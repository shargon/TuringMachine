namespace System.Net
{
    public static class IpEndPointExtension
    {
        public static IPEndPoint ToIpEndPoint(this string value)
        {
            string[] sp = value.Split(new char[] { ',', ';', '=' }, StringSplitOptions.RemoveEmptyEntries);
            return new IPEndPoint(IPAddress.Parse(sp[0]), Convert.ToInt32(sp[1]));
        }
    }
}