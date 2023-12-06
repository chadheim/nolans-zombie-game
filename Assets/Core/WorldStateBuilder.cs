using System;

namespace Assets.Core
{
    public class WorldStateBuilder
    {
        private readonly WorldState state;

        public WorldStateBuilder(int worldWidth, int worldHeight)
        {
            state = new WorldState(worldWidth, worldHeight);
        }

        public WorldState Build()
        {
            return state;
        }

        public WorldStateBuilder PutBunny(int x, int y)
        {
            WorldBoundsCheck(x, y);

            state.bunny.position = new Vec2(x, y);
            return this;
        }

        public WorldStateBuilder AddZombie(int id, int x, int y)
        {
            WorldBoundsCheck(x, y);

            if (state.zombies.Exists(z => z.id == id))
            {
                throw new ArgumentException("id already exists");
            }

            state.zombies.Add(new Zombie(id, new Vec2(x, y)));
            return this;
        }

        public WorldStateBuilder AddTrap(int id, int x, int y)
        {
            WorldBoundsCheck(x, y);

            if (state.traps.Exists(t => t.id == id))
            {
                throw new ArgumentException("id already exists");
            }

            state.traps.Add(new Trap(id, new Vec2(x, y)));
            return this;
        }

        private void WorldBoundsCheck(int x, int y)
        {
            if (x < 0 || x >= state.worldWidth || y < 0 || y >= state.worldHeight)
            {
                throw new ArgumentOutOfRangeException("outside of world bounds");
            }
        }

        public WorldStateBuilder SetZombieTurn(int id)
        {
            state.turn = Turn.ZombieTurn(id);
            return this;
        }

        public WorldStateBuilder AddDeadZombie(int id, int x, int y)
        {
            WorldBoundsCheck(x, y);

            if (state.zombies.Exists(z => z.id == id))
            {
                throw new ArgumentException("id already exists");
            }

            state.zombies.Add(new Zombie(id, new Vec2(x, y)) { alive = false });
            return this;
        }
    }
}