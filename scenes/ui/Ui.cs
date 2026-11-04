using Godot;
using System;

public partial class Ui : CanvasLayer
{
	private Panel _hurtVignette;

    public override void _Ready()
    {
        _hurtVignette = GetNode<Panel>("%HurtVignette");
		GameEvents.Instance.PlayerHurt += OnPlayerHurt;
    }

    private void OnPlayerHurt(Player player)
    {
        var tween = CreateTween();
		tween.TweenProperty(_hurtVignette, "modulate:a", 1.0, 0.1);
		tween.TweenProperty(_hurtVignette, "modulate:a", 0.0, 0.1);
    }
}
