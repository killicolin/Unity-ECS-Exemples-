﻿ using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Jobs;

//boot class
public class UniversBoot: MonoBehaviour
{
    public Mesh _mesh;
    public Material _mat;
    [Space]
    public int _bodyNumber = 10;
    public float2 _SpawnAreaSize;
    public float _mass;
    [Space]
    EntityManager entityManager;
    void Start()
    {
        //Create entity manager, who will manage all the entity.
        //An Entity is basically a Gameobject.
        entityManager = World.Active.GetOrCreateManager<EntityManager>();
        EntityArchetype celestialArchetype = entityManager.CreateArchetype(typeof(CelestialBody), typeof(Position), ComponentType.Create<TransformMatrix>(), ComponentType.Create<MeshInstanceRenderer>());
        //Generate entities and position them in a grid
        CreateUnivers(celestialArchetype);
    }

    public void CreateUnivers(EntityArchetype celestialArchetype)
    {
        // create each entity
        for (int i = 0; i < _bodyNumber; i++)
        {
            Entity celestialBodyEntity;
            celestialBodyEntity = entityManager.CreateEntity(celestialArchetype);
            CreateCelestialBody(celestialBodyEntity);
        }
    }

    public void CreateCelestialBody(Entity celestialBodyEntity)
    {
        // init the position of the entity in the _SpawnAreaSize
        float2 initialPosition = new float2(Random.Range(-_SpawnAreaSize.x, _SpawnAreaSize.x), Random.Range(-_SpawnAreaSize.y, _SpawnAreaSize.y));
        //set the position
        entityManager.SetComponentData(celestialBodyEntity, new Position { Value = new float3(initialPosition,0)});
        //init the direction of the entity
        CelestialBody celestB = new CelestialBody
        {
            position = initialPosition,
            mass = _mass,
            velocity = 0,
            acceleration = 0,
        };
        //set the CelestialBody
        entityManager.SetComponentData(celestialBodyEntity, celestB);
        //set the shared MeshInstanceRenderer
        entityManager.SetSharedComponentData(celestialBodyEntity, new MeshInstanceRenderer
        {
            mesh = _mesh,
            material = _mat
        });
    }
}
