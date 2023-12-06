namespace Assets.Core
{
    public class Trap
    {
        public int id;
        public Vec2 position;

        public Trap(Trap t)
        {
            id = t.id;
            position = new Vec2(t.position);
        }

        public Trap(int id, Vec2 position)
        {
            this.id = id;
            this.position = position;
        }
    }
}