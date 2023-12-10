using Assets.Core;
using NUnit.Framework;
using System.Linq;

public class RulesTests
{
    [Test]
    public void ShouldMoveBunnyLeft()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Left, s0);

        Assert.AreEqual(s0.bunny.position.x - 1, s1.bunny.position.x);
        Assert.AreEqual(s0.bunny.position.y, s1.bunny.position.y);
    }

    [Test]
    public void ShouldMoveBunnyRight()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Right, s0);

        Assert.AreEqual(s0.bunny.position.x + 1, s1.bunny.position.x);
        Assert.AreEqual(s0.bunny.position.y, s1.bunny.position.y);
    }

    [Test]
    public void ShouldMoveBunnyUp()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Up, s0);

        Assert.AreEqual(s0.bunny.position.x, s1.bunny.position.x);
        Assert.AreEqual(s0.bunny.position.y + 1, s1.bunny.position.y);
    }

    [Test]
    public void ShouldMoveBunnyDown()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Down, s0);

        Assert.AreEqual(s0.bunny.position.x, s1.bunny.position.x);
        Assert.AreEqual(s0.bunny.position.y - 1, s1.bunny.position.y);
    }

    [Test]
    public void ShouldNotMoveBunnyLeftWhenAtLeftEdgeOfWorld()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(0, 10).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Left, s0);

        Assert.AreEqual(s0.bunny.position, s1.bunny.position);
    }

    [Test]
    public void ShouldNotMoveBunnyRightWhenAtRightEdgeOfWorld()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(19, 10).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Right, s0);

        Assert.AreEqual(s0.bunny.position, s1.bunny.position);
    }

    [Test]
    public void ShouldNotMoveBunnyDownWhenAtBottomEdgeOfWorld()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 0).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Down, s0);

        Assert.AreEqual(s0.bunny.position, s1.bunny.position);
    }

    [Test]
    public void ShouldNotMoveBunnyUpWhenAtTopEdgeOfWorld()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 19).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Up, s0);

        Assert.AreEqual(s0.bunny.position, s1.bunny.position);
    }

    [Test]
    public void ShouldNotMoveBunnyUpWhenNotBunnyTurn()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).AddZombie(0, 0, 0).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Down, s0);

        Assert.AreEqual(s0.bunny.position, s1.bunny.position);
    }

    [Test]
    public void ShouldNotMoveBunnyOnToTrap()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(0, 10).AddTrap(0, 0, 11).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Up, s0);

        Assert.AreEqual(s0.bunny.position, s1.bunny.position);
    }

    [Test]
    public void ShouldMoveZombieLeft()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Left, s0);

        Assert.AreEqual(9, s1.zombies.First().position.x);
        Assert.AreEqual(10, s1.zombies.First().position.y);
    }

    [Test]
    public void ShouldMoveZombieRight()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Right, s0);

        Assert.AreEqual(11, s1.zombies.First().position.x);
        Assert.AreEqual(10, s1.zombies.First().position.y);
    }

    [Test]
    public void ShouldMoveZombieUp()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Up, s0);

        Assert.AreEqual(10, s1.zombies.First().position.x);
        Assert.AreEqual(11, s1.zombies.First().position.y);
    }

    [Test]
    public void ShouldMoveZombieDown()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Down, s0);

        Assert.AreEqual(10, s1.zombies.First().position.x);
        Assert.AreEqual(9, s1.zombies.First().position.y);
    }

    [Test]
    public void ShouldNotMoveZombieLeftWhenAtLeftEdgeOfWorld()
    {
        WorldState s0 = new WorldStateBuilder(1, 1).AddZombie(0, 0, 0).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Left, s0);

        Assert.AreEqual(0, s1.zombies.First().position.x);
        Assert.AreEqual(0, s1.zombies.First().position.y);
    }

    [Test]
    public void ShouldNotMoveZombieRightWhenAtRightEdgeOfWorld()
    {
        WorldState s0 = new WorldStateBuilder(1, 1).AddZombie(0, 0, 0).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Right, s0);

        Assert.AreEqual(0, s1.zombies.First().position.x);
        Assert.AreEqual(0, s1.zombies.First().position.y);
    }

    [Test]
    public void ShouldNotMoveZombieDownWhenAtBottomEdgeOfWorld()
    {
        WorldState s0 = new WorldStateBuilder(1, 1).AddZombie(0, 0, 0).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Down, s0);

        Assert.AreEqual(0, s1.zombies.First().position.x);
        Assert.AreEqual(0, s1.zombies.First().position.y);
    }

    [Test]
    public void ShouldNotMoveZombieUpWhenAtTopEdgeOfWorld()
    {
        WorldState s0 = new WorldStateBuilder(1, 1).AddZombie(0, 0, 0).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Up, s0);

        Assert.AreEqual(0, s1.zombies.First().position.x);
        Assert.AreEqual(0, s1.zombies.First().position.y);
    }

    [Test]
    public void ShouldMarkBunnyDeadWhenBunnyOnZombieAfterZombieMove()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).AddZombie(0, 9, 10).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Right, s0);

        Assert.AreEqual(s1.bunny.position, s1.zombies.First().position);
        Assert.IsFalse(s1.bunny.alive);
    }

    [Test]
    public void ShouldMarkBunnyDeadWhenBunnyOnZombieAfterBunnyMove()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).AddZombie(0, 9, 10).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Left, s0);

        Assert.AreEqual(s1.bunny.position, s1.zombies.First().position);
        Assert.IsFalse(s1.bunny.alive);
    }

    [Test]
    public void ShouldMarkZombieDeadWhenZombieOnTrapAfterMove()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).AddZombie(0, 1, 1).AddTrap(0, 1, 2).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Up, s0);

        Assert.AreEqual(s1.traps.First().position, s1.zombies.First().position);
        Assert.IsFalse(s1.zombies.First().alive);
    }

    [Test]
    public void ShouldNotAllowTwoZombiesOnOneSquareIfBothAlive()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).AddZombie(0, 0, 1).AddZombie(1, 0, 2).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Up, s0);

        var z0s0 = s0.zombies.First(z => z.id == 0);
        var z0s1 = s1.zombies.First(z => z.id == 0);
        var z1s0 = s0.zombies.First(z => z.id == 1);
        var z1s1 = s1.zombies.First(z => z.id == 1);
        Assert.AreEqual(z0s0.position, z0s1.position);
        Assert.AreEqual(z1s0.position, z1s1.position);
    }

    [Test]
    public void ShouldAllowTwoZombiesOnOneSquareIfAtLeastOneIsNotAlive()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).AddZombie(0, 0, 1).AddDeadZombie(1, 0, 2).SetZombieTurn(0).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Up, s0);

        var z0s1 = s1.zombies.First(z => z.id == 0);
        var z1s1 = s1.zombies.First(z => z.id == 1);
        Assert.AreEqual(z0s1.position, z1s1.position);
        Assert.IsTrue(z0s1.alive);
        Assert.IsFalse(z1s1.alive);
    }

    [Test]
    public void ShouldNotAllowZombieMoveWhenBunnyTurn()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).AddZombie(0, 0, 1).Build();

        WorldState s1 = Rules.MoveZombie(0, Rules.Direction.Up, s0);

        Assert.AreEqual(s0, s1);
    }

    [Test]
    public void ShouldBeZombieTurnAfterBunnyTurn()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).AddZombie(0, 0, 0).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Down, s0);

        Assert.AreEqual(Turn.Types.Bunny, s0.turn.Type);
        Assert.AreEqual(Turn.Types.Zombie, s1.turn.Type);
    }

    [Test]
    public void ShouldBeBunnyTurnAfterLastZombieTurn()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).AddZombie(0, 0, 0).AddZombie(1, 1, 1).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Down, s0);
        WorldState s2 = Rules.MoveZombie(0, Rules.Direction.Up, s1);
        WorldState s3 = Rules.MoveZombie(1, Rules.Direction.Down, s2);

        Assert.AreEqual(Turn.Types.Bunny, s0.turn.Type);

        Assert.AreEqual(Turn.Types.Zombie, s1.turn.Type);
        Assert.AreEqual(0, s1.turn.Id);

        Assert.AreEqual(Turn.Types.Zombie, s2.turn.Type);
        Assert.AreEqual(1, s2.turn.Id);

        Assert.AreEqual(Turn.Types.Bunny, s3.turn.Type);
    }

    [Test]
    public void ShouldBeNextTurnAfterZombiePassesTurn()
    {
        WorldState s0 = new WorldStateBuilder(20, 20).PutBunny(10, 10).AddZombie(0, 0, 0).Build();

        WorldState s1 = Rules.MoveBunny(Rules.Direction.Down, s0);
        WorldState s2 = Rules.PassZombie(0, s1);

        Assert.AreEqual(Turn.Types.Bunny, s0.turn.Type);
        Assert.AreEqual(Turn.Types.Zombie, s1.turn.Type);
        Assert.AreEqual(Turn.Types.Bunny, s2.turn.Type);
    }

    [Test]
    public void ShouldReturnWinWhenBunnyAliveAndNoZombieAlive()
    {
        var current = new WorldStateBuilder(3, 3).Build();

        var results = Rules.GameStateCheck(current);

        Assert.AreEqual(Rules.GameState.Win, results);
    }

    [Test]
    public void ShouldReturnLoseWhenBunnyNotAlive()
    {
        var current = new WorldStateBuilder(3, 3).Build();

        current.bunny.alive = false;

        var results = Rules.GameStateCheck(current);

        Assert.AreEqual(Rules.GameState.Lose, results);
    }

    [Test]
    public void ShouldReturnInProgressWhenBunnyAliveAndAtLeastOneZombiesAlive()
    {
        var current = new WorldStateBuilder(3, 3).AddZombie(0, 1, 1).Build();

        var results = Rules.GameStateCheck(current);

        Assert.AreEqual(Rules.GameState.InProgress, results);
    }

    [Test]
    public void ShouldNotChangeWorldStateWhenBunnyMoveGoesOutOfBounds()
    {
        var s0 = new WorldStateBuilder(3, 3).Build();

        var s1 = Rules.MoveBunny(Rules.Direction.Left, s0);

        Assert.AreEqual(s0, s1);
    }

    [Test]
    public void ShouldPassFirstTurnForZombie()
    {
        var s0 = new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).Build();

        var s1 = Rules.MoveBunny(Rules.Direction.Up, s0);
        var s2 = Rules.MoveZombie(0, s1);

        Assert.AreEqual(Turn.Types.Bunny, s2.turn.Type);
        Assert.AreEqual(s1.zombies.First().position, s2.zombies.First().position);
    }

    [Test]
    public void ShouldPassSecondTurnForZombie()
    {
        var s0 = new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).Build();

        var s1 = Rules.MoveBunny(Rules.Direction.Up, s0);
        var s2 = Rules.MoveZombie(0, s1);
        var s3 = Rules.MoveBunny(Rules.Direction.Up, s2);
        var s4 = Rules.MoveZombie(0, s3);

        Assert.AreEqual(Turn.Types.Bunny, s4.turn.Type);
        Assert.AreEqual(s3.zombies.First().position, s4.zombies.First().position);
    }

    [Test]
    public void ShouldMoveThirdTurnForZombie()
    {
        var s0 = new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).Build();

        var s1 = Rules.MoveBunny(Rules.Direction.Up, s0);
        var s2 = Rules.MoveZombie(0, s1);
        var s3 = Rules.MoveBunny(Rules.Direction.Up, s2);
        var s4 = Rules.MoveZombie(0, s3);
        var s5 = Rules.MoveBunny(Rules.Direction.Up, s4);
        var s6 = Rules.MoveZombie(0, s5);

        Assert.AreEqual(Turn.Types.Bunny, s6.turn.Type);
        Assert.AreNotEqual(s5.zombies.First().position, s6.zombies.First().position);
    }

    [Test]
    public void ShouldMoveTowardBunny()
    {
        var s0 = new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).Build();

        var s1 = Rules.MoveBunny(Rules.Direction.Up, s0);
        var s2 = Rules.MoveZombie(0, s1);
        var s3 = Rules.MoveBunny(Rules.Direction.Down, s2);
        var s4 = Rules.MoveZombie(0, s3);
        var s5 = Rules.MoveBunny(Rules.Direction.Up, s4);
        var s6 = Rules.MoveZombie(0, s5);

        Assert.Less((s6.bunny.position - s6.zombies.First().position).Length, (s5.bunny.position - s5.zombies.First().position).Length);
    }
}
