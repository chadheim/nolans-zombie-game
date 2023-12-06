using Assets.Core;
using NUnit.Framework;
using System;

public class WorldStateBuilderTests
{
    [Test]
    public void ShouldThrowExceptionWhenAddingZombieOutsideOfWorldBounds()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new WorldStateBuilder(10, 10).AddZombie(0, 11, 11));
    }

    [Test]
    public void ShouldThrowExceptionWhenAddingTrapOutsideOfWorldBounds()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new WorldStateBuilder(10, 10).AddTrap(0, 11, 11));
    }

    [Test]
    public void ShouldThrowExceptionWhenPuttingBunnyOutsideOfWorldBounds()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new WorldStateBuilder(10, 10).PutBunny(11, 11));
    }

    [Test]
    public void ShouldThrowExceptionWhenAddingZombieWithExistingId()
    {
        Assert.Throws<ArgumentException>(() => new WorldStateBuilder(10, 10).AddZombie(0, 0, 0).AddZombie(0, 1, 1));
    }

    [Test]
    public void ShouldThrowExceptionWhenAddingTrapWithExistingId()
    {
        Assert.Throws<ArgumentException>(() => new WorldStateBuilder(10, 10).AddTrap(0, 0, 0).AddTrap(0, 1, 1));
    }
}
