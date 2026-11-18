using Terraria;

namespace VisualRadar; 
public class NPCTracker : Tracker {
    public NPCTracker() {
			tracked = new TrackedNPC[Main.npc.Length];
			for (int i = 0; i < tracked.Length; i++)
				tracked[i] = new TrackedNPC();
    }
    public override void Update(TrackedContext context) {
        for (int i = 0; i < tracked.Length; i++) {
            NPC npc = Main.npc[tracked.Length-1-i];
            ((TrackedNPC)tracked[i]).Get(context, npc, this);
        }
        for (int i = 0; i < tracked.Length; i++) {
            tracked[i].Animate(context);
				tracked[i].SetScale(context);
				tracked[i].SetPosition(context);
        }
    }
}
