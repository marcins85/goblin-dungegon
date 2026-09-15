using Godot;

public interface IPickable
{
    public WeaponData WeaponData { get; }
    public Transform3D Transform();
    public void Delete();
}