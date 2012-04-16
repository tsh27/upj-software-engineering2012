using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UPJTowerDefense
{
    public class Level
    {
        // The different tiles for the map
        private List<Texture2D> tileTextures = new List<Texture2D>();

        // The list of waypoint queues
        private List<Queue<Vector2>> waypointsList = new List<Queue<Vector2>>();

        // Used for storing text read from map files
        private string mapText;
        private string[] mapParsedString;

        // Logical storage for the map
        int[,] map = new int[13, 20];
        
        /// <summary>
        /// Reads correct map file based on current world
        /// </summary>
        private void ReadMapFile()
        {
            char[] delimiters = { ' ', '\r', '\n' };
            mapText = File.ReadAllText(@"Map Files\Level" + Options.worldNumber + @"Map.txt");
            mapParsedString =  mapText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Loads map array with the values
        /// </summary>
        private void LoadMap()
        {
            int i = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (int.Parse(mapParsedString[i]) != -1)
                    {
                        map[y, x] = int.Parse(mapParsedString[i]);
                    }
                    i++;
                }
            }           
        }
        
        // Reads two sets of waypoints into the waypointList
        private void ReadWaypointsFile()
        {
            char[] delimiters = { ' ', '\r', '\n' };

            string waypointsText;
            string[] waypointsParsedString;

            for (int i = 1; i <= 2; i++)
            {
                waypointsText = string.Empty;
                waypointsParsedString = null;
                waypointsText = File.ReadAllText(@"Map Files\Level" + Options.worldNumber + @"Waypoints" +  i + ".txt");
                waypointsParsedString = waypointsText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                waypointsList.Add(LoadWaypoints(waypointsParsedString));
            }
        }

        /// <summary>
        /// Loads each waypoint queue with the values from the correct waypoint file
        /// </summary>
        /// <param name="waypointsParsedString">The parsed values from the waypoint file</param>
        /// <returns></returns>
        private Queue<Vector2> LoadWaypoints(string[] waypointsParsedString)
        {
            Queue<Vector2> waypoints = new Queue<Vector2>();
            for (int n = 0; n < waypointsParsedString.Length; n += 2)
            {
                if (int.Parse(waypointsParsedString[n]) != -1)
                {
                    int x = int.Parse(waypointsParsedString[n]);
                    int y = int.Parse(waypointsParsedString[n + 1]);

                    waypoints.Enqueue(new Vector2(x, y) * Util.tileSize);
                }
            }

            return waypoints;
        }

        /// <summary>
        /// Returns the waypointList
        /// </summary>
        public List<Queue<Vector2>> Waypoints
        {
            get { return waypointsList; }
        }

        public int Width
        {
            get { return map.GetLength(1); }
        }

        public int Height
        {
            get { return map.GetLength(0); }
        }

        /// <summary>
        /// Constructs a Level
        /// </summary>
        public Level()
        {
            ReadMapFile();
            LoadMap();
            ReadWaypointsFile();
        }  

        /// <summary>
        /// Get index of the current cell
        /// </summary>
        /// <param name="cellX"></param>
        /// <param name="cellY"></param>
        /// <returns>Returns the cell value</returns>
        public int GetIndex(int cellX, int cellY)
        {
            // It needed to be Width - 1 and Height - 1.
            if (cellX < 0 || cellX > Width - 1 || cellY < 0 || cellY > Height - 1)
                return 0;

            return map[cellY, cellX];
        }

        /// <summary>
        /// Adds map tiles to the tileTextures list
        /// </summary>
        /// <param name="tileTextureToAdd"></param>
        public void AddTileTexture(Texture2D tileTextureToAdd)
        {
            tileTextures.Add(tileTextureToAdd);
        }

        /// <summary>
        /// Draws the map
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];

                    if (textureIndex == -1)
                    {
                        continue;
                    }

                    Texture2D texture = tileTextures[textureIndex];

                    // Draw the correct tile (normal, path, base)
                    if (textureIndex == 0)
                    {
                        spriteBatch.Draw(texture, new Rectangle(x * Util.tileSize, y * Util.tileSize, Util.tileSize, Util.tileSize), Color.White);
                    }
                    else if (textureIndex == 1)
                    {
                        spriteBatch.Draw(texture, new Rectangle(x * Util.tileSize, y * Util.tileSize, Util.tileSize, Util.tileSize), Color.White);
                    }
                    else if (textureIndex == 2)
                    {
                        spriteBatch.Draw(texture, new Rectangle(x * Util.tileSize, y * Util.tileSize, Util.tileSize, Util.tileSize), Color.White);
                    }
                }
            }

            // Draw a spiderweb at each entry point
            foreach (Queue<Vector2> waypoints in waypointsList)
            {
                Vector2 entryPoint = waypoints.Peek();
                spriteBatch.Draw(tileTextures[3], new Rectangle((int)entryPoint.X, (int)entryPoint.Y, Util.tileSize, Util.tileSize), Color.White);
            }
        }
    }
}