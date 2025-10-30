// UofU-CS3500 
//<authors> Judy Ojewia, Natalie Hicks </authors>
//<date> Fall 2024 </date>

using System.Text.Json;
using GUI.Client.Models;
using MySql.Data.MySqlClient;

namespace GUI.Client.Controllers
{
    /// <summary>
    /// This class represents a NetworkController object 
    /// which sets up a connection to a specified server. 
    /// NetworkController contains methods for the functionality 
    /// needed between the client and the server using the logic
    /// of the Snake game.
    /// </summary>
    public class NetworkController
    {
        private NetworkConnection network;
        private int gameID;

        /// <summary>
        /// <summary>
        /// The connection string to connect to the Snake database.
        /// </summary>
        /// </summary>
        private const string connectionString = "server=atr.eng.utah.edu;" +
      "database=u6026126;" +
      "uid=u6026126;" +
      "password=3500PS10";

        /// <summary>
        /// Sets up a network connection to a server 
        /// with the specified port and address.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="address"></param>
        public NetworkController(int port, string address)
        {
            network = new NetworkConnection();
            network.Connect(address, port);
        }

        /// <summary>
        /// Disconnects this network.
        /// from the server.
        /// Updates the end time of the game and the leave times
        /// of all snakes who were connected in the game.
        /// </summary>
        public void NetworkDisconnect(World gameWorld)
        {
            //update leave time for game in table
            UpdateGameEndTime(gameID);
            World gameWorldCopy;
            lock (gameWorld) {
                gameWorldCopy = new(gameWorld);
            }
            foreach (Snake s in gameWorldCopy.Snakes.Values)
            {
                UpdateSnakeLeaveTime(s, gameID);
            }

            network.Disconnect();
        }

        /// <summary>
        /// This method runs the connection between the snake client
        /// and the server. It starts by sending the name of the snake
        /// client user to the server, then gets the ID of the client
        /// from the server and the dimension of the World that the 
        /// snake game is running in. Then for as long as the method is 
        /// ran, strings sent from the server are read and then passed into 
        /// HandleData to be added to the World.
        /// </summary>
        /// <param name="PlayerName"></param>
        /// <param name="gameWorld"></param>
        /// <param name="id"></param>
        public void HandleNetwork(string PlayerName, ref World gameWorld, ref int id)
        {
            network.Send(PlayerName);

            string ID = string.Empty;
            string dimension = string.Empty;
            lock (network)
            {
                ID = network.ReadLine();
                dimension = network.ReadLine();
            }

            gameWorld = new World((int)Double.Parse(dimension));
            id = (int)Double.Parse(ID);

            //Adds row to game data and stores gameID of this game as member var
            AddRowToGame();


            //While network is running, world is updated by reading the objects sent from the server
            while (network.IsConnected)
            {
                string jsonText = String.Empty;
                try
                {
                    lock (network)
                    {
                        jsonText = network.ReadLine();
                    }
                }
                catch (Exception)
                {
                    break;
                }
                lock (gameWorld)
                {
                    HandleData(jsonText, gameWorld);
                }
            }
        }


        /// <summary>
        /// Converts the string Json to an object and if the
        /// object is a Snake, Powerup, or Wall it adds the object
        /// to the World. If the snake trying to be added has disconnected,
        /// the snake is removed from the world. If the powerup has died,
        /// the powerup is removed from the world.
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="gameWorld"></param>
        public void HandleData(string Json, World gameWorld)
        {
            lock (gameWorld)
            {
                if (Json.Contains("snake"))
                {
                    Snake? snake = JsonSerializer.Deserialize<Snake>(Json) as Snake;
                    if (snake != null)
                    {
                        int ID = snake.snake;
                        if (gameWorld.Snakes.ContainsKey(ID))
                        {
                            int score = gameWorld.Snakes[ID].maxScore;  
                            if (snake.score > score)
                            {
                                snake.maxScore = snake.score;
                                UpdatePlayerScore(snake, gameID);
                              
                            }
                            else
                            {
                                snake.maxScore = score;
                            }

                            gameWorld.AddObject(snake);
                        }
                        else
                        {
                            //add row to players table
                            gameWorld.AddObject(snake);
                            AddRowToPlayers(snake, gameID);
                            
                        }
                        if (snake.dc)
                        {
                            //update leave time in table
                            UpdateSnakeLeaveTime(snake, gameID);
                            
                            gameWorld.RemoveObject(snake);
                        }
                    }

                }
                else if (Json.Contains("power"))
                {
                    Powerup? powerup = JsonSerializer.Deserialize<Powerup>(Json) as Powerup;
                    if (powerup != null)
                    {
                        int ID = powerup.power;

                        if (gameWorld.Powerups.ContainsKey(ID) && !powerup.died)
                        {
                            gameWorld.AddObject(powerup);
                        }
                        else if (!powerup.died)
                        {
                            gameWorld.AddObject(powerup);
                        }
                        else
                        {
                            gameWorld.RemoveObject(powerup);
                        }
                    }
                }
                else if (Json.Contains("wall"))
                {
                    Wall? wall = JsonSerializer.Deserialize<Wall>(Json) as Wall;
                    if (wall != null)
                    {
                        int ID = wall.wall;
                        gameWorld.AddObject(wall);
                    }
                }
            }
        }

