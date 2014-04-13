//using Microsoft.AspNet.SignalR.Hubs;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNet.SignalR;

//namespace CollabCube.Hubs
//{
//    public class Room
//    {
//        public string Name { get; set; }
//        public List<Solve> Solves { get; set; }
//    }

//    public class Solve
//    {
//        public string Scramble { get; set; }
//        public List<Move> Moves { get; set; }
//        public bool isSolved { get; set;}
//    }
//    public class Move
//    {
//        public string MoveString { get; set; }
//    }

//    public class Stuff
//    {
//        // Singleton instance       
//        private readonly static Lazy<Stuff> _instance = new Lazy<Stuff>(() => new Stuff(GlobalHost.ConnectionManager.GetHubContext<CubeHub>().Clients));

//        private readonly ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();

//        private readonly object _updateStockPricesLock = new object();

//        //stock can go up or down by a percentage of this factor on each change
//        private readonly double _rangePercent = .002;

//        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
//        private readonly Random _updateOrNotRandom = new Random();

//        private readonly Timer _timer;
//        private volatile bool _updatingStockPrices = false;

//        private Stuff(IHubConnectionContext clients)
//        {
//            Clients = clients;

//            _rooms.Clear();

//            var rooms = new List<Room>
//            {
//                new Room { Name = "Testing" },
//                new Room { Name = "CFOP" },
//                new Room { Name = "Petrus" }
//            };
//            rooms.ForEach(room => _rooms.TryAdd(room.Name, room));
//        }

//        public static Stuff Instance
//        {
//            get
//            {
//                return _instance.Value;
//            }
//        }

//        private IHubConnectionContext Clients { get; set; }

//        public IEnumerable<Room> GetAllRooms()
//        {
//            return _rooms.Values;
//        }

//        private void UpdateStockPrices(object state)
//        {
//            lock (_updateStockPricesLock)
//            {
//                if (!_updatingStockPrices)
//                {
//                    _updatingStockPrices = true;

//                    foreach (var stock in _rooms.Values)
//                    {
//                        if (TryUpdateStockPrice(stock))
//                        {
//                            BroadcastStockPrice(stock);
//                        }
//                    }

//                    _updatingStockPrices = false;
//                }
//            }
//        }

//        private bool TryUpdateStockPrice(Room room)
//        {
//            // Randomly choose whether to update this stock or not
//            var r = _updateOrNotRandom.NextDouble();
//            if (r > .1)
//            {
//                return false;
//            }

//            // Update the stock price by a random factor of the range percent
//            //var random = new Random((int)Math.Floor(room.Price));
//            //var percentChange = random.NextDouble() * _rangePercent;
//            //var pos = random.NextDouble() > .51;
//            //var change = Math.Round(room.Price * (decimal)percentChange, 2);
//            //change = pos ? change : -change;

//            //stock.Price += change;
//            return true;
//        }

//        private void BroadcastStockPrice(Room room)
//        {
//            Clients.All.updateStockPrice(room);
//        }
//    }
//}