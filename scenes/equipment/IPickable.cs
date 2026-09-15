using Godot;

public interface IPickable
{
    public WeaponData WeaponData { get; }
    public Transform3D TransformItem();
    public void Delete();
}