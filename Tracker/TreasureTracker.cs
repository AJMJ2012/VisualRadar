using Terraria;

namespace VisualRadar {
    public class TreasureTracker : Tracker {
        public TreasureTracker() {
			tracked = new TrackedTreasure[1];
			for (int i = 0; i < tracked.Length; i++)
				tracked[i] = new TrackedTreasure();
        }
        public override void Update(string context) {
			for (int i = 0; i < tracked.Length; i++) {
				((TrackedTreasure)tracked[i]).Get(context);
				tracked[i].lerpPosition = context == "screen";
				tracked[i].SetPosition(context);
				tracked[i].SetScale(context);
			}
        }
    }
}