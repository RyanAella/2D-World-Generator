# Changelog


## [1.4.0] - 2023-01-11
## Added
- Generation of water tiles for Pseudo Random
- Generation of grass (for meadows and woods) and stones (for meadows, woods and cavity)


## [1.3.0] - 2023-01-09
## Added
- Generation of water tiles for gradient noise

## Changed
- Minor changes in the Pseudo Random generation process


## [1.2.1] - 2023-01-03
## Changed
- Added new bushes to TilePalette Bush

## Fixes
- Wrong Pixels Per Unit for Meadows and Woods Tiles
- Wrong Pivets for Tree Sprites


## [1.1.0] - 2023-01-01
## Added
- ScriptableObjects with List<Tile> for each AssetType and the Bioms


## [1.0.1] - 2022-12-30
[RELEASE]

## Changed
- Minor Typos


## [0.10.1] - 2022-12-28
## Added
- Entire generation process in Pseudo Random


## [0.9.1] - 2022-12-27
## Added
- Collider (Tilemap and Composite) for MassiveRock, Wall and Tree Layers

## Changed
- Walls were created on cells that should have been massive rock
- Trees and Bushes now look better


## [0.8.0] - 2022-12-26
## Added
- Dictionary in Cell class containing the Tiles a cell adds to a layer/Tilemap
- Sprites for Bioms and Assets

## Changed
- TilemapGenerator adds Tiles to Tilemap based on information stored in the Dictionary of each cell


## [0.7.0] - 2022-12-24
## Added
- Cell has now attribute Tile

## Changed
- Different Asset Types are now on different Tilemaps based on collidable, interactable an collidableInteractable


## [0.6.0] - 2022-12-23
## Added
- Constructor for CellAsset

## Changed
- Massive Rock is now completely black


## [0.5.0] - 2022-12-22
## Added
- Percentages for trees, bushes and grass as fields in inspector


## [0.4.0] - 2022-12-18
## Added
- Decided against Pseudo Random
- Comments


## [0.3.0] - 2022-12-16 
## Added
- Bioms and Assets for cells
- Assets can be collidable
- Different Bioms for indoor and outdoor cells


## [0.2.0] - 2022-12-15
## Added
- indoor/outdoor Lists

## Changed
- Value Generation in one Script


## [0.1.0] - 2022-12-14
## Added
- Map Generator Script
- Cell Map Generator
  - Pseudo Random
  - Open Simplex Noise
  - Perlin Noise
- CellDebugger
- Tilemap Generator

## [0.0.1] - 2022-12-08
## Added
- Cell class
- Generating an array of Cells with either o or 1 as value
- Smoothing of the CellMap
- Generating Tilemap from CellMap