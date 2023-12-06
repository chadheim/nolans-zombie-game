namespace Assets.Core
{
    public class Bunny
    {
        public Vec2 position;
        public bool alive;

        public Bunny()
        {
            position = new Vec2();
            alive = true;
        }

        public Bunny(Bunny b)
        {
            position = new Vec2(b.position);
            alive = b.alive;
        }
    }
}