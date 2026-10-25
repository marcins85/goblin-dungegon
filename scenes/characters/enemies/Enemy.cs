using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : CharacterBody3D
{
    public static readonly PackedScene EquipedItemPrefab =
        GD.Load<PackedScene>("res://scenes/equipment/equiped_item.tscn");

    public PhysicalBone3D torso;
    public PhysicalBoneSimulator3D skeletonSimulator;
    public CollisionShape3D collision;
    public EquipmentComponent equipment;
    public float impaleIntensity = 100f;
    public AnimationPlayer animationPlayer;
    public Area3D playerDetectionArea;
    public RayCast3D weaponReachRaycast;
    
    [Export]
    private Player _player;
    [Export]
    public ulong durationBetweenAttacks;
    public ulong timeSinceLastAttack;

    public enum State { MOVING, IMPALING, DYING, DEAD, SLASHING };
    private State _state;
    private EnemyState _stateNode;
    private Dictionary<State, Func<Enemy, EnemyStateData, EnemyState>> _stateMap;

    public override void _Ready()
    {
        torso = GetNode<PhysicalBone3D>("%Physical Bone Torso");
        skeletonSimulator = GetNode<PhysicalBoneSimulator3D>("%PhysicalBoneSimulator3D");
        collision = GetNode<CollisionShape3D>("%CollisionShape");
        equipment = GetNode<EquipmentComponent>("%EquipmentComponent");
        animationPlayer = GetNode<AnimationPlayer>("character/AnimationPlayer");
        playerDetectionArea = GetNode<Area3D>("%PlayerDetectionArea");
        weaponReachRaycast = GetNode<RayCast3D>("%WeaponReachRaycast");

        _stateMap = new Dictionary<State, Func<Enemy, EnemyStateData, EnemyState>>
        {
            { State.MOVING, (enemy, data) => new EnemyStateMoving(enemy, data) },
            { State.IMPALING, (enemy, data) => new EnemyStateImpaling(enemy, data) },
            { State.DYING, (enemy, data) => new EnemyStateDying(enemy, data) },
            { State.DEAD, (enemy, data) => new EnemyStateDead(enemy, data) },
            { State.SLASHING, (enemy, data) => new EnemyStateSlashing(enemy, data) },
        };

        playerDetectionArea.BodyEntered += OnPlayerDetected;
        SwitchState(State.MOVING);
    }

    private void OnPlayerDetected(Node3D body)
    {
        if (body is Player player)
        {
            _player = player;
            GD.Print("Player detected!");
        }
    }


    public void SwitchState(State newState, EnemyStateData data = null)
    {
        if (_stateNode != null)
        {
            _stateNode.QueueFree();
        }

        _stateNode = _stateMap[newState](this, data);
        _stateNode.TransitionRequested += OnTransitionRequested;
        _stateNode.Name = $"State_{newState}";
        _state = newState;
        AddChild(_stateNode);
    }

    private void OnTransitionRequested(int newState, EnemyStateData data)
    {
        SwitchState((State)newState, data);
    }

    public void Impale(ThrowedItem thrownItem, Basis basis)
    {
        var data = new EnemyStateData();
        data.ThrownItem = thrownItem;
        data.ThrownBasis = basis;
        SwitchState(State.IMPALING, data);
    }

    public bool HasRegisteredPlayer()
    {
        return _player != null && IsInstanceValid(_player);
    }

    public bool IsPlayerWithinReach()
    {
        if (HasRegisteredPlayer() && equipment.HasWeapon())
        {
            return weaponReachRaycast.IsColliding();
        }

        return false;
    }
}
