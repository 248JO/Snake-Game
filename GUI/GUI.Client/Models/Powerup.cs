// UofU-CS3500 
//<authors> Judy Ojewia, Natalie Hicks </authors>
//<date> Fall 2024 </date>

using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// This class represents the Powerup object which has a 
/// location, a ID, and can die.
/// </summary>
public class Powerup
{
    /// <summary>
    /// An int representing the powerup's unique ID.
    /// </summary>
    [JsonInclude]
    public int power
    { get; private set; }

    /// <summary>
    ///  A Point2D representing the location of the powerup.
    /// </summary>
    [JsonInclude]
    public Point2D loc
    { get; private set; }

    /// <summary>
    /// a bool indicating if the powerup "died" (was collected by a player) on this frame. 
    /// </summary>
    [JsonInclude]
    public bool died
    { get; private set; }

    /// <summary>
    /// Default constructor for a Powerup.
    /// </summary>
    public Powerup()
    {
        power = 0;
        loc = new Point2D();
        died = false;
    }

    /// <summary>
    /// Constructor for a Powerup object. Assigns
    /// the power, loc, and died status of the Powerup
    /// to the entered parameters.
    /// </summary>
    /// <param name="power"></param>
    /// <param name="loc"></param>
    /// <param name="died"></param>
    public Powerup(int power, Point2D loc, bool died)
    {
        this.power = power;
        this.loc = loc;
        this.died = died;
    }
}
