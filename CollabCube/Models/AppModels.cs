using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CollabCube.Models
{
    public class CubeContext : DbContext
    {
        public CubeContext(): base("DefaultConnection")
        {

        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Solve> Solves { get; set; }
        public DbSet<Move> Moves { get; set; }
    }

    public class Room
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Solve> Solves { get; set; }
    }

    public class Move
    {
        public int ID { get; set; }
        public string MoveString { get; set; }
        public virtual Solve Solve { get; set; }
    }

    public class Solve
    {
        public int ID { get; set; }
        public string Scramble { get; set; }
        public bool IsSolved { get; set; }

        public List<Move> MovesMade { get; set; }
        public virtual Room Room { get; set; }
    }
}