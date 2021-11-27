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
        private NetworkStream _stream;
        private TcpClient _client;
        private TcpListener _listener;
        
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

        public void SetIpAddress(string ip)
        {
            _ipAddress = ip;
        }
        
        protected override void CleanUp()
        {
            foreach (var player in Players)
            {
                player.CleanUp();
            }
            
            _client.Close();
            if (_isServer)
            {
                _listener.Stop();
            }
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
            Console.WriteLine("Sending data");
            Console.WriteLine("Data: " + data);
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            _stream.Write(bytes, 0, bytes.Length);
        }
        
        private string ReceiveData()
        {
            Console.WriteLine("Receiving Data");
            byte[] bytes = new byte[_client.ReceiveBufferSize];
            int bytesRead = _stream.Read(bytes, 0, _client.ReceiveBufferSize);

            string data = Encoding.UTF8.GetString(bytes);
            Console.WriteLine("Data: " + data);
            return data;
        }
        
        private void ConnectToServer()
        {
            _client = new TcpClient(_ipAddress, _port);
            _stream = _client.GetStream();

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
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            
            _client = _listener.AcceptTcpClient();
            Console.WriteLine("A Client connected");
            
            _stream = _client.GetStream();
            
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