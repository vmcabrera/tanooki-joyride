using System.Collections.Generic;
using Godot;
using TanookiJoyride.Src.Common.Components;
using TanookiJoyride.Src.Common.Utils;

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
        _spawnTimer.OneShot = true;
        _spawnTimer.Autostart = false;
        _spawnTimer.Timeout += OnSpawnTimeout;

        AddChild(_spawnTimer);
        _spawnTimer.Start();
    }

    private void OnSpawnTimeout()
    {
        _spawnTimer.Stop();

        float entityDelay = 0f;

        if (_randomizedEntitiesScenePaths != null && _randomizedEntitiesScenePaths.Count != 0)
        {
            Entity entity = SpawnRandomEntity();

            if (entity.HasComponent<SpawnCooldownComponent>())
            {
                entityDelay = entity.GetComponent<SpawnCooldownComponent>().Time;
            }
        }

        RandomizeSpawnTimer(entityDelay);
        _spawnTimer.Start();
    }

    private Entity SpawnRandomEntity()
    {
        string randomScenePath = _randomizedEntitiesScenePaths[RandomUtility.RandRange(0, _randomizedEntitiesScenePaths.Count - 1)];

        Entity entity = (Entity)ResourceLoader.Load<PackedScene>(randomScenePath).Instantiate();
        _entityManager.AddEntity(entity);

        return entity;
    }

    private void RandomizeSpawnTimer(float delay)
    {
        _spawnTimer.WaitTime = _randomWaitTimeArray[RandomUtility.RandRange(0, _randomWaitTimeArray.Length - 1)] + delay;
    }
}
