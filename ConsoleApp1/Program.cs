namespace ConsoleAppClient
{
    using System;
    using System.Collections;
    using System.Net.Security;
    using System.Net.Sockets;
    using System.Security.Authentication;
    using System.Text;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Http.Json;
    using System.Text.Json;
    using ConsoleApp1;
    using System.Net.Http.Headers;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;
    using JsonSerializer = System.Text.Json.JsonSerializer;
    using System.IO.Compression;
    using System.Net;

    namespace Examples.System.Net
    {
        public class SslTcpClient
        {
            private static Hashtable certificateErrors = new Hashtable();
            // The following method is invoked by the RemoteCertificateValidationDelegate.
            public static bool ValidateServerCertificate(
                  object sender,
                  X509Certificate certificate,
                  X509Chain chain,
                  SslPolicyErrors sslPolicyErrors)
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;
                Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
                // Do not allow this client to communicate with unauthenticated servers.
                return false;
            }

            public static async void RunClient(string machineName)
            {
                string clientmachineName = "https://www.isn88.com/membersite-api/api/member/authenticate";
                HttpClientHandler httpMessage = new();
                httpMessage.AutomaticDecompression = DecompressionMethods.GZip;
                HttpClient client = new HttpClient(httpMessage);
                var value = new Dictionary<string, string>
                {
                    { "username", "eds_test_1" },
                    { "password", "1234qwer" }
                };
                var jsonString = JsonSerializer.Serialize(value);
                var content = new StringContent(jsonString,Encoding.UTF8,"application/json");
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, clientmachineName);
                httpRequest.Headers.Add("Accept", "application/json");
                httpRequest.Headers.Add("locale", "en-US");
                httpRequest.Content = content;
                var resultclient = await client.SendAsync(httpRequest);
                Console.WriteLine(resultclient.StatusCode);
                var data1 = await resultclient.Content.ReadAsStringAsync();
                Console.WriteLine(data1);
                Console.ReadKey();
                var ddd = JsonSerializer.Deserialize<jwttoken>(data1);




                string client2machineName = "https://www.isn88.com/membersite-api/api/data/sports";
                HttpClient client2 = new HttpClient();
                HttpRequestMessage client2httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, client2machineName);
                client2httpRequestMessage.Headers.Add("Accept", "application/json");
                //client2httpRequestMessage.Headers.Add("Authorization", $"Bearer {}");
                client2httpRequestMessage.Headers.Add("locale", "zh-TW");
                var result = await client2.SendAsync(client2httpRequestMessage);
                if (result != null)
                {
                    var data = await result.Content.ReadAsStringAsync();
                    Console.WriteLine(data);
                }
            }

            static string ReadMessage(SslStream sslStream)
            {
                // Read the  message sent by the server.
                // The end of the message is signaled using the
                // "<EOF>" marker.
                byte[] buffer = new byte[2048];
                StringBuilder messageData = new StringBuilder();
                int bytes = -1;
                do
                {
                    bytes = sslStream.Read(buffer, 0, buffer.Length);

                    // Use Decoder class to convert from bytes to UTF8
                    // in case a character spans two buffers.
                    Decoder decoder = Encoding.UTF8.GetDecoder();
                    char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                    decoder.GetChars(buffer, 0, bytes, chars, 0);
                    messageData.Append(chars);
                    // Check for EOF.
                    if (messageData.ToString().IndexOf("<EOF>") != -1)
                    {
                        break;
                    }
                } while (bytes != 0);

                return messageData.ToString();
            }

            private static void DisplayUsage()
            {
                Console.WriteLine("To start the client specify:");
                Console.WriteLine("clientSync machineName [serverName]");
                Environment.Exit(1);
            }

            public static void Main(string[] args)
            {
                
                string machineName = null;
                machineName = "127.0.0.1";
                try
                {
                    RunClient(machineName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }
    }
}