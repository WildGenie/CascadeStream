// Decompiled with JetBrains decompiler
// Type: FaceIdentification.SfinksClient
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FaceIdentification
{
  public class SfinksClient
  {
    private IPAddress _ipAddress = new IPAddress(new byte[4]
    {
      (byte) 192,
      (byte) 168,
      (byte) 1,
      (byte) 10
    });
    private int _port = 7211;
    private SfinksClient.StateObject _state = new SfinksClient.StateObject();
    private bool _isConnected;
    private TcpClient _server;

    public Thread CurrentThread { get; set; }

    public IPAddress IpAddress
    {
      get
      {
        return this._ipAddress;
      }
      set
      {
        this._ipAddress = value;
      }
    }

    public bool IsConnected
    {
      get
      {
        return this._isConnected;
      }
      set
      {
        this._isConnected = value;
      }
    }

    public int Port
    {
      get
      {
        return this._port;
      }
      set
      {
        this._port = value;
      }
    }

    public TcpClient Server
    {
      get
      {
        return this._server;
      }
      set
      {
        this._server = value;
      }
    }

    public SfinksClient.StateObject State
    {
      get
      {
        return this._state;
      }
      set
      {
        this._state = value;
      }
    }

    public event SfinksClient.ConnectionStateEventHandler ConnectionState;

    public event SfinksClient.ReceiveDataEventHandler Receive;

    public SfinksClient(IPAddress adr, int port)
    {
      this.IpAddress = adr;
      this.Port = port;
    }

    private void CheckState()
    {
      while (true)
      {
        if (!IdentificationServer.StopFlag)
        {
          Thread.Sleep(2000);
          try
          {
            if (this.Server == null || !this.Server.Connected)
            {
              try
              {
                this.IsConnected = false;
                this.Server = new TcpClient(this.IpAddress.ToString(), this.Port);
                this.IsConnected = true;
                this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs("Client connected"));
              }
              catch (Exception ex)
              {
                this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs("Client connection error \r\n" + ex.Message + "\r\n" + ex.StackTrace + "\r\n" + ex.Source));
                this.IsConnected = false;
              }
            }
            else
              this.IsConnected = true;
            this.ConnectionState((object) this, new SfinksClient.ConnectionStateEventArgs(this.IsConnected));
          }
          catch
          {
          }
        }
        else
          break;
      }
    }

    public void Connect()
    {
      this.CurrentThread = new Thread(new ThreadStart(this.CheckState))
      {
        IsBackground = true
      };
      this.CurrentThread.Start();
    }

    public void OnReceive(IAsyncResult ar)
    {
      SfinksClient.StateObject stateObject = (SfinksClient.StateObject) ar.AsyncState;
      Socket socket = stateObject.WorkSocket;
      if (socket.Connected)
      {
        try
        {
          int num = socket.EndReceive(ar);
          this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs("Message Received"));
          if (num > 0)
            this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs(Encoding.UTF8.GetString(stateObject.Buffer)));
          else
            this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs("Empty request"));
        }
        catch (SocketException ex)
        {
          this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs("Error:" + ex.Message));
          if (ex.ErrorCode != 10054 && (ex.ErrorCode == 10004 || ex.ErrorCode == 10053))
            return;
          socket.Close();
        }
        catch (Exception ex)
        {
          this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs("Error:" + ex.Message));
        }
      }
      else
        this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs("Server not connected"));
    }

    public void SendCommand(string cmd)
    {
      if (this.Server == null || !this.Server.Connected)
        return;
      this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs(cmd));
      byte[] bytes = Encoding.UTF8.GetBytes(cmd + "\r\n");
      NetworkStream stream = this.Server.GetStream();
      stream.Write(bytes, 0, bytes.Length);
      stream.Flush();
      this.State = new SfinksClient.StateObject();
      this.State.Buffer = new byte[100];
      this.State.WorkSocket = this.Server.Client;
      this.Server.Client.ReceiveTimeout = 500;
      this.Server.Client.BeginReceive(this.State.Buffer, 0, this.State.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) this.State);
      this.IsConnected = true;
      this.Receive((object) this, new SfinksClient.ReceiveDataEventArgs("Message Sent"));
    }

    public class StateObject
    {
      public byte[] Buffer = new byte[52];
      public const int BufferSize = 52;
      public Socket WorkSocket;
    }

    public class ConnectionStateEventArgs : EventArgs
    {
      private bool state;

      public bool ConnectionState
      {
        get
        {
          return this.state;
        }
      }

      public ConnectionStateEventArgs(bool conState)
      {
        this.state = conState;
      }
    }

    public class ReceiveDataEventArgs : EventArgs
    {
      private string _str;

      public string Data
      {
        get
        {
          return this._str;
        }
      }

      public ReceiveDataEventArgs(string data)
      {
        this._str = data;
      }
    }

    public delegate void ConnectionStateEventHandler(object sender, SfinksClient.ConnectionStateEventArgs eventArgs);

    public delegate void ReceiveDataEventHandler(object sender, SfinksClient.ReceiveDataEventArgs eventArgs);
  }
}
