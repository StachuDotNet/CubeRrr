using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CollabCube.Models;

namespace CollabCube.Hubs
{
    public class CubeHub : Hub
    {
        Models.CubeContext dbContext;

        public CubeHub()
        {
            this.dbContext = new Models.CubeContext();
        }

        public void Send(string name, string message, string roomName)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message, roomName);
        }

        public void Update(string roomName)
        {
            // make it so same person can't update super often.
            var room = dbContext.Rooms.FirstOrDefault(r => r.Name == roomName);

            if (room.Solves == null)
            {
                var newSolve = new Solve { MovesMade = new List<Move>(), Scramble = "R U R' U'", IsSolved = false };
                room.Solves = new List<Solve> { newSolve };
                dbContext.SaveChanges();
            }

            var currentSolve = room.Solves.FirstOrDefault(s => !s.IsSolved);

            var scramble = currentSolve.Scramble;
            var movesMade = String.Join(" ", currentSolve.MovesMade.Select(m => m.MoveString).ToList());
            Clients.Caller.refreshCube(scramble + " " + movesMade);

            Clients.Others.forcePause(roomName);
        }

        public void DoMove(string move, string roomName)
        {
            var room = dbContext.Rooms.Include("Solves").Include("Solves.MovesMade").FirstOrDefault(r => r.Name == roomName);

            // eventually deal with this somewhere else.
            if (room.Solves == null)
            {
                var newCurrentSolve = new Solve { IsSolved = false, MovesMade = new List<Move>(), Scramble = "R U R' U'" };
                room.Solves = new List<Solve>() { newCurrentSolve };
                dbContext.SaveChanges();
            }

            var currentSolve = room.Solves.FirstOrDefault(s => !s.IsSolved);

            var newMove = new CollabCube.Models.Move { MoveString = move };

            currentSolve.MovesMade = currentSolve.MovesMade ?? new List<Move>();

            currentSolve.MovesMade.Add(newMove);

            dbContext.SaveChanges();

            // At this point, check if solved and send message to all if it is.

            Clients.All.doMoveToCube(move, roomName);
        }
    }
}