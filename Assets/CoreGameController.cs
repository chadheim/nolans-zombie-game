using Assets.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreGameController : MonoBehaviour
{
    public int WorldWidth;
    public int WorldHeight;

    private WorldState current;
    private BunnyController bunnyController;
    private List<ZombieController> zombieControllers;
    private List<TrapController> trapControllers;

    void Start()
    {
        var builder = new WorldStateBuilder(WorldWidth, WorldHeight);

        bunnyController = FindFirstObjectByType<BunnyController>();
        builder.PutBunny((int)bunnyController.transform.position.x, (int)bunnyController.transform.position.y);

        int zombieId = 0;
        zombieControllers = FindObjectsByType<ZombieController>(FindObjectsSortMode.None).ToList();
        foreach (var controller in zombieControllers)
        {
            controller.Id = zombieId++;
            builder.AddZombie(controller.Id, (int)controller.transform.position.x, (int)controller.transform.position.y);
        }

        int trapId = 0;
        trapControllers = FindObjectsByType<TrapController>(FindObjectsSortMode.None).ToList();
        foreach (var controller in trapControllers)
        {
            controller.Id = trapId++;
            builder.AddTrap(controller.Id, (int)controller.transform.position.x, (int)controller.transform.position.y);
        }

        current = builder.Build();

        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (Rules.GameStateCheck(current) == Rules.GameState.InProgress)
        {
            WorldState next = null;

            if (current.turn.Type == Turn.Types.Bunny)
            {
                Rules.Direction? direction = null;
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    direction = Rules.Direction.Right;
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    direction = Rules.Direction.Left;
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                    direction = Rules.Direction.Up;
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    direction = Rules.Direction.Down;

                if (direction.HasValue)
                    next = Rules.MoveBunny(direction.Value, current);
            }
            else if (current.turn.Type == Turn.Types.Zombie)
            {
                next = Rules.MoveZombie(current.turn.Id, current);
            }

            if (next != null)
            {
                current = next;
                yield return bunnyController.Apply(current);
                foreach (var controller in zombieControllers)
                    yield return controller.Apply(current);
                foreach (var controller in trapControllers)
                    yield return controller.Apply(current);
            }
            else
            {
                yield return null;
            }
        }

        if (Rules.GameStateCheck(current) == Rules.GameState.Win)
        {
            Debug.Log("WIN!");
            GameData.CurrentLevelCompleted();
        }
        else if (Rules.GameStateCheck(current) == Rules.GameState.Lose)
        {
            Debug.Log("LOSE!");
        }

        yield return new WaitForSeconds(3);
        GameData.LoadMenu();
    }
}