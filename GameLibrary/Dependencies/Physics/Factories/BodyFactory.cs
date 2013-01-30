using System;
using System.Collections.Generic;
using GameLibrary.Dependencies.Physics.Collision.Shapes;
using GameLibrary.Dependencies.Physics.Common;
using GameLibrary.Dependencies.Physics.Common.Decomposition;
using GameLibrary.Dependencies.Physics.Dynamics;
using Microsoft.Xna.Framework;

namespace GameLibrary.Dependencies.Physics.Factories
{
    public static class BodyFactory
    {
        public static PhysicsBody CreateBody(PhysicsWorld world)
        {
            return CreateBody(world, null);
        }

        public static PhysicsBody CreateBody(PhysicsWorld world, object userData)
        {
            PhysicsBody body = new PhysicsBody(world, userData);
            return body;
        }

        public static PhysicsBody CreateBody(PhysicsWorld world, Vector2 position)
        {
            return CreateBody(world, position, null);
        }

        public static PhysicsBody CreateBody(PhysicsWorld world, Vector2 position, object userData)
        {
            PhysicsBody body = CreateBody(world, userData);
            body.Position = position;
            return body;
        }

        public static PhysicsBody CreateEdge(PhysicsWorld world, Vector2 start, Vector2 end)
        {
            return CreateEdge(world, start, end, null);
        }

        public static PhysicsBody CreateEdge(PhysicsWorld world, Vector2 start, Vector2 end, object userData)
        {
            PhysicsBody body = CreateBody(world);
            FixtureFactory.AttachEdge(start, end, body, userData);
            return body;
        }

        public static PhysicsBody CreateLoopShape(PhysicsWorld world, Vertices vertices)
        {
            return CreateLoopShape(world, vertices, null);
        }

        public static PhysicsBody CreateLoopShape(PhysicsWorld world, Vertices vertices, object userData)
        {
            return CreateLoopShape(world, vertices, Vector2.Zero, userData);
        }

        public static PhysicsBody CreateLoopShape(PhysicsWorld world, Vertices vertices, Vector2 position)
        {
            return CreateLoopShape(world, vertices, position, null);
        }

        public static PhysicsBody CreateLoopShape(PhysicsWorld world, Vertices vertices, Vector2 position,
                                           object userData)
        {
            PhysicsBody body = CreateBody(world, position);
            FixtureFactory.AttachLoopShape(vertices, body, userData);
            return body;
        }

        public static PhysicsBody CreateRectangle(PhysicsWorld world, float width, float height, float density)
        {
            return CreateRectangle(world, width, height, density, null);
        }

        public static PhysicsBody CreateRectangle(PhysicsWorld world, float width, float height, float density, object userData)
        {
            return CreateRectangle(world, width, height, density, Vector2.Zero, userData);
        }

        public static PhysicsBody CreateRectangle(PhysicsWorld world, float width, float height, float density, Vector2 position)
        {
            return CreateRectangle(world, width, height, density, position, null);
        }

        public static PhysicsBody CreateRectangle(PhysicsWorld world, float width, float height, float density, Vector2 position,
                                           object userData)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", "Width must be more than 0 meters");

            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", "Height must be more than 0 meters");

            PhysicsBody newBody = CreateBody(world, position);
            Vertices rectangleVertices = PolygonTools.CreateRectangle(width / 2, height / 2);
            PolygonShape rectangleShape = new PolygonShape(rectangleVertices, density);
            newBody.CreateFixture(rectangleShape, userData);

            return newBody;
        }

        public static PhysicsBody CreateCircle(PhysicsWorld world, float radius, float density)
        {
            return CreateCircle(world, radius, density, null);
        }

        public static PhysicsBody CreateCircle(PhysicsWorld world, float radius, float density, object userData)
        {
            return CreateCircle(world, radius, density, Vector2.Zero, userData);
        }

        public static PhysicsBody CreateCircle(PhysicsWorld world, float radius, float density, Vector2 position)
        {
            return CreateCircle(world, radius, density, position, null);
        }

        public static PhysicsBody CreateCircle(PhysicsWorld world, float radius, float density, Vector2 position, object userData)
        {
            PhysicsBody body = CreateBody(world, position);
            FixtureFactory.AttachCircle(radius, density, body, userData);
            return body;
        }

        public static PhysicsBody CreateEllipse(PhysicsWorld world, float xRadius, float yRadius, int edges, float density)
        {
            return CreateEllipse(world, xRadius, yRadius, edges, density, null);
        }

        public static PhysicsBody CreateEllipse(PhysicsWorld world, float xRadius, float yRadius, int edges, float density,
                                         object userData)
        {
            return CreateEllipse(world, xRadius, yRadius, edges, density, Vector2.Zero, userData);
        }

