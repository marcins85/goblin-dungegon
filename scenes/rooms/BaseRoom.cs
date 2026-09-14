using Godot;
using System;
using System.Collections.Generic;

public partial class BaseRoom : Node3D
{
	private GridMap _ceilings;
	private GridMap _floors;
	private List<int> _cellIdsWithNoCeiling	= new();

	public override void _Ready()
	{
		_ceilings = GetNode<GridMap>("Ceilings");
		_floors = GetNode<GridMap>("Floors");

		FillCeilings();
	}

    private void FillCeilings()
    {
        foreach (string cellName in new[] { "Ground", "Hole-Corner", "Hole-Side", "Hole-UTurn" })
		{
			_cellIdsWithNoCeiling.Add(_floors.MeshLibrary.FindItemByName(cellName));
		}

		var usedCells = _floors.GetUsedCells();

		foreach (var cellCords in usedCells)
		{
			int titleId = _floors.GetCellItem(cellCords);
			if (_cellIdsWithNoCeiling.Contains(titleId))
			{
				_ceilings.SetCellItem(cellCords, 0);
			}
		}
    }

}
