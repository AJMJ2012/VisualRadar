using Terraria;

namespace VisualRadar {
    public class BagTracker : Tracker {
        public BagTracker() {
			tracked = new TrackedBag[Main.item.Length];
			for (int i = 0; i < tracked.Length; i++)
				tracked[i] = new TrackedBag();
        }
        public override void Update(string context) {
            for (int i = 0; i < tracked.Length; i++) {
                Item item = Main.item[i];
                ((TrackedBag)tracked[i]).Get(context, item);
                tracked[i].SetPosition(context);
                tracked[i].SetScale(context);
            }
        }
    }
}
