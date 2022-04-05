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
- [x] Spawn enemies from spawnspots
- [x] Wave UI
	- [x] Start next wave button during build mode
	- [x] Wave number
		- [x]  Wave number vs Build mode status
	- [x] Number of enemies left
	- [x] Wave reward
- [x] Temple health restored at end of round
- [x] Restart wave option

Game start and over
- [x] Show high score at game over screen

Attack
- [ ] Money system
- [ ] Hotbar
- [ ] Spikes
- [ ] Arrow
	- [ ] Shoot four ways
- [ ] Enemy change colour according to health