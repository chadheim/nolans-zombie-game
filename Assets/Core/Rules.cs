using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Core
{
    public class Rules
    {
        public enum Direction { Left, Right, Up, Down }

        private static Vec2 Translate(Direction direction, Vec2 position)
        {
            Vec2 p = new(position);
            switch (direction)
            {
                case Direction.Left: p.x--; break;
                case Direction.Right: p.x++; break;
                case Direction.Up: p.y++; break;
                case Direction.Down: p.y--; break;
            }
            return p;
        }

        private static bool WorldBoundsCheck(WorldState state, Vec2 position)
        {
            if (position.x < 0 || position.x >= state.worldWidth)
            {
                return false;
            }

            if (position.y < 0 || position.y >= state.worldHeight)
            {
                return false;
            }

            return true;
        }

        private static Turn ChooseNextTurn(WorldState current)
        {
            Zombie nextZombie = (current.turn.Type == Turn.Types.Bunny) ?
                current.zombies.FirstOrDefault() :
                current.zombies.SkipWhile(z => z.id != current.turn.Id).Skip(1).SkipWhile(z => !z.alive).FirstOrDefault();

            return (nextZombie != null) ? Turn.ZombieTurn(nextZombie.id) : Turn.BunnyTurn();
        }


        private static bool BunnyTurnCheck(WorldState current)
        {
            return current.turn.Type == Turn.Types.Bunny;
        }

        private static bool BunnyOnTrapCheck(Bunny b, List<Trap> traps)
        {
            return traps.Exists(t => t.position == b.position);
        }

        public static WorldState MoveBunny(Direction direction, WorldState current)
        {
            if (!BunnyTurnCheck(current))
            {
                return current;
            }

            WorldState next = current.Copy();

            next.turn = ChooseNextTurn(current);

            next.bunny.position = Translate(direction, next.bunny.position);

            if(BunnyOnTrapCheck(next.bunny, next.traps))
            {
                return current;
            }

            if (!WorldBoundsCheck(next, next.bunny.position))
            {
                return current;
            }

            return next;
        }

        private static bool ZombieTurnCheck(int id, WorldState current)
        {
            return current.turn.Type == Turn.Types.Zombie && current.turn.Id == id;
        }

        private static bool ZombieOnBunnyCheck(Zombie z, Bunny b)
        {
            return z.position == b.position;
        }

        private static bool ZombieOnTrapCheck(Zombie z, List<Trap> traps)
        {
            return traps.Exists(t => t.position == z.position);
        }

        private static bool ZombieOnOtherZombiesCheck(Zombie z, List<Zombie> zombies)
        {
            return z.alive && zombies.Exists(o => o.position == z.position && o.id != z.id && o.alive == true);
        }

        private static bool InternalMoveZombie(int id, Direction direction, WorldState current)
        {
            current.turn = ChooseNextTurn(current);

            Zombie zombie = current.zombies.Find(z => z.id == id);

            if (!zombie.alive)
                return true;

            zombie.position = Translate(direction, zombie.position);

            if (!WorldBoundsCheck(current, zombie.position))
            {
                return false;
            }

            if (ZombieOnOtherZombiesCheck(zombie, current.zombies))
            {
                return false;
            }

            if (ZombieOnBunnyCheck(zombie, current.bunny))
            {
                current.bunny.alive = false;
            }

            if (ZombieOnTrapCheck(zombie, current.traps))
            {
                zombie.alive = false;
            }

            return true;
        }

        public static WorldState MoveZombie(int id, Direction direction, WorldState current)
        {
            if (!ZombieTurnCheck(id, current))
            {
                return current;
            }

            WorldState next = current.Copy();

            return InternalMoveZombie(id, direction, next) ? next : current;
        }

        public static WorldState MoveZombie(int id, WorldState current)
        {
            if (!ZombieTurnCheck(id, current))
            {
                return current;
            }

            WorldState next = current.Copy();

            Zombie zombie = next.zombies.Find(z => z.id == id);

            zombie.thinking++;
            if (zombie.thinking < 3)
            {
                return InternalPassZombie(id, next) ? next : current;
            }

            zombie.thinking = 0;
            Vec2 toBunny = next.bunny.position - zombie.position;
            Direction direction = Direction.Left;
            if (Math.Abs(toBunny.x) >= Math.Abs(toBunny.y))
            {
                direction = toBunny.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                direction = toBunny.y > 0 ? Direction.Up : Direction.Down;
            }

            return InternalMoveZombie(id, direction, next) ? next : current;

        }

        private static bool InternalPassZombie(int id, WorldState current)
        {
            current.turn = ChooseNextTurn(current);

            return true;
        }

        public static WorldState PassZombie(int id, WorldState current)
        {
            if (!ZombieTurnCheck(id, current))
            {
                return current;
            }

            WorldState next = current.Copy();

            return InternalPassZombie(id, next) ? next : current;
        }

        public enum GameState { Win, Lose, InProgress }
        public static GameState GameStateCheck(WorldState current)
        {
            if (!current.bunny.alive)
            {
                return GameState.Lose;
            }

            if (current.zombies.All(z => !z.alive))
            {
                return GameState.Win;
            }

            return GameState.InProgress;
        }

    }
}