using GXPEngine;

class Draw : EasyDraw {
	public Draw(int width, int height) : base(width,height) {
		StrokeWeight (4);
	}

	public void DrawVerlet(VerletBody body) {
		Stroke (255, 0, 0);
		foreach (VerletConstraint c in body.constraint) {
			Line (c.one.position.x, c.one.position.y, c.two.position.x, c.two.position.y);
		}
		Stroke(0,255,0);
		Fill (0, 255, 0);
		foreach (VerletPoint p in body.point) {
			Ellipse(p.position.x,p.position.y,8,8);
		}
	}

	public void DrawMover(Mover mover) {
		Stroke(0, 96, 255);
		Fill(0, 96, 255);
		Ellipse(mover.x, mover.y, mover.radius * 2, mover.radius * 2);
	}
}
