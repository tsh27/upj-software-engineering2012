using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UPJTowerDefense
{
    public class Level
    {
        private List<Texture2D> tileTextures = new List<Texture2D>();
        private Queue<Vector2> waypoints = new Queue<Vector2>();

        const int tileSize = 48;
        const int tileWidth = tileSize;
        const int tileHeight = tileSize;

        int[,] map = new int[,]
        {
            {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,2},
        };

        public Queue<Vector2> Waypoints
        {
            get { return waypoints; }
        }

        public int Width
        {
            get { return map.GetLength(1); }
        }
        public int Height
        {
            get { return map.GetLength(0); }
        }

        public Level()
        {
            waypoints.Enqueue(new Vector2(0, 0) * tileSize);
            waypoints.Enqueue(new Vector2(2, 0) * tileSize);
            waypoints.Enqueue(new Vector2(2, 1) * tileSize);
            waypoints.Enqueue(new Vector2(3, 1) * tileSize);
            waypoints.Enqueue(new Vector2(3, 2) * tileSize);
            waypoints.Enqueue(new Vector2(4, 2) * tileSize);
            waypoints.Enqueue(new Vector2(4, 4) * tileSize);
            waypoints.Enqueue(new Vector2(3, 4) * tileSize);
            waypoints.Enqueue(new Vector2(3, 5) * tileSize);
            waypoints.Enqueue(new Vector2(2, 5) * tileSize);
            waypoints.Enqueue(new Vector2(2, 7) * tileSize);
            waypoints.Enqueue(new Vector2(7, 7) * tileSize);
            waypoints.Enqueue(new Vector2(7, 12) * tileSize);
            waypoints.Enqueue(new Vector2(19, 12) * tileSize);
        }

        public int GetIndex(int cellX, int cellY)
        {
            // It needed to be Width - 1 and Height - 1.
            if (cellX < 0 || cellX > Width - 1 || cellY < 0 || cellY > Height - 1)
                return 0;

            return map[cellY, cellX];
        }

        public void AddTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        public void Draw(SpriteBatch batch, List<Vector2> clickedTowerRadius)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];

                    if (textureIndex == -1)
                        continue;

                    Texture2D texture = tileTextures[textureIndex];
                    Vector2 mapCoordinate = new Vector2(x*tileSize, y*tileSize);
                    Vector2 currentRadiusCoordinate = new Vector2(-1, -1);

                    foreach (Vector2 radiusPoint in clickedTowerRadius)
                    {
                        if (radiusPoint.Equals(mapCoordinate))
                        {
                            currentRadiusCoordinate = radiusPoint;
                        }
                    }

                    //Console.WriteLine(textureIndex);

                    if (currentRadiusCoordinate.Equals(mapCoordinate))
                    {
                        if (textureIndex == 0)
                        {
                            batch.Draw(texture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), Color.LightBlue);
                        }
                        else if (textureIndex == 1)
                        {
                            batch.Draw(texture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), Color.Goldenrod);
                        }
                        else if (textureIndex == 2)
                        {
                            batch.Draw(texture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), Color.White);
                        }
                    }
                    else
                    {
                        if (textureIndex == 0)
                        {
                            batch.Draw(texture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), Color.CornflowerBlue);
                        }
                        else if (textureIndex == 1)
                        {
                            batch.Draw(texture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), Color.Yellow);
                        }
                        else if (textureIndex == 2)
                        {
                            batch.Draw(texture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), Color.White);
                        }
                    }
                    //clickedTowerRadius.Remove(currentRadiusCoordinate);
                }
            }
        }
    }
}
