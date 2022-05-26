using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using GXPEngine;
using GXPEngine.Core;

public class CollisionResolver {
    private VerletBody verletBody;
    private Mover mover;
    private List<Obstacle> obstacles;
    public bool collisionThisFrame;
    private float Cr = 0.95f; // coefficent of restitution
    private float fixedMult = 15f;
    
    public CollisionResolver(VerletBody verletBody, Mover mover, List<Obstacle> obstacles) {
        this.verletBody = verletBody;
        this.obstacles = obstacles;
        this.mover = mover;
    }
    /*
    public void VerletBoundaries() {
        foreach (VerletPoint p in verletBody.point) {
            if (p.position.y > 550) {
                p.y = 550; 
            }
            if (p.position.y < -550) {
                p.y = -550; 
            }

            if (p.x < 10) {
                p.x = 10; 
            }
            if (p.x > 790) {
                p.x = 790; 
            }
        }
    }
    */
    public void VerletMoverCollisionCheck() {
        
        Vec2 moverNormalToPLatform;

        foreach (VerletConstraint constraint in verletBody.constraint) {
            float ConstraintWidth = constraint.height / 2;
            moverNormalToPLatform = mover.position.VecNormalToLine(constraint.one.position, constraint.two.position);
            Vec2 onePosition = constraint.one.position - ConstraintWidth * moverNormalToPLatform.Normalized();
            Vec2 twoPosition = constraint.two.position - ConstraintWidth * moverNormalToPLatform.Normalized();
            
            if (moverNormalToPLatform.Length() - ConstraintWidth - mover.radius < float.Epsilon) { 
                Vec2 POI = mover.position.VecNormalToLine(onePosition, twoPosition) +
                               mover.position;
                if (new Vec2(POI.x - onePosition.x, POI.y - onePosition.y).Length() +
                    new Vec2(POI.x - twoPosition.x, POI.y - twoPosition.y).Length() <=
                    new Vec2(onePosition.x - twoPosition.x, onePosition.y - twoPosition.y).Length()) {
                    Vec2 offset = new Vec2();
                    if (mover.oldPosition.PointWhichSide(onePosition, twoPosition) ==
                        mover.position.PointWhichSide(onePosition, twoPosition)) {
                        offset = (mover.radius + ConstraintWidth) * moverNormalToPLatform.Normalized() - moverNormalToPLatform;
                    }
                    else { 
                        offset = -(mover.radius + ConstraintWidth) * moverNormalToPLatform.Normalized(); 
                    }

                    PlatformAndMoverCollision(offset, mover, constraint);
                }
            }
                
        }
    }

    public void ObstacleMoverCollisionCheck() {
        for (int i = 0; i < obstacles.Count; i++) {
            Vec2 MoverToObstacle = new Vec2(obstacles[i].position.x - mover.position.x, obstacles[i].position.y - mover.position.y);
            if (obstacles[i].radius + mover.radius >= MoverToObstacle.Length()) {
                mover.position -= (obstacles[i].radius + mover.radius) * MoverToObstacle.Normalized() - MoverToObstacle;
                mover.velocity = mover.velocity.VecReflect(Vec2.Unit().VecNormalToLine(mover.position, obstacles[i].position), Cr);
            }
        }
    }

    public void VerletObstacleCollisionCheck() {
        Vec2 obstacleToFirstPoint = new Vec2();
        Vec2 obstacleToSecondPoint = new Vec2();
        Vec2 obstacleNormalToPLatform;

        foreach (VerletConstraint constraint in verletBody.constraint) {
            for (int i = 0; i < obstacles.Count; i++) {
                
                obstacleToFirstPoint.SetXY(Mathf.Abs(obstacles[i].position.x - constraint.one.x),
                    Mathf.Abs(obstacles[i].position.y - constraint.one.y));
                obstacleToSecondPoint.SetXY(Mathf.Abs(obstacles[i].position.x - constraint.two.x),
                    Mathf.Abs(obstacles[i].position.y - constraint.two.y));
                obstacleNormalToPLatform =
                    obstacles[i].position.VecNormalToLine(constraint.one.position, constraint.two.position);
                
                if (obstacleNormalToPLatform.Length() - obstacles[i].radius < float.Epsilon) {
                    Vec2 POI = obstacles[i].position.VecNormalToLine(constraint.one.position, constraint.two.position) +
                               obstacles[i].position;
                    
                    if (new Vec2(POI.x - constraint.one.x, POI.y - constraint.one.y).Length() +
                        new Vec2(POI.x - constraint.two.x, POI.y - constraint.two.y).Length() <=
                        new Vec2(constraint.one.x - constraint.two.x, constraint.one.y - constraint.two.y).Length()) {
                        Vec2 offset = new Vec2();
                        
                        if (obstacles[i].position.PointWhichSide(constraint.one.position, constraint.two.position) ==
                            obstacles[i].position.PointWhichSide(constraint.one.oldPosition, constraint.two.oldPosition)) {
                            offset = obstacles[i].radius * obstacleNormalToPLatform.Normalized() - obstacleNormalToPLatform;
                        }
                        else {
                            offset = obstacles[i].radius * obstacleNormalToPLatform.Normalized() + obstacleNormalToPLatform;
                        }
                        
                        PlatformAndObstacleCollision( offset, constraint);
                    }
                }
            }
        }
    }

