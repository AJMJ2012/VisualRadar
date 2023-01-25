using Terraria;

namespace VisualRadar {
    public class NPCTracker : Tracker {
        public NPCTracker() {
			tracked = new TrackedNPC[Main.npc.Length];
			for (int i = 0; i < tracked.Length; i++)
				tracked[i] = new TrackedNPC();
        }
        public override void Update(string context) {
            for (int i = 0; i < tracked.Length; i++) {
                NPC npc = Main.npc[i];
                ((TrackedNPC)tracked[i]).Get(context, npc, this);
            }
            for (int i = 0; i < tracked.Length; i++) {
                tracked[i].SetScale(context);
                tracked[i].SetPosition(context);
            }
        }
    }
}
