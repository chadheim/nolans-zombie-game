using System.Collections.Generic;
using System.Linq;

namespace Assets.Core
{
    public class WorldState
    {
        public Bunny bunny;
        public List<Trap> traps;
        public List<Zombie> zombies;
        public int worldWidth;
        public int worldHeight;
        public Turn turn;

        public WorldState(int worldWidth, int worldHeight)
        {
            bunny = new Bunny();
            zombies = new();
            traps = new();
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            turn = Turn.BunnyTurn();
        }

        private WorldState(WorldState state)
        {
            bunny = new Bunny(state.bunny);
            traps = state.traps.Select(t => new Trap(t)).ToList();
            zombies = state.zombies.Select(z => new Zombie(z)).ToList();
            worldWidth = state.worldWidth;
            worldHeight = state.worldHeight;
            turn = new Turn(state.turn);
        }

        public WorldState Copy()
        {
            return new(this);
        }
    }
}