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
    public float impaleIntensity = 100f;
    public AnimationPlayer animationPlayer;

    public enum State { MOVING, IMPALING, DYING, DEAD };
    private State _state;
    private EnemyState _stateNode;
    private Dictionary<State, Func<Enemy, EnemyStateData, EnemyState>> _stateMap;

    public override void _Ready()
    {
        torso = GetNode<PhysicalBone3D>("%Physical Bone Torso");
        skeletonSimulator = GetNode<PhysicalBoneSimulator3D>("%PhysicalBoneSimulator3D");
        collision = GetNode<CollisionShape3D>("%CollisionShape");
        animationPlayer = GetNode<AnimationPlayer>("character/AnimationPlayer");

        _stateMap = new Dictionary<State, Func<Enemy, EnemyStateData, EnemyState>>
        {
            { State.MOVING, (enemy, data) => new EnemyStateMoving(enemy, data) },
            { State.IMPALING, (enemy, data) => new EnemyStateImpaling(enemy, data) },
            { State.DYING, (enemy, data) => new EnemyStateDying(enemy, data) },
            { State.DEAD, (enemy, data) => new EnemyStateDead(enemy, data) },
        };

        SwitchState(State.MOVING);
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

    // public void RegisterDeath()
    // {
    //     RegisterDeath(Vector3.Zero);
    // }

    // public void RegisterDeath(Vector3 impulse)
    // {
    //     var data = new EnemyStateData();
    //     data.Impulse = impulse;
    //     SwitchState(State.DYING, data);
    // }

    // private void FreezeRagdoll()
    // {
    //     foreach (var child in skeletonSimulator.GetChildren())
    //     {
    //         if (child is PhysicalBone3D bone)
    //         {
    //             var boneRID = bone.GetRid();
    //             PhysicsServer3D.BodySetState(boneRID, PhysicsServer3D.BodyState.Sleeping, true);
    //         }
    //     }
    // }
}
