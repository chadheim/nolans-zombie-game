using Assets.Core;
using Moq;
using NUnit.Framework;
using System.Linq;

public class GameStateMachine
{
    public enum States
    {
        WaitingForBunnyMove,
        WaitingForZombieMove,
    }
    public States State { get; private set; }

    public WorldState WorldState { get; private set; }

    private int currentZombieMove;

    public GameStateMachine(WorldStateBuilder builder)
    {
        WorldState = builder.Build();
    }

    internal void BunnyMove()
    {
        currentZombieMove = 0;
        State = States.WaitingForZombieMove;
    }

    internal void ZombieMove()
    {
        currentZombieMove++;
        if (currentZombieMove >= WorldState.zombies.Count)
            State = States.WaitingForBunnyMove;
    }
}

public class ExperimentalTests
{
    [Test]
    public void ShouldBeWaitingForBunnyMoveStateWhenConstructed()
    {
        GameStateMachine gsm = new(new WorldStateBuilder(20, 20));

        Assert.AreEqual(gsm.State, GameStateMachine.States.WaitingForBunnyMove);
    }


    [Test]
    public void ShouldBeWaitingForZombieMoveAfterBunnyMove()
    {
        GameStateMachine gsm = new(new WorldStateBuilder(20, 20).AddZombie(0, 10, 10));

        gsm.BunnyMove();

        Assert.AreEqual(gsm.State, GameStateMachine.States.WaitingForZombieMove);
    }

    [Test]
    public void ShouldBeWaitingForZombieMoveAfterWaitingForZombieMove()
    {
        GameStateMachine gsm = new(new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).AddZombie(1, 11, 11));

        gsm.BunnyMove();
        gsm.ZombieMove();

        Assert.AreEqual(gsm.State, GameStateMachine.States.WaitingForZombieMove);
    }

    [Test]
    public void ShouldBeWaitingForBunnyMoveAfterLastWaitingForZombieMove()
    {
        GameStateMachine gsm = new(new WorldStateBuilder(20, 20).AddZombie(0, 10, 10).AddZombie(1, 11, 11));

        gsm.BunnyMove();
        gsm.ZombieMove();
        gsm.ZombieMove();

        Assert.AreEqual(gsm.State, GameStateMachine.States.WaitingForBunnyMove);
    }

    [Test]
    public void ShouldNotChangeStatesWhenCallingMoveBunnyWhileWaitingForZombieMove()
    {
        GameStateMachine gsm = new(new WorldStateBuilder(20, 20).AddZombie(0, 10, 10));

        gsm.BunnyMove();
        gsm.BunnyMove();

        Assert.AreEqual(gsm.State, GameStateMachine.States.WaitingForZombieMove);
    }

    [Test]
    public void ShouldNotChangeStatesWhenCallingMoveZombieWhileWaitingForBunnyMove()
    {
        GameStateMachine gsm = new(new WorldStateBuilder(20, 20).AddZombie(0, 10, 10));

        gsm.BunnyMove();
        gsm.ZombieMove();
        gsm.ZombieMove();

        Assert.AreEqual(gsm.State, GameStateMachine.States.WaitingForBunnyMove);
    }

    public interface ITurnHandler
    {
        WorldState BunnyTurn(WorldState current);
        WorldState ZombieTurn(int zombieId, WorldState current);
    }

    public class Round
    {
        public static WorldState Turn(ITurnHandler handler, WorldState current)
        {
            current = handler.BunnyTurn(current);

            var zombieIds = current.zombies.Select(z => z.id).ToList();
            foreach (var zombieId in zombieIds)
                current = handler.ZombieTurn(zombieId, current);

            return current;
        }
    }

    [Test]
    public void ShouldCallBunnyTurnForBunny()
    {
        var current = new WorldStateBuilder(10, 10)
            .Build();

        var mock = new Mock<ITurnHandler>();
        mock.Setup(obj => obj.BunnyTurn(It.IsAny<WorldState>()))
            .Returns(current);
        mock.Setup(obj => obj.ZombieTurn(It.IsAny<int>(), It.IsAny<WorldState>()))
            .Returns(current);

        current = Round.Turn(mock.Object, current);

        mock.Verify(obj => obj.BunnyTurn(It.IsAny<WorldState>()), Times.Once());
    }

    [Test]
    public void ShouldCallZombieTurnForEachZombie()
    {
        var current = new WorldStateBuilder(10, 10)
            .AddZombie(1, 1, 1)
            .AddZombie(2, 2, 2)
            .AddZombie(3, 3, 3)
            .Build();

        var mock = new Mock<ITurnHandler>();
        mock.Setup(obj => obj.BunnyTurn(It.IsAny<WorldState>()))
            .Returns(current);
        mock.Setup(obj => obj.ZombieTurn(It.IsAny<int>(), It.IsAny<WorldState>()))
            .Returns(current);

        current = Round.Turn(mock.Object, current);

        mock.Verify(obj => obj.ZombieTurn(1, It.IsAny<WorldState>()), Times.Once());
        mock.Verify(obj => obj.ZombieTurn(2, It.IsAny<WorldState>()), Times.Once());
        mock.Verify(obj => obj.ZombieTurn(3, It.IsAny<WorldState>()), Times.Once());
    }
}
