using GXPEngine;

public class Mover : Sprite{

    public float x {
        get {
            return position.x;
        }
        set {
            position.x = value;
        }
    }
    public float y {
        get {
            return position.y;
        }
        set {
            position.y = value;
        }
    }


    public Vec2 velocity;
    public Vec2 position;
    public Vec2 acceleration;
    public Vec2 oldPosition;
    public float radius; //20
    public float mass = 1f;

    public Mover(Vec2 newPosition, float radius) : base("ball.png", 2*radius/128, 2*radius/128, false) {
        this.radius = radius;
        position = newPosition;
        velocity = new Vec2();
        acceleration = new Vec2();
    }

    public Mover(float px, float py, float radius): base("ball.png", 2*radius/128, 2*radius/128, false)
    {
        this.radius = radius;
        position.x = px;
        position.y = py;
        SetOrigin(width/2, height/2);
        UpdateMoverSprite();
        velocity = new Vec2();
        acceleration = new Vec2();
    }
    public void MoveMover() {
        velocity += acceleration;
        position += velocity;

        oldPosition = position;

        acceleration.SetXY (0, 0);
    }

    public void AddAcceleration(Vec2 pacceleration) {
        acceleration += pacceleration;
    }

    public void UpdateMoverSprite() {
        SetXY(position.x, position.y);
    }

}