        public static PhysicsBody CreateEllipse(PhysicsWorld world, float xRadius, float yRadius, int edges, float density,
                                         Vector2 position)
        {
            return CreateEllipse(world, xRadius, yRadius, edges, density, position, null);
        }

        public static PhysicsBody CreateEllipse(PhysicsWorld world, float xRadius, float yRadius, int edges, float density,
                                         Vector2 position, object userData)
        {
            PhysicsBody body = CreateBody(world, position);
            FixtureFactory.AttachEllipse(xRadius, yRadius, edges, density, body, userData);
            return body;
        }

        public static PhysicsBody CreatePolygon(PhysicsWorld world, Vertices vertices, float density)
        {
            return CreatePolygon(world, vertices, density, null);
        }

        public static PhysicsBody CreatePolygon(PhysicsWorld world, Vertices vertices, float density, object userData)
        {
            return CreatePolygon(world, vertices, density, Vector2.Zero, userData);
        }

        public static PhysicsBody CreatePolygon(PhysicsWorld world, Vertices vertices, float density, Vector2 position)
        {
            return CreatePolygon(world, vertices, density, position, null);
        }

        public static PhysicsBody CreatePolygon(PhysicsWorld world, Vertices vertices, float density, Vector2 position,
                                         object userData)
        {
            PhysicsBody body = CreateBody(world, position);
            FixtureFactory.AttachPolygon(vertices, density, body, userData);
            return body;
        }

        public static PhysicsBody CreateCompoundPolygon(PhysicsWorld world, List<Vertices> list, float density)
        {
            return CreateCompoundPolygon(world, list, density, BodyType.Static);
        }

        public static PhysicsBody CreateCompoundPolygon(PhysicsWorld world, List<Vertices> list, float density,
                                                 object userData)
        {
            return CreateCompoundPolygon(world, list, density, Vector2.Zero, userData);
        }

        public static PhysicsBody CreateCompoundPolygon(PhysicsWorld world, List<Vertices> list, float density,
                                                 Vector2 position)
        {
            return CreateCompoundPolygon(world, list, density, position, null);
        }

        public static PhysicsBody CreateCompoundPolygon(PhysicsWorld world, List<Vertices> list, float density,
                                                 Vector2 position, object userData)
        {
            //We create a single body
            PhysicsBody polygonBody = CreateBody(world, position);
            FixtureFactory.AttachCompoundPolygon(list, density, polygonBody, userData);
            return polygonBody;
        }


        public static PhysicsBody CreateGear(PhysicsWorld world, float radius, int numberOfTeeth, float tipPercentage,
                                      float toothHeight, float density)
        {
            return CreateGear(world, radius, numberOfTeeth, tipPercentage, toothHeight, density, null);
        }

        public static PhysicsBody CreateGear(PhysicsWorld world, float radius, int numberOfTeeth, float tipPercentage,
                                      float toothHeight, float density, object userData)
        {
            Vertices gearPolygon = PolygonTools.CreateGear(radius, numberOfTeeth, tipPercentage, toothHeight);

            //Gears can in some cases be convex
            if (!gearPolygon.IsConvex())
            {
                //Decompose the gear:
                List<Vertices> list = EarclipDecomposer.ConvexPartition(gearPolygon);

                return CreateCompoundPolygon(world, list, density, userData);
            }

            return CreatePolygon(world, gearPolygon, density, userData);
        }