        /// <summary>
        /// Sends command request to the server based on
        /// if the key "W", "A", "S" or "D" was pressed. If 
        /// the key press was not any of these options then 
        /// no command is sent.
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void KeyPress(string key)
        {
            if (key == "A")
            {
                key = "left";
            }
            else if (key == "S")
            {
                key = "down";
            }
            else if (key == "D")
            {
                key = "right";
            }
            else if (key == "W")
            {
                key = "up";
            }
            else
            {
                key = "none";
                return;
            }
            //Build correct command, send it to sever
            string command = "{\"moving\":\"" + key + "\"}";
            network.Send(command);
        }

        /// <summary>
        /// Adds a row to the game which has this game's id as it's ID and
        /// the current time as the start time.
        /// </summary>
        public void AddRowToGame()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {

                try
                {
                    //Open a connection
                    conn.Open();

                    //Create a command
                    MySqlCommand command = conn.CreateCommand();

                    //add row to game
                    command.CommandText = "insert into Games (StartTime) values(\'" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "\')";
                    command.ExecuteNonQuery();

                    //Get the ID of the most recently added game and store it as member variable
                    command.CommandText = "select last_insert_id()";
                    gameID = (int)Double.Parse(command.ExecuteScalar().ToString());

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// This method updates the score of a player with the player's max score
        /// </summary>
        /// <param name="snake"></param> a snake to have score updated
        /// <param name="game"></param> the players corresponding game id
        public static void UpdatePlayerScore(Snake snake, int game)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    //Open a connection
                    conn.Open();

                    //Create a command
                    MySqlCommand command = conn.CreateCommand();

                    //add row to game
                    command.CommandText = $"update Players set MaxScore = {snake.maxScore} where PlayerID = {snake.snake} and GameID = {game}";

                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// This method adds a row to players when a snake is introduced to the game, 
        /// the player is not given a leave time because at this point they player is still in the game
        /// </summary>
        /// <param name="snake"></param> a snake representing the player being added to 
        /// <param name="game"></param> the game id of the player
        public static void AddRowToPlayers(Snake snake, int game)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    //Open a connection
                    conn.Open();

                    //Create a command
                    MySqlCommand command = conn.CreateCommand();

                    //add row to game
                    command.CommandText = $"insert into Players (PlayerID, PlayerName, MaxScore, EnterTime, GameID) values({snake.snake}, \"{snake.name}\", 0, '{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}', {game})";
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// This method updates the game end time once the game is disconnected aka once the game ends
        /// </summary>
        /// <param name="game"></param> the id of the ending game
        public static void UpdateGameEndTime(int game)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    //Open a connection
                    conn.Open();

                    //Create a command
                    MySqlCommand command = conn.CreateCommand();

                    //add row to game
                    command.CommandText = $"update Games set EndTime = '{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}' where ID = {game}";
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

       /// <summary>
       /// This method updates the leave time of the snake given a snake and its corresponding games
       /// </summary>
       /// <param name="snake"></param> a snake representing the snake that just left the game
       /// <param name="game"></param> an integer representing the corresponding game id
        public static void UpdateSnakeLeaveTime(Snake snake, int game)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    //Open a connection
                    conn.Open();

                    //Create a command
                    MySqlCommand command = conn.CreateCommand();

                    //add row to game
                    command.CommandText = $"update Players set LeaveTime = '{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}' where GameID = {game} and PlayerID = {snake.snake}"; //Correct syntax?
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

    }
}
