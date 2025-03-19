```__| |____________________________________________________| |__
 __   ____________________________________________________   __
   | |                                                    | |
   | |________                  ____                      | |
   | |\______ \  __ __  ____   / ___\  ____  ____   ____  | |
   | | |    |  \|  |  \/    \ / /_/  >/ __ \/  _ \ /    \ | |
   | | |    `   \  |  /   |  \\___  /\  ___(  <_> )   |  \| |
   | |/_______  /____/|___|  /_____/  \___  >____/|___|  /| |
   | |_________/           \/       .__   \/           \/ | |
   | |\_   ___ \___________ __  _  _|  |   ___________    | |
   | |/    \  \|_  __ \__  \\ \/ \/ /  | _/ __ \_  __ \   | |
   | |\     \___|  | \// __ \\     /|  |_\  ___/|  | \/   | |
   | | \______  /__|  (____  /\/\_/ |____/\___  >__|      | |
   | |        \/           \/                 \/          | |
 __| |____________________________________________________| |__
 __   ____________________________________________________   __
   | |                                                    | |
```

This is largely a test project making use of some art from a game jam. But maybe it will become something fun.

## 📝 Todo

- [ ] Create enemy database.
  - [x] ~~Set up initial Enemy Database and plan schema of Enemy class.~~
  - [x] ~~Create Enemy JSON Converter.~~
  - [x] ~~Updated Enemy class to include Rarity~~
  - [ ] Set up Enemy move factory.
- [ ] Implement player stats.
- [ ] Implement combat
  - [x] ~~Set up turn order queue.~~
  - [ ] Create player attack bar UI.
- [ ] Create cosmetic item database.
- [ ] Implement item 'use' function.
- [ ] Implement error popups/feedback (inventory full, can't move, etc)

## ✅ Completed Tasks

- [x] ~~Implement recursive backtracking algorithm for dungeon generation.~~
- [x] ~~Create RoomData to store exits, items, and enemies.~~
- [x] ~~Visualize the dungeon with placeholder assets and add movement through rooms.~~
- [x] ~~Create item database using JSON/YAML to edit properties.~~
- [x] ~~Set up JSON parser to read data, validate properties, and create internal database.~~
- [x] ~~Visualize items in dungeon.~~
- [x] ~~Implement inventory.~~
  - [x] ~~GameManager loads an Inventory instance.~~
  - [x] ~~Create action to pick up items.~~
  - [x] ~~Visualize inventory.~~
  - [x] ~~Handle item removal~~.
  - [x] ~~Fixed bug where removed items were not null and new items couldn't be picked up~~
- [x] ~~Visualize enemies in dungeon.~~
  - [x] ~~Spawn enemy data in rooms.~~

### 📌 Notes

> - Should items stack? Undecided on the exact inventory system. Maybe even inventory tetris? Who knows?
> - Eventually, the control schema needs to be set up so keyboard, mouse, and controller are all equally viable ways to play + ability to rekeybind.
> - I'm leaning towards pre-rendered 3D dungeon backgrounds just to try to mitigate the amount of art assets needed. Not that 3D is _easy_ either. But I could reuse shapes and layouts a lot easier.
> - Need to think about the UI at some point since the current placeholders are crude. Do I really need a unique icon for every item? Maybe I can get by with text and simple icon markers? Or? Idk. Unique icons are fun but just bloat the amount of assets I need.
