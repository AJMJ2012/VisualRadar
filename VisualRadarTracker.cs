using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace VisualRadar
{
	public class VisualRadarTracker : ModSystem {
        public static VisualRadarTracker instance;
        public List<Tracker> trackedScreenObjects;
        public List<Tracker> trackedMapObjects;
		public VisualRadarTracker() {
            instance = this;
			trackedScreenObjects = new(){
				new SpawnTracker(),
				new NPCTracker(),
				new BagTracker(),
				new TreasureTracker(),
			};
			trackedMapObjects = new(){
				new SpawnTracker(),
				new BagTracker(),
				new NPCTracker(),
				new TreasureTracker(),
			};
        }
        public override void PostUpdateEverything() {
            if (Main.netMode == 2 || Main.gameMenu) return;
			foreach (Tracker tracked in trackedScreenObjects) {
				tracked.Update("screen");
			}
			foreach (Tracker tracked in trackedMapObjects) {
				tracked.Update("map");
			}
        }
    }
}
