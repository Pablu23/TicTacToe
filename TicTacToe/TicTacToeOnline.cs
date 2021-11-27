using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using TicTacToe.Players;

namespace TicTacToe
{
    public class TicTacToeOnline : TicTacToe
    {
        private bool _isServer = false;
        private int _port = 6969;
        private string _ipAddress = "127.0.0.1";
        private Socket _client;

        private int _clientPlayerId = -1;
        
        public override void AddPlayer(Player player)
        {
            if (player.GetType() != typeof(HumanPlayer))
                throw new Exception("Online Play is only available with human Players");
            base.AddPlayer(player);
        }

        public void IsServer(bool x)
        {
            _isServer = x;
        }
        
        public override void StartGame()
        {
            if (_isServer)
            {
                StartServer();

                if (Players.Count != 2) throw new Exception("Not enough, or too many Players");
                Board = new TicTacToeBoard();
                string ticTacToeBoardJson = JsonConvert.SerializeObject(Board);
                SendData(ticTacToeBoardJson);
                
            }
            else
            {
                ConnectToServer();
                
                if (Players.Count != 2) throw new Exception("Not enough, or too many Players");
                string ticTacToeBoardJson = ReceiveData();
                Board = JsonConvert.DeserializeObject<TicTacToeBoard>(ticTacToeBoardJson);
            }
            
            Board.DrawBoard();
            GameFinished = false;
            int round = 0;

            while (GameFinished != true)
            {
                // For both Players
                for (int i = 0; i < 2; i++)
                {
                    if (i == _clientPlayerId)
                    {
                        bool placed;
                        do
                        {
                            placed = Board.TryPlace(Players[i].MakeMove(Board), Players[i], round);
                        } while (!placed);
                        
                        string newBoardJson = JsonConvert.SerializeObject(Board);
                        SendData(newBoardJson);
                    }
                    else
                    {
                        string data = ReceiveData();
                        Board = JsonConvert.DeserializeObject<TicTacToeBoard>(data);
                    }
                    
                    round++;
                    Board.DrawBoard();
                    CheckGameState();
                    //Dont let the second Player make his turn if the game is already finished
                    if (GameFinished) break;
                }
            }
        }

        private void SendData(string data)
        {
            data += "<EOF>";
            Console.WriteLine("Sending Data: " + data);
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            _client.Send(bytes);
        }
        
        private string ReceiveData()
        {
            string data = null;
            byte[] bytes = null;

            Console.WriteLine("Receiving Data");
            
            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = _client.Receive(bytes);
                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                Console.WriteLine("Data: " + data);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }
            
            Console.WriteLine("Data Received, returning");
            
            return data;
        }
        
        private void ConnectToServer()
        {
            IPHostEntry host = Dns.GetHostEntry(_ipAddress);
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, _port);

            _client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _client.Connect(remoteEndPoint);

            Console.WriteLine("connected");
            
            string data = ReceiveData();

            HumanPlayer opponent = JsonConvert.DeserializeObject<HumanPlayer>(data);
            AddPlayer(opponent);
            HumanPlayer player = new HumanPlayer("Client",
                opponent.Symbol == FieldState.PlayerX ? FieldState.PlayerO : FieldState.PlayerX);
            AddPlayer(player);

            string playerJson = JsonConvert.SerializeObject(player);
            SendData(playerJson);
            _clientPlayerId = 0;
        }

        private void StartServer()
        {
            IPHostEntry host = Dns.GetHostEntry(_ipAddress);
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _port);
            Socket server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(localEndPoint);
            server.Listen(1);

            _client = server.Accept();

            Console.WriteLine("A Client connected");
            
            var player = new HumanPlayer("Server", FieldState.PlayerX);
            AddPlayer(player);
            
            string playerJson = JsonConvert.SerializeObject(player);
            SendData(playerJson);

            string data = ReceiveData();
            var opponent = JsonConvert.DeserializeObject<HumanPlayer>(data);
            
            AddPlayer(opponent);
            _clientPlayerId = 1;
        }
    }
}