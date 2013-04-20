using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Collision;
using GameLibrary.Dependencies.Physics.Collision.Shapes;
using GameLibrary.Dependencies.Physics.Common;
using GameLibrary.Dependencies.Physics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceHordes.Entities.Components
{
    // Some useful structs
    internal struct shapeData
    {
        public PhysicsBody body;
        public float min; // absolute angles
        public float max;
    }

    internal struct rayData
    {
        public float angle;
        public Vector2 pos;
    }

    /// <summary>
    /// This is a comprarer used for
    /// detecting angle difference between rays
    /// </summary>
    internal class rayDataComparer : IComparer<rayData>
    {
        int IComparer<rayData>.Compare(rayData a, rayData b)
        {
            float diff = (a.angle - b.angle);
            if (diff > 0)
                return 1;
            else if (diff < 0)
                return -1;
            return 0;
        }
    }

    /// <summary>
    /// This is an explosive... it explodes.
    /// Original Code by Steven Lu
    /// (see http://www.box2d.org/forum/viewtopic.php?f=3&t=1688)
    /// Ported to Farseer 3.0
    /// </summary>
    public abstract class Explosive
    {
        private const int MAX_SHAPES = 100;
        private Dictionary<Fixture, List<Vector2>> exploded;
        private rayDataComparer rdc;
        private List<shapeData> data = new List<shapeData>();
        private World world;

        public Explosive(World farseerWorld)
        {
            exploded = new Dictionary<Fixture, List<Vector2>>();
            rdc = new rayDataComparer();
            data = new List<shapeData>();
            world = farseerWorld;
        }

        /// <summary>
        /// This makes the explosive explode
        /// <param name="pos">
        /// The position where the explosion happens
        /// </param>
        /// <param name="radius">
        /// The explosion radius
        /// </param>
        /// <param name="maxForce">
        /// The explosion force at the explosion point
        /// (then is inversely proportional to the square of the distance)
        /// </param>
        /// <returns>
        /// A dictionnary containing all the "exploded" fixtures
        /// with a list of the applied impulses
        /// </returns>
        /// </summary>
        public virtual Dictionary<Fixture, List<Vector2>> Explode(Entity setter, float damage, Vector2 pos, float radius, float maxForce)
        {
            exploded.Clear();

            AABB aabb;
            aabb.LowerBound = pos + new Vector2(-radius, -radius);
            aabb.UpperBound = pos + new Vector2(radius, radius);
            Fixture[] shapes = new Fixture[MAX_SHAPES];

            int shapeCount = 0;

            // Query the world for overlapping shapes.
            world.QueryAABB(
                fixture =>
                {
                    if(shapeCount < shapes.Length-1)
                    shapes[shapeCount++] = fixture;

                    // Continue the query.
                    return true;
                }, ref aabb);

            // check if the explosion point is contained inside of a shape
            bool isInsideSomething = false;
            for (int i = 0; i < shapeCount; ++i)
            {
                if (shapes[i].TestPoint(ref pos))
                {
                    isInsideSomething = true;
                    break;
                }
            }

            if (isInsideSomething)
            {
                // per shape max/min angles for now
                rayData[] vals = new rayData[shapeCount * 2];
                int valIndex = 0;
                for (int i = 0; i < shapeCount; ++i)
                {
                    PolygonShape ps;
                    CircleShape cs = shapes[i].Shape as CircleShape;

                    // If it is a circle, we create a diamond from it
                    if (cs != null)
                    {
                        Vertices v = new Vertices();
                        Vector2 vec = Vector2.Zero + new Vector2(cs.Radius, 0);
                        v.Add(vec);
                        vec = Vector2.Zero + new Vector2(0, cs.Radius);
                        v.Add(vec);
                        vec = Vector2.Zero + new Vector2(-cs.Radius, cs.Radius);
                        v.Add(vec);
                        vec = Vector2.Zero + new Vector2(0, -cs.Radius);
                        v.Add(vec);
                        ps = new PolygonShape(v, 1);
                    }
                    else
                        ps = shapes[i].Shape as PolygonShape;

                    if (ps != null)
                    {
                        Vector2 toCentroid = shapes[i].Body.GetWorldPoint(Vector2.Zero) - pos; //TODO: FIX
                        float angleToCentroid = (float)Math.Atan2(toCentroid.Y, toCentroid.X);
                        float min = float.MaxValue;
                        float max = float.MinValue;
                        float minAbsolute = 0.0f;
                        float maxAbsolute = 0.0f;
                        Vector2 minPt = new Vector2(-1, -1);
                        Vector2 maxPt = new Vector2(1, 1);

                        for (int j = 0; j < (ps.Vertices.Count()); ++j)
                        {
                            Vector2 toVertex = (shapes[i].Body.GetWorldPoint(ps.Vertices[j]) - pos);
                            float newAngle = (float)Math.Atan2(toVertex.Y, toVertex.X);
                            float diff = (newAngle - angleToCentroid);

                            diff = (diff - MathHelper.Pi) % (2 * MathHelper.Pi); // the minus pi is important.

                            // It means cutoff for going other
                            // direction is at 180 deg where it
                            // needs to be

                            if (diff < 0.0f)
                                diff += 2 * MathHelper.Pi; // correction for not handling negs

                            diff -= MathHelper.Pi;

                            if (Math.Abs(diff) > MathHelper.Pi)
                                throw new ArgumentException("OMG!"); // Something's wrong,

                            // point not in shape but
                            // exists angle diff > 180

                            if (diff > max)
                            {
                                max = diff;
                                maxAbsolute = newAngle;
                                maxPt = shapes[i].Body.GetWorldPoint(ps.Vertices[j]);
                            }
                            if (diff < min)
                            {
                                min = diff;
                                minAbsolute = newAngle;
                                minPt = shapes[i].Body.GetWorldPoint(ps.Vertices[j]);
                            }
                        }

                        vals[valIndex].angle = minAbsolute;
                        vals[valIndex].pos = minPt;
                        ++valIndex;
                        vals[valIndex].angle = maxAbsolute;
                        vals[valIndex].pos = maxPt;
                        ++valIndex;
                    }
                }

                Array.Sort(vals, 0, valIndex, rdc);
                data.Clear();
                bool rayMissed = true;

                for (int i = 0; i < valIndex; ++i)
                {
                    Fixture shape = null;
                    float midpt;

                    int iplus = (i == valIndex - 1 ? 0 : i + 1);
                    if (vals[i].angle == vals[iplus].angle)
                        continue;

                    if (i == valIndex - 1)
                    {
                        // the single edgecase
                        midpt = (vals[0].angle + MathHelper.Pi * 2 + vals[i].angle);
                    }
                    else
                    {
                        midpt = (vals[i + 1].angle + vals[i].angle);
                    }

                    midpt = midpt / 2;

                    Vector2 p1 = pos;
                    Vector2 p2 = radius * new Vector2((float)Math.Cos(midpt),
                        (float)Math.Sin(midpt)) + pos;

                    float fraction = 0;

                    // RaycastOne
                    bool hitClosest = false;
                    world.RayCast((f, p, n, fr) =>
                    {
                        PhysicsBody body = f.Body;
                        if (body.UserData != null && body.UserData is int)
                        {
                            int index = (int)body.UserData;
                            if (index == 0)
                            {
                                // filter
                                return -1.0f;
                            }
                        }

                        hitClosest = true;
                        shape = f;
                        fraction = fr;
                        return fr;
                    }, p1, p2);

                    //draws radius points
                    if ((hitClosest))
                    {
                        if ((data.Count() > 0) && (data.Last().body == shape.Body) && (!rayMissed))
                        {
                            int laPos = data.Count - 1;
                            shapeData la = data[laPos];
                            la.max = vals[iplus].angle;
                            data[laPos] = la;
                        }
                        else
                        {
                            // make new
                            shapeData d;
                            d.body = shape.Body;
                            d.min = vals[i].angle;
                            d.max = vals[iplus].angle;
                            data.Add(d);
                        }

                        if ((data.Count() > 1)
                            && (i == valIndex - 1)
                            && (data.Last().body == data.First().body)
                            && (data.Last().max == data.First().min))
                        {
                            shapeData fi = data[0];
                            fi.min = data.Last().min;
                            data.RemoveAt(data.Count() - 1);
                            data[0] = fi;
                            while (data.First().min >= data.First().max)
                            {
                                fi.min -= MathHelper.Pi * 2;
                                data[0] = fi;
                            }
                        }

                        int lastPos = data.Count - 1;
                        shapeData last = data[lastPos];
                        while ((data.Count() > 0)
                            && (data.Last().min >= data.Last().max)) // just making sure min<max
                        {
                            last.min = data.Last().min - 2 * MathHelper.Pi;
                            data[lastPos] = last;
                        }
                        rayMissed = false;
                    }
                    else
                    {
                        // add entry to data with body = NULL to indicate a lack of objects in this range.
                        // Useful for drawing/graphical/other purposes.
                        if (data.Count() > 0 && rayMissed && data.Last().body == null)
                        {
                            int laPos = data.Count - 1;
                            shapeData la = data[laPos];
                            la.max = vals[iplus].angle;
                            data[laPos] = la;
                        }
                        else
                        {
                            shapeData d;
                            d.body = null;
                            d.min = vals[i].angle;
                            d.max = vals[iplus].angle;
                            data.Add(d);
                        }

                        if ((data.Count() > 1) && (i == valIndex - 1)
                            && (data.First().body == null)
                            && (data.Last().max == data.First().min))
                        {
                            shapeData fi = data[0];
                            fi.min = data.Last().min;
                            data.RemoveAt(data.Count() - 1);
                            while (data.First().min >= data.First().max)
                            {
                                fi.min -= MathHelper.Pi * 2;
                                data[0] = fi;
                            }
                        }

                        int lastPos = data.Count - 1;
                        shapeData last = data[lastPos];
                        while ((data.Count() > 0)
                            && (data.Last().min >= data.Last().max)) // just making sure min<max
                        {
                            last.min = data.Last().min - 2 * MathHelper.Pi;
                            data[lastPos] = last;
                        }
                        rayMissed = true; // raycast did not find a shape
                    }
                }

                for (int i = 0; i < data.Count(); ++i)
                {
                    Vector2 vectImp = Vector2.Zero;
                    const int min_rays = 5; // for small arcs -- how many rays per shape/body/segment
                    const float max_angle = MathHelper.Pi / 15; // max angle between rays (used when segment is large)
                    const float edge_ratio = 1.0f / 40.0f; // ratio of arc length to angle from edges to first ray tested
                    const float max_edge_offset = MathHelper.Pi / 90; // two degrees: maximum angle

                    // from edges to first ray tested

                    float arclen = data[i].max - data[i].min;

                    if (data[i].body == null)
                    {
                        for (float j = data[i].min; j <= data[i].max; j += max_angle)
                        {
                            // Draw Debug stuff... if you want to.
                            // Nothing found
                        }
                        continue;
                    }

                    float first = MathHelper.Min(max_edge_offset, edge_ratio * arclen);
                    int inserted_rays = (int)Math.Ceiling((double)(((arclen - 2.0f * first) - (min_rays - 1) * max_angle) / max_angle));

                    if (inserted_rays < 0)
                        inserted_rays = 0;

                    float offset = (arclen - first * 2.0f) / ((float)min_rays + inserted_rays - 1);

                    int jj = 0;
                    for (float j = data[i].min + first; j <= data[i].max; j += offset)
                    {
                        Vector2 p1 = pos;
                        Vector2 p2 = pos + radius * new Vector2((float)Math.Cos(j), (float)Math.Sin(j));
                        Vector2 hitpoint = Vector2.Zero;
                        float minlambda = float.MaxValue;

                        List<Fixture> fl = data[i].body.FixtureList;
                        for (int x = 0; x < fl.Count; ++x)
                        {
                            Fixture f = fl[x];
                            RayCastInput ri;
                            ri.Point1 = p1;
                            ri.Point2 = p2;
                            ri.MaxFraction = 50f;

                            RayCastOutput ro;
                            if (f.RayCast(out ro, ref ri, 0))
                            {
                                if (minlambda > ro.Fraction)
                                {
                                    minlambda = ro.Fraction;
                                    hitpoint = ro.Fraction * p2 + (1 - ro.Fraction) * p1;
                                }
                            }

                            // the force that is to be applied for this particular ray.
                            // offset is angular coverage. lambda*length of segment is distance.
                            float impulse = (arclen / (float)(min_rays + inserted_rays))
                                * maxForce * 180.0f / MathHelper.Pi * (1.0f - Math.Min(1.0f, minlambda));

                            // We Apply the impulse!!!
                            vectImp = Vector2.Dot(impulse * new Vector2((float)Math.Cos(j),
                               (float)Math.Sin(j)), -ro.Normal) * new Vector2((float)Math.Cos(j),
                                   (float)Math.Sin(j));

                            //data[i].body.ApplyLinearImpulse(vectImp, hitpoint);

                            // We gather the fixtures for returning them
                            Vector2 val = Vector2.Zero;
                            List<Vector2> vectorList;
                            if (exploded.TryGetValue(f, out vectorList))
                            {
                                val.X += Math.Abs(vectImp.X);
                                val.Y += Math.Abs(vectImp.Y);
                                if(vectorList.Count < 2000)
                                vectorList.Add(val);
                            }
                            else
                            {
                                vectorList = new List<Vector2>();
                                val.X = Math.Abs(vectImp.X);
                                val.Y = Math.Abs(vectImp.Y);
                                if (vectorList.Count < 2000)
                                vectorList.Add(val);
                                exploded.Add(f, vectorList);
                            }

                            if (minlambda > 1.0f)
                            {
                                hitpoint = p2;
                            }

                            ++jj;
                        }
                    }

                    //Apply damage
                    if (data[i].body.UserData != null && data[i].body.UserData is Entity)
                    {
                        Entity e = data[i].body.UserData as Entity;
                        if (e.HasComponent<Health>() && e.GetComponent<Health>().IsAlive && e != setter)
                            e.GetComponent<Health>().SetHealth(setter, e.GetComponent<Health>().CurrentHealth - vectImp.Length());
                    }
                }
            }

            return exploded;
        }
    }
}