        /// <summary>
        /// Creates a capsule.
        /// Note: Automatically decomposes the capsule if it contains too many vertices (controlled by Settings.MaxPolygonVertices)
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="height">The height.</param>
        /// <param name="topRadius">The top radius.</param>
        /// <param name="topEdges">The top edges.</param>
        /// <param name="bottomRadius">The bottom radius.</param>
        /// <param name="bottomEdges">The bottom edges.</param>
        /// <param name="density">The density.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static PhysicsBody CreateCapsule(PhysicsWorld world, float height, float topRadius, int topEdges,
                                         float bottomRadius,
                                         int bottomEdges, float density, Vector2 position, object userData)
        {
            Vertices verts = PolygonTools.CreateCapsule(height, topRadius, topEdges, bottomRadius, bottomEdges);

            PhysicsBody body;

            //There are too many vertices in the capsule. We decompose it.
            if (verts.Count >= Settings.MaxPolygonVertices)
            {
                List<Vertices> vertList = EarclipDecomposer.ConvexPartition(verts);
                body = CreateCompoundPolygon(world, vertList, density, userData);
                body.Position = position;

                return body;
            }

            body = CreatePolygon(world, verts, density, userData);
            body.Position = position;

            return body;
        }

        public static PhysicsBody CreateCapsule(PhysicsWorld world, float height, float topRadius, int topEdges,
                                         float bottomRadius,
                                         int bottomEdges, float density, Vector2 position)
        {
            return CreateCapsule(world, height, topRadius, topEdges, bottomRadius, bottomEdges, density, position, null);
        }

        public static PhysicsBody CreateCapsule(PhysicsWorld world, float height, float endRadius, float density)
        {
            return CreateCapsule(world, height, endRadius, density, null);
        }

        public static PhysicsBody CreateCapsule(PhysicsWorld world, float height, float endRadius, float density,
                                         object userData)
        {
            //Create the middle rectangle
            Vertices rectangle = PolygonTools.CreateRectangle(endRadius, height / 2);

            List<Vertices> list = new List<Vertices>();
            list.Add(rectangle);

            PhysicsBody body = CreateCompoundPolygon(world, list, density, userData);

            //Create the two circles
            CircleShape topCircle = new CircleShape(endRadius, density);
            topCircle.Position = new Vector2(0, height / 2);
            body.CreateFixture(topCircle, userData);

            CircleShape bottomCircle = new CircleShape(endRadius, density);
            bottomCircle.Position = new Vector2(0, -(height / 2));
            body.CreateFixture(bottomCircle, userData);
            return body;
        }

        /// <summary>
        /// Creates a rounded rectangle.
        /// Note: Automatically decomposes the capsule if it contains too many vertices (controlled by Settings.MaxPolygonVertices)
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="xRadius">The x radius.</param>
        /// <param name="yRadius">The y radius.</param>
        /// <param name="segments">The segments.</param>
        /// <param name="density">The density.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static PhysicsBody CreateRoundedRectangle(PhysicsWorld world, float width, float height, float xRadius,
                                                  float yRadius,
                                                  int segments, float density, Vector2 position,
                                                  object userData)
        {
            Vertices verts = PolygonTools.CreateRoundedRectangle(width, height, xRadius, yRadius, segments);

            //There are too many vertices in the capsule. We decompose it.
            if (verts.Count >= Settings.MaxPolygonVertices)
            {
                List<Vertices> vertList = EarclipDecomposer.ConvexPartition(verts);
                PhysicsBody body = CreateCompoundPolygon(world, vertList, density, userData);
                body.Position = position;
                return body;
            }

            return CreatePolygon(world, verts, density);
        }

        public static PhysicsBody CreateRoundedRectangle(PhysicsWorld world, float width, float height, float xRadius,
                                                  float yRadius,
                                                  int segments, float density, Vector2 position)
        {
            return CreateRoundedRectangle(world, width, height, xRadius, yRadius, segments, density, position, null);
        }

        public static PhysicsBody CreateRoundedRectangle(PhysicsWorld world, float width, float height, float xRadius,
                                                  float yRadius,
                                                  int segments, float density)
        {
            return CreateRoundedRectangle(world, width, height, xRadius, yRadius, segments, density, null);
        }

        public static PhysicsBody CreateRoundedRectangle(PhysicsWorld world, float width, float height, float xRadius,
                                                  float yRadius,
                                                  int segments, float density, object userData)
        {
            return CreateRoundedRectangle(world, width, height, xRadius, yRadius, segments, density, Vector2.Zero,
                                          userData);
        }

        public static PhysicsBreakableBody CreateBreakableBody(PhysicsWorld world, Vertices vertices, float density)
        {
            return CreateBreakableBody(world, vertices, density, null);
        }

        public static PhysicsBreakableBody CreateBreakableBody(PhysicsWorld world, Vertices vertices, float density, object userData)
        {
            return CreateBreakableBody(world, vertices, density, Vector2.Zero, userData);
        }

        /// <summary>
        /// Creates a breakable body. You would want to remove collinear points before using this.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="vertices">The vertices.</param>
        /// <param name="density">The density.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static PhysicsBreakableBody CreateBreakableBody(PhysicsWorld world, Vertices vertices, float density, Vector2 position,
                                                        object userData)
        {
            List<Vertices> triangles = EarclipDecomposer.ConvexPartition(vertices);

            PhysicsBreakableBody breakableBody = new PhysicsBreakableBody(triangles, world, density, userData);
            breakableBody.MainBody.Position = position;
            world.AddBreakableBody(breakableBody);

            return breakableBody;
        }

        public static PhysicsBreakableBody CreateBreakableBody(PhysicsWorld world, Vertices vertices, float density, Vector2 position)
        {
            return CreateBreakableBody(world, vertices, density, position, null);
        }

        public static PhysicsBody CreateLineArc(PhysicsWorld world, float radians, int sides, float radius, Vector2 position,
                                         float angle, bool closed)
        {
            PhysicsBody body = CreateBody(world);
            FixtureFactory.AttachLineArc(radians, sides, radius, position, angle, closed, body);
            return body;
        }

        public static PhysicsBody CreateSolidArc(PhysicsWorld world, float density, float radians, int sides, float radius,
                                          Vector2 position, float angle)
        {
            PhysicsBody body = CreateBody(world);
            FixtureFactory.AttachSolidArc(density, radians, sides, radius, position, angle, body);
            return body;
        }
    }
}