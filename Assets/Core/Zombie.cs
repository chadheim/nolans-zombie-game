namespace Assets.Core
{
    public class Zombie
    {
        public int id;
        public Vec2 position;
        public bool alive;
        public int thinking;

        public Zombie(Zombie z)
        {
            id = z.id;
            position = new Vec2(z.position);
            alive = z.alive;
            thinking = z.thinking;
        }

        public Zombie(int id, Vec2 position)
        {
            this.id = id;
            this.position = position;
            alive = true;
            thinking = 0;
        }
    }
}