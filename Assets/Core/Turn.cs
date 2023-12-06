namespace Assets.Core
{
    public class Turn
    {
        public enum Types { Bunny, Zombie };
        public Types Type { get; private set; }
        public int Id { get; private set; }

        private Turn()
        {
            Type = Types.Bunny;
            Id = 0;
        }

        public Turn(Turn turn)
        {
            Type = turn.Type;
            Id = turn.Id;
        }

        public static Turn ZombieTurn(int id)
        {
            return new() { Type = Types.Zombie, Id = id };
        }

        public static Turn BunnyTurn()
        {
            return new() { Type = Types.Bunny };
        }
    }
}