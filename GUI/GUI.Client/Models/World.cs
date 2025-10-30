// UofU-CS3500 
//<authors> Judy Ojewia, Natalie Hicks </authors>
//<date> Fall 2024 </date>


namespace GUI.Client.Models; 

/// <summary>
/// This class represents a World object which can hold Snakes, Powerups,
/// and Walls. A world has specific dimension or size, and also has the ability 
/// to have Snakes, Powerups, and Walls added or removed from it.
/// </summary>
public class World
{
    /// <summary>
    /// All of the Snakes in this World.
    /// </summary>
    public Dictionary<int, Snake> Snakes
    { get; private set; }

    /// <summary>
    /// All of the Powerups in this World.
    /// </summary>
    public Dictionary<int, Powerup> Powerups
    { get; private set; }


    /// <summary>
    /// All of the Walls in this World.
    /// </summary>
    public Dictionary<int, Wall> Walls
    { get; private set; }


    /// <summary>
    /// The dimension of a single side of the square world.
    /// </summary>
    public int Dimension
    { get; private set; }

    /// <summary>
    /// Creates a new world with the given dimension.
    /// </summary>
    /// <param name="dimension"></param>
    public World(int dimension)
    {
        Snakes = new Dictionary<int, Snake>();
        Powerups = new Dictionary<int, Powerup>();
        Walls = new Dictionary<int, Wall>();
        Dimension = dimension;
    }

    /// <summary>
    /// Shallow copy constructor.
    /// </summary>
    /// <param name="world"></param>
    public World(World world)
    {
        Snakes = new(world.Snakes);
        Powerups = new(world.Powerups);
        Walls = new(world.Walls);
        Dimension = world.Dimension;
    }


    /// <summary>
    ///  Adds an object to the World. Can add a Snake, Powerup or a Wall. 
    /// </summary>
    /// <param name="obj"></param>
    public void AddObject(object obj)
    {
        if (obj is Snake)
        {
            Snake s = (Snake)obj;
            if (Snakes.ContainsKey(s.snake))
            {
                Snakes.Remove(s.snake);
            }
            Snakes.Add(s.snake, s);
        }
        else if (obj is Powerup)
        {
            Powerup p = (Powerup)obj;
            if (Powerups.ContainsKey(p.power))
            {
                Powerups.Remove(p.power);
            }
            Powerups.Add(p.power, p);
        }
        else if (obj is Wall)
        {
            Wall w = (Wall)obj;
            if (Walls.ContainsKey(w.wall))
            {
                Walls.Remove(w.wall);
            }
            Walls.Add(w.wall, w);
        }

    }

    /// <summary>
    /// Removes an object to the World. Can remove a Snake, Powerup or a Wall. 
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveObject(object obj)
    {
        if (obj is Snake)
        {
            Snake s = (Snake)obj;
            Snakes.Remove(s.snake);
        }
        else if (obj is Powerup)
        {
            Powerup p = (Powerup)obj;
            Powerups.Remove(p.power);
        }
        else if (obj is Wall)
        {
            Wall w = (Wall)obj;
            Walls.Remove(w.wall);
        }

    }

}