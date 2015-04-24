#pragma strict
import SpriteTile;

// In addition to animation, this demo shows the usage of layers to reduce the number of tiles;
// specifically the grass edge tiles work with both the water and path tiles.

var level : TextAsset;

function Start () {
	Tile.SetCamera();
	Tile.LoadLevel (level);
	
	// This line animates the water tiles at 8.0fps, which start at tile #16 in set 4 and have a total of 8 tiles.
	Tile.AnimateTileRange (TileInfo(4, 16), 8, 8.0);
	
	// This line animates the #71 gem tile at 15.0fps, cycling through the 9 gem tiles.
	Tile.AnimateTile (TileInfo(4, 71), 9, 15.0);
	
	// This line animates the #79 gem tile. We want it to cycle through 5 specific gem tiles at 2fps,
	// rather than a range, so we specify the tiles in an array.
	var tiles = [TileInfo(4, 79), TileInfo(4, 77), TileInfo(4, 75), TileInfo(4, 73), TileInfo(4, 71)];
	Tile.AnimateTile (TileInfo(4, 79), tiles, 2.0);
}