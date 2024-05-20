using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PP_console_snake
{
    public class SnakeGame
    {
        private const int WindowHeight = 16;
        private const int WindowWidth = 32;
        private const int InitialScore = 5;
        private const int GameSpeed = 500; // milliseconds

        private int score;
        private bool isGameOver;
        private SnakeSegment head;
        private string currentDirection;
        private List<SnakeSegment> body;
        private SnakeSegment berry;
        private Random random;

        public SnakeGame()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;
            score = InitialScore;
            isGameOver = false;
            head = new SnakeSegment(WindowWidth / 2, WindowHeight / 2, ConsoleColor.Red);
            currentDirection = "RIGHT";
            body = new List<SnakeSegment>();
            random = new Random();
            berry = new SnakeSegment(random.Next(1, WindowWidth - 2), random.Next(1, WindowHeight - 2), ConsoleColor.Cyan);
        }

        public void Run()
        {
            while (!isGameOver)
            {
                Render();
                HandleInput();
                UpdateGame();
                Thread.Sleep(GameSpeed);
            }
            DisplayGameOver();
        }

        private void Render()
        {
            Console.Clear();
            DrawBorders();
            DrawBerry();
            DrawSnake();
        }

        private void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < WindowWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, WindowHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(WindowWidth - 1, i);
                Console.Write("■");
            }
        }

        private void DrawBerry()
        {
            Console.SetCursorPosition(berry.X, berry.Y);
            Console.ForegroundColor = berry.Color;
            Console.Write("■");
        }

        private void DrawSnake()
        {
            Console.SetCursorPosition(head.X, head.Y);
            Console.ForegroundColor = head.Color;
            Console.Write("■");

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var segment in body)
            {
                Console.SetCursorPosition(segment.X, segment.Y);
                Console.Write("■");
            }
        }

        private void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                ChangeDirection(key);
            }
        }

        private void ChangeDirection(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (currentDirection != "DOWN") currentDirection = "UP";
                    break;
                case ConsoleKey.DownArrow:
                    if (currentDirection != "UP") currentDirection = "DOWN";
                    break;
                case ConsoleKey.LeftArrow:
                    if (currentDirection != "RIGHT") currentDirection = "LEFT";
                    break;
                case ConsoleKey.RightArrow:
                    if (currentDirection != "LEFT") currentDirection = "RIGHT";
                    break;
            }
        }

        private void UpdateGame()
        {
            MoveSnake();
            CheckCollision();
            CheckBerry();
        }

        private void MoveSnake()
        {
            body.Add(new SnakeSegment(head.X, head.Y, ConsoleColor.Green));

            switch (currentDirection)
            {
                case "UP":
                    head.Y--;
                    break;
                case "DOWN":
                    head.Y++;
                    break;
                case "LEFT":
                    head.X--;
                    break;
                case "RIGHT":
                    head.X++;
                    break;
            }

            if (body.Count > score)
            {
                body.RemoveAt(0);
            }
        }

        private void CheckCollision()
        {
            if (head.X == 0 || head.X == WindowWidth - 1 ||
                head.Y == 0 || head.Y == WindowHeight - 1 ||
                body.Any(segment => segment.X == head.X && segment.Y == head.Y))
            {
                isGameOver = true;
            }
        }

        private void CheckBerry()
        {
            if (head.X == berry.X && head.Y == berry.Y)
            {
                score++;
                berry.X = random.Next(1, WindowWidth - 2);
                berry.Y = random.Next(1, WindowHeight - 2);
            }
        }

        private void DisplayGameOver()
        {
            Console.SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
        }
    }
}
