// UofU-CS3500 
//<authors> Judy Ojewia, Natalie Hicks </authors>
//<date> Fall 2024 </date>

using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    /// <summary>
    /// This class represents a Wall object that has a unique id (wall),
    /// and 2 Point2D properties which define the two endpoints of the Wall.
    /// </summary>
    public class Wall
    {
        /// <summary>
        /// An int representing this wall's unique ID.
        /// </summary>
        [JsonInclude]
        public int wall
        { get; private set; }

        /// <summary>
        /// A Point2D representing one endpoint of this wall.
        /// </summary>
        [JsonInclude]
        public Point2D p1
        { get; private set; }


        /// <summary>
        /// A Point2D representing the other endpoint of this wall.
        /// </summary>
        [JsonInclude]
        public Point2D p2
        { get; private set; }

        /// <summary>
        /// Default constructor for a Wall.
        /// </summary>
        public Wall()
        {
            wall = 0;
            p1 = new Point2D();
            p2 = new Point2D();
        }

        /// <summary>
        /// Constructor for a Wall that takes in an int
        /// whose value is assigned as the ID or wall property of
        /// this wall, and then 2 Point2D objects which represent the
        /// start point and endpoint of this wall.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public Wall(int wall, Point2D p1, Point2D p2)
        {
            this.wall = wall;
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
