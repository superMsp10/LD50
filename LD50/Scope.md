Not need to use complex mesh generation just use cubes as blocks for now
- Performance should be fine

- [x] Blocks data structure
- [x] Rendering Blocks
- [x] Placing & Destroying Blocks
	- [x] Line rasterization
	- [x] Highlight dragged line
	- [x] Bottom right waudrant line rasterization broken
- [x] Build arena walls and temple and save as intial level

Foot enemy
- [x] Path find to temple
- [x] Destroy blocks if no path

Temple health
- [x] Enemy damage temple when within range
- [x] Enemy dies when it deals damage
- [x] Show Temple Health

Refactoring
- [x] Dont destroy all blocks everything there is an edit
	- [x] Edit per block
	- [x] Save references of block models using hash map
- [x] Enemy use last valid path in destruction mode

Wave system
- [ ] Spawn enemies from spawnspots
- [ ] Wave UI
	- [ ] Start next wave during build mode
	- [ ] Wave number
		- [ ]  Wave number vs Build mode status
	- [ ] Number of enemies left
	- [ ] Wave reward
- [ ] Temple health restored at end of round

Game start and over
- [ ] Show high score at game over screen