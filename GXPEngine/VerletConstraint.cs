// Basic constraint: two points should be at a given distance from each other:
public class VerletConstraint {
	public VerletPoint one;
	public VerletPoint two;
	public readonly float length;

	public VerletConstraint(VerletPoint pOne, VerletPoint pTwo) {
		one = pOne;
		two = pTwo;
		length = (pTwo.position - pOne.position).Length ();
	}

	public void Apply() {
		Vec2 diff = two.position - one.position;
		float currentlength = diff.Length();
		diff.Normalize ();
		diff = (length - currentlength) * diff;
		
		if (!one._fixed && !two._fixed) {
			one.position -= diff * 0.5f;
			two.position += diff * 0.5f;
		} else if (!one._fixed && two._fixed) {
			one.position -= diff;
		} else if (one._fixed && !two._fixed) {
			two.position += diff;
		}
		
	}
}