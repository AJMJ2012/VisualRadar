namespace VisualRadar
{
	public class Tracker {
        public TrackedObject[] tracked;
        public virtual void Update(TrackedContext context) {}
    }
}
