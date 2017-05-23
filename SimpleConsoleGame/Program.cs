using System;
using System.Threading;

namespace ConsoleApplication9
{

    public class Ball
    {
        private static int Xpos = 1;
        private static int YPos = 1;

        public static readonly char BallChar = 'O';


        private static void Move(ConsoleKeyInfo key)
        {
            if (key.Key.ToString() == "UpArrow" && YPos > 1)
            {
                YPos--;
            }
            if (key.Key.ToString() == "DownArrow" && YPos < Field.MaxHeight - 2)
            {
                YPos++;
            }
            if (key.Key.ToString() == "LeftArrow" && Xpos > 1)
            {
                Xpos--;
            }
            if (key.Key.ToString() == "RightArrow" && Xpos < Field.MaxWidth - 2)
            {
                Xpos++;
            }
        }

        public static void ReadKeys()
        {
            while (true)
            {
                var key = Console.ReadKey();
                Move(key);
                Thread.Sleep(0);
            }
        }

        public static Frame PrepareDataToFrame()
        {
            var result = new Frame();
            result.Data[Xpos, YPos] = BallChar;
            return result;
        }

    }

    public class Frame
    {
        public static readonly char EmptyChar = '\0';

        public char[,] Data;

        public Frame()
        {
            Data = new char[Field.MaxWidth, Field.MaxHeight];
        }

        public static Frame operator +(Frame left, Frame right)
        {
            var maxWidth = left.Data.GetLength(0);
            var maxHeight = left.Data.GetLength(1);

            var result = new Frame();
            for (var i = 0; i < maxWidth; i++)
            {
                for (var j = 0; j < maxHeight; j++)
                {

                    if (right.Data[i, j] != EmptyChar)
                    {
                        result.Data[i, j] = right.Data[i, j];
                    }
                    else
                    {
                        result.Data[i, j] = left.Data[i, j];
                    }
                }

            }
            return result;
        }
    }

    public class Field : Frame
    {
        public static readonly char FieldChar = '+';
        public static readonly int MaxWidth = 60;
        public static readonly int MaxHeight = 23;

        public static Frame PrepareDataToFrame()
        {
            var result = new Frame();
            for (var i = 0; i < MaxWidth; i++)
            {
                for (var j = 0; j < MaxHeight; j++)
                {

                    if (i == 0 || i == MaxWidth - 1 || (j == 0 || j == MaxHeight - 1))
                    {
                        result.Data[i, j] = FieldChar;
                    }
                    else
                    {
                        result.Data[i, j] = EmptyChar;
                    }

                }
            }
            return result;
        }

        /*public static void Draw()
        {
            
            for (var i = 0; i <= MaxHeight; i++)
            {
                for (var j = 0; j <= MaxWidth; j++)
                {
                    Console.SetCursorPosition(j, i);
                    if (i == 0 || i == MaxHeight || (j == 0 || j == MaxWidth))
                    {
                        Console.Write(FieldChar);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }
        }*/
    }

    public static class Draw
    {
        private static readonly int FPS = 20;

        private static int SleepTime
        {
            get { return 1000 / FPS; }
        }

        public static void GetFrame()
        {
            var frame = Field.PrepareDataToFrame() + Ball.PrepareDataToFrame();

            for (var i = 0; i < frame.Data.GetLength(0); i++)
            {
                for (var j = 0; j < frame.Data.GetLength(1); j++)
                {
                    if (frame.Data[i, j] != Frame.EmptyChar)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.WriteLine(frame.Data[i, j]);
                    }
                }
            }
        }

        public static void Start()
        {
            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();
                GetFrame();
                Thread.Sleep(SleepTime);
            }

        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            var moveBallReadKey = new Thread(Ball.ReadKeys);
            var drawBall = new Thread(Draw.Start);

            moveBallReadKey.Start();
            drawBall.Start();

        }
    }
}

