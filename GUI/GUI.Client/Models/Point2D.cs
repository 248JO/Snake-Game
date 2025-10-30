// UofU-CS3500 
//<authors> Judy Ojewia, Natalie Hicks </authors>
//<date> Fall 2024 </date>

using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// This class represents a pair of 2 integers, or a 2D point.
/// </summary>
public class Point2D
{
    /// <summary>
    /// The X coordinate of this point.
    /// </summary>
    [JsonInclude]
    public int X
    { get; private set; }

    /// <summary>
    /// The Y coordinate of this point.
    /// </summary>
    [JsonInclude]
    public int Y
    { get; private set; }

    /// <summary>
    /// Constructor for a Point2D, sets the 
    /// X and Y coordinates of this point to the 
    /// entered parameters.
    /// </summary>
    public Point2D(int _x, int _y)
    {
        this.X = _x;
        this.Y = _y;
    }

    /// <summary>
    /// Default constructor for a Point2D.
    /// </summary>
    public Point2D()
    {
        X = 0;
        Y = 0;
    }
}