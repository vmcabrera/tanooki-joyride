using System.Collections.Generic;
using System.Linq;
using Godot;

namespace TanookiJoyride.Src.Common.Entities;

public partial class EntitySpawner(EntityManager entityManager) : Node
{
    private Timer _spawnTimer = new();

    private readonly EntityManager _entityManager = entityManager;
    private readonly List<string> _randomizedEntitiesScenePaths = [];
    private readonly float[] _randomWaitTimeArray = [1.5f, 2.5f, 3f];

    private const float StartingWaitTime = 3.0f;

    public override void _Ready()
    {
        InitSpawnTimer();
    }

    public void InitSpawnTimer()
    {
        _spawnTimer.WaitTime = StartingWaitTime;
        _spawnTimer.OneShot = false;
        _spawnTimer.Timeout += OnSpawnTimeout;

        AddChild(_spawnTimer);
        _spawnTimer.Start();
    }

    private void OnSpawnTimeout()
    {
        if (_randomizedEntitiesScenePaths != null && _randomizedEntitiesScenePaths.Any())
        {
            SpawnRandomEntity();
        }

        RandomizeSpawnTimer();
    }

    private void SpawnRandomEntity()
    {
        string randomScenePath = _randomizedEntitiesScenePaths[GD.RandRange(0, _randomizedEntitiesScenePaths.Count - 1)];

        Entity entity = (Entity)ResourceLoader.Load<PackedScene>(randomScenePath).Instantiate();
        _entityManager.AddEntity(entity);
    }

    private void RandomizeSpawnTimer()
    {
        _spawnTimer.WaitTime = _randomWaitTimeArray[GD.RandRange(0, _randomWaitTimeArray.Length - 1)];
    }
}
