using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// Pixel Worlds Private Server (c) ALIX
// OpenSource on Github.com/Rijndael3
// Subscribe Youtube Channel justAlix
public class PWPrivateServer
{
     
    public Socket workSocket = null;
      
    public const int BufferSize = 1024;
      
    public byte[] buffer = new byte[BufferSize];
     
    public StringBuilder sb = new StringBuilder();
}

public class AsynchronousSocketListener
{
    
    public static ManualResetEvent allDone = new ManualResetEvent(false);

    public AsynchronousSocketListener()
    {
    }

    public static void StartListening()
    {
        string hostName = Dns.GetHostName(); 
    
        IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
        IPAddress[] addr = hostEntry.AddressList;
        var ip = addr.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                     .FirstOrDefault();
      
     


      
        IPEndPoint localEndPoint = new IPEndPoint(ip, 10001);


        Socket listener = new Socket(ip.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

   
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10001);

            while (true)
            {

                allDone.Reset();


                Console.WriteLine("Someone connecting to the private server...");
                Console.WriteLine("Getting Player data...");


               




                Thread.Sleep(1250);
                Console.WriteLine("Creating user database...");
                Thread.Sleep(1250);
                Console.WriteLine("waiting player to connect...");
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);


                allDone.WaitOne();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public static void AcceptCallback(IAsyncResult ar)
    {

        allDone.Set();
  
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);
        
        PWPrivateServer state = new PWPrivateServer();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, PWPrivateServer.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }
    public void PlayerConnecting()
    {

        Socket handler;

      
    }
    public static void ReadCallback( IAsyncResult ar)
    {


        PWPrivateServer state = (PWPrivateServer)ar.AsyncState;
        Socket handler = state.workSocket;

        
        string[] packet1 = System.IO.File.ReadAllLines("packet1.txt");
        foreach (string pl1 in packet1)
        {
            Console.WriteLine(pl1);
            Send(handler, pl1);
          

        }

        for (; ; )
        {
            Console.WriteLine("D...@....m0./....ID.....SyncTime..STime.....,-...SSlp.......mc......");
            Send(handler, "D...@....m0./....ID.....SyncTime..STime.....,-...SSlp.......mc......");
        }
       
       





        int bytesRead = handler.EndReceive(ar);

       
    }

    private static void Send(Socket handler, String data)
    {
        try
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);


            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
        catch
        {

        }
        
      
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
           
            Socket handler = (Socket)ar.AsyncState;

           
            int bytesSent = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);

           

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Pixel World Private Server (c) Liab");
        Console.ForegroundColor = ConsoleColor.White;

        StartListening();
        return 0;
    }
}
