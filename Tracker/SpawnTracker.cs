namespace VisualRadar;

	public class SpawnTracker : Tracker {
    public SpawnTracker() {
			tracked = new TrackedSpawn[2];
			for (int i = 0; i < tracked.Length; i++)
				tracked[i] = new TrackedSpawn();
    }
    public override void Update(TrackedContext context) {
        for (int i = 0; i < tracked.Length; i++) {
            ((TrackedSpawn)tracked[i]).Get(context, i > 0);
				tracked[i].Animate(context);
				tracked[i].SetScale(context);
				tracked[i].SetPosition(context);
        }
    }
}
