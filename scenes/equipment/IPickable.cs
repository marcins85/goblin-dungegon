using Godot;

public interface IPickable
{
    public WeaponData WeaponData { get; set; }
    public Transform3D GlobalTransform { get; set; }
    public void Delete();
    public Node3D AsNode();
}