    private void PlatformAndObstacleCollision(Vec2 offset, VerletConstraint constraint) {
        constraint.one.position += offset;
        constraint.two.position += offset;
        
        Vec2 vConstraintBefore = Vec2.FindCenter(constraint.one.position, constraint.two.position) -
                                   Vec2.FindCenter(constraint.one.oldPosition, constraint.two.oldPosition);

        //constraint.one.oldPosition -= vConstraintBefore.VecReflect(constraint.one.position - constraint.two.position, Cr);
        //constraint.two.oldPosition -= vConstraintBefore.VecReflect(constraint.one.position - constraint.two.position, Cr);

    }

    private void PlatformAndMoverCollision(Vec2 offset, Mover mover, VerletConstraint constraint) {
        
        mover.position -= offset;

        if (!collisionThisFrame) {
            float vBallBefore = mover.velocity.Length();
            float vConstraintAfter;
            float vBallAfter;
            float vConstraintBefore = (Vec2.FindCenter(constraint.one.position, constraint.two.position) -
                                       Vec2.FindCenter(constraint.one.oldPosition, constraint.two.oldPosition)).Length();
            
            vBallAfter = (Cr * constraint.mass * (vBallBefore - vConstraintBefore) +
                          constraint.mass * vConstraintBefore +
                          mover.mass * vBallBefore) / (constraint.mass + mover.mass);

            if(constraint.one._fixed && constraint.two._fixed)
                mover.velocity = mover.velocity.VecReflect(new Vec2(constraint.one.x - constraint.two.x, constraint.one.y - constraint.two.y), 1) * Cr;
            else {
                mover.velocity = mover.velocity.VecReflect(new Vec2(constraint.one.x - constraint.two.x, constraint.one.y - constraint.two.y), 1).Normalized()  * vBallAfter;
            }
            Console.WriteLine(vConstraintBefore);
            vConstraintAfter = (Cr * mover.mass * (vConstraintBefore - vBallBefore) +
                                constraint.mass * vConstraintBefore +
                                mover.mass * vBallBefore) / (constraint.mass + mover.mass);
            Console.WriteLine(vConstraintAfter);
            Console.WriteLine("\n");
            if (!constraint.one._fixed && !constraint.two._fixed) {
                constraint.one.oldPosition -= offset.Normalized() * vConstraintAfter;
                constraint.two.oldPosition -= offset.Normalized() * vConstraintAfter;
            }
            else if(!constraint.one._fixed && constraint.two._fixed) {
                constraint.one.oldPosition -= offset.Normalized() * vConstraintAfter / fixedMult;
            }
            else if(constraint.one._fixed && !constraint.two._fixed) {
                constraint.two.oldPosition -= offset.Normalized() * vConstraintAfter / fixedMult;
            }
            
            Vec2 POIOnTheLine = mover.position + mover.radius * offset.Normalized();
            GiveAngularVelocity(constraint, POIOnTheLine, offset, vConstraintAfter, 2);
                
            collisionThisFrame = true;
        }
    }

    private void GiveAngularVelocity(VerletConstraint constraint, Vec2 POI, Vec2 offset, float speedAfter, float angularMult) {
        float lengthToPointOne = new Vec2(POI.x - constraint.one.x, POI.y - constraint.one.y).Length();
        float lengthToPointTwo = new Vec2(POI.x - constraint.two.x, POI.y - constraint.two.y).Length();

        if (!constraint.one._fixed && !constraint.two._fixed) {
            if (lengthToPointOne > lengthToPointTwo) {
                constraint.one.oldPosition -=
                    offset.Normalized() * lengthToPointTwo / (lengthToPointOne + lengthToPointTwo) * speedAfter * angularMult;
                constraint.two.oldPosition +=
                    offset.Normalized() * lengthToPointOne / (lengthToPointOne + lengthToPointTwo) * speedAfter * angularMult;
            }
            else {
                constraint.one.oldPosition +=
                    offset.Normalized() * lengthToPointTwo / (lengthToPointOne + lengthToPointTwo) * speedAfter * angularMult;
                constraint.two.oldPosition -=
                    offset.Normalized() * lengthToPointOne / (lengthToPointOne + lengthToPointTwo) * speedAfter * angularMult;
            } 
        }
        

    }

    public bool CheckFinish() {
        bool finish = mover.position.x > 1670 && mover.position.y < 1080;
        return finish;
    }
    


}
