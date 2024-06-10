using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using SnakeGame;
using StbImageSharp;
using System.Transactions;
using SnakeGame.Drawing;
using System.Web;
using static OpenTK.Graphics.OpenGL.GL;
using System.Linq;
using Microsoft.VisualBasic.Devices;

namespace SnakeGame
{
    public class C
    {
        public const int UP = 11;
        public const int RIGHT = 12;
        public const int DOWN = 13;
        public const int LEFT = 14;

        public const int NO = 0;
    }
    
    internal class GameLogic
    {
        public bool IsWaitingForInput;
        public List<(int, int)> tailCoords;
        public List<int> dirs;
        public (int, int) HeadCoord;
        public (int, int) FoodCoord;
        public int HeadDir;
        public (int, int) lastPos;

        public GameLogic() {
        this.IsWaitingForInput = true;
        this.tailCoords = new List<(int, int)>();
            this.dirs= new List<int>();
            this.HeadCoord = (4, 5);
            this.HeadDir = C.UP;
    }

        public void SetGame()
        {
            HeadCoord = (4, 5);
            tailCoords.Add((4, 4));
            dirs.Add(C.UP);
        }
        public void SpawnFood() {
            while (true)
            {
                var rand = new Random();
                int randI = rand.Next(10), randJ = rand.Next(10);
                int tmp = 1;
                if (HeadCoord != (randI, randJ))
                {
                    for (int i = 0;i < tailCoords.Count; i++)
                    {
                        if (tailCoords[i] == (randI, randJ))
                            tmp = 0;

                    }
                    if (tmp == 1)
                    {
                        FoodCoord = (randI, randJ);
                        break;
                    }
                }
                
            }
        }

        public bool CollidedWithFood()
        {
            IsWaitingForInput = true;
            return (HeadCoord == FoodCoord);

        }
        public bool CollidedWithTail()
        {
            for (int i = 0; i < tailCoords.Count; i++)
            {
                if(HeadCoord == tailCoords[i])
                    return true;
            }
            return false;
        }

    }
}
