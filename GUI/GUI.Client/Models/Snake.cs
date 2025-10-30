// UofU-CS3500 
//<authors> Judy Ojewia, Natalie Hicks </authors>
//<date> Fall 2024 </date>

using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// This class represents the Snake object that is composed of 
/// a list of Point2Ds representing the line segments of this Snake.
/// The snake has various properties that define its condition.
/// </summary>
public class Snake
{
    /// <summary>
    /// an int representing the snake's unique ID.  
    /// </summary>
    [JsonInclude]
    public int snake
    { get; private set; }


    /// <summary>
    /// a string representing the player's name.
    /// </summary>
    [JsonInclude]
    public string name
    { get; private set; }

    /// <summary>
    /// a List<Point2D> representing the entire body of the snake. 
    /// Each point in this list represents one vertex of the snake's body, 
    /// where two consecutive vertices make up one straight segment of the body. 
    /// The first point of the list gives the location of the snake's tail, 
    /// and the last gives the location of the snake's head. 
    /// </summary>
    [JsonInclude]
    public List<Point2D> body
    { get; private set; }


    /// <summary>
    /// an Point2D representing the snake's orientation. This will always be 
    /// an axis-aligned vector (purely horizontal or vertical). 
    /// </summary>
    [JsonInclude]
    private Point2D direction;

    /// <summary>
    /// an int representing the player's score (the number of powerups it has 
    /// eaten).
    /// </summary>
    [JsonInclude]
    public int score
    { get; private set; }

    /// <summary>
    ///  a bool indicating if the snake died on this frame.
    /// </summary>
    [JsonInclude]
    public bool died
    { get; private set; }


    /// <summary>
    ///  a bool indicating whether a snake is alive or dead.
    /// </summary>
    [JsonInclude]
    public bool alive
    { get; private set; }

    /// <summary>
    /// a bool indicating if the player controlling that snake 
    /// disconnected on that frame.
    /// </summary>
    [JsonInclude]
    public bool dc
    { get; private set; }

    /// <summary>
    /// an int indicating the max score of the player
    /// </summary>
    [JsonIgnore]
    public int maxScore
    { get;  set; }

    /// <summary>
    /// a bool indicating if the player joined on this frame.
    /// </summary>
    [JsonInclude]
    private bool join;

    /// <summary>
    /// Default constructor for a snake object
    /// </summary>
    public Snake()
    {
        snake = 0;
        name = string.Empty;
        body = new List<Point2D>();
        direction = new Point2D();
        score = 0;
        died = false;
        alive = true;
        dc = false;
        join = true;
        maxScore = 0;
    }

    /// <summary>
    /// Constructor for a snake object
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="body"></param>
    /// <param name="direction"></param>
    /// <param name="score"></param>
    /// <param name="died"></param>
    /// <param name="alive"></param>
    /// <param name="dc"></param>
    /// <param name="join"></param>
    public Snake(int id, string name, List<Point2D> body, Point2D direction, int score, bool died, bool alive, bool dc, bool join)
    {
        this.snake = id;
        this.name = name;
        this.body = body;
        this.direction = direction;
        this.score = score;
        this.died = died;
        this.alive = alive;
        this.dc = dc;
        this.join = join;
    }



}