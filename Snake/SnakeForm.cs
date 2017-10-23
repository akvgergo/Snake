using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Snake {

    public class SnakeForm : Form {

        Size Resolution = default(Size);
        int PixelSize = 0;

        Timer GameLoop = new Timer();

        Queue<Point> SnakeSegments = new Queue<Point>(100);
        Queue<Keys> InputQueue = new Queue<Keys>(5);
        Point Food = default(Point);
        Direction SnakeHeadDirection = Direction.up;

        Point Head {
            get { return SnakeSegments.Last(); }
        }

        public SnakeForm() {
            var bounds = Screen.PrimaryScreen.Bounds;
            Bounds = bounds;
            BackColor = Color.Black;
            FormBorderStyle = FormBorderStyle.None;
            Cursor.Hide();

            for (int i = 40; i < 400; i++) {
                if (bounds.Width % i == 0 && bounds.Height % i == 0) {
                    PixelSize = i;
                    Resolution = new Size(bounds.Width / PixelSize, bounds.Height / PixelSize);
                    break;
                }
            }

            GameLoop.Interval = 300;
            GameLoop.Tick += OnGameTick;
            KeyDown += HandleInput;

            Point start = new Point(Resolution.Width / 2, Resolution.Height / 2);
            SnakeSegments.Enqueue(start);
            SnakeSegments.Enqueue(new Point(start.X, start.Y - 1));
            SnakeSegments.Enqueue(new Point(start.X, start.Y - 2));
            PlaceFood();

            GameLoop.Start();
        }

        private void HandleInput(object sender, KeyEventArgs e) {
            InputQueue.Enqueue(e.KeyCode);
        }

        private void OnGameTick(object sender, EventArgs e) {
            var OriginalDirection = SnakeHeadDirection;

            while (InputQueue.Count > 0) {
                var key = InputQueue.Dequeue();
                switch (key) {
                    case Keys.Left:
                        if (OriginalDirection != Direction.right)
                            SnakeHeadDirection = Direction.left;
                        break;
                    case Keys.Up:
                        if (OriginalDirection != Direction.down)
                            SnakeHeadDirection = Direction.up;
                        break;
                    case Keys.Right:
                        if (OriginalDirection != Direction.left)
                            SnakeHeadDirection = Direction.right;
                        break;
                    case Keys.Down:
                        if (OriginalDirection != Direction.up)
                            SnakeHeadDirection = Direction.down;
                        break;
                    case Keys.A:
                        if (OriginalDirection != Direction.right)
                            SnakeHeadDirection = Direction.left;
                        break;
                    case Keys.D:
                        if (OriginalDirection != Direction.left)
                            SnakeHeadDirection = Direction.right;
                        break;
                    case Keys.S:
                        if (OriginalDirection != Direction.up)
                            SnakeHeadDirection = Direction.down;
                        break;
                    case Keys.W:
                        if (OriginalDirection != Direction.down)
                            SnakeHeadDirection = Direction.up;
                        break;
                    default:
                        break;
                }
            }

            Point newTile;
            switch (SnakeHeadDirection) {

                case Direction.up:
                    newTile = new Point(Head.X, Head.Y - 1);
                    if (SnakeSegments.Contains(newTile)) {
                        GameLoop.Stop();
                        break;
                    }
                    if (newTile.Y < 0) {
                        GameLoop.Stop();
                    } else {
                        SnakeSegments.Enqueue(newTile);
                        if (Food == newTile) {
                            PlaceFood();
                            if (GameLoop.Interval > 60) {
                                GameLoop.Interval -= 10;
                            }
                        } else {
                            SnakeSegments.Dequeue();
                        }
                    }

                    break;
                case Direction.down:
                    newTile = new Point(Head.X, Head.Y + 1);
                    if (SnakeSegments.Contains(newTile)) {
                        GameLoop.Stop();
                        break;
                    }
                    if (newTile.Y >= Resolution.Height) {
                        GameLoop.Stop();
                    } else {
                        SnakeSegments.Enqueue(newTile);
                        if (Food == newTile) {
                            PlaceFood();
                            if (GameLoop.Interval > 100) {
                                GameLoop.Interval -= 10;
                            }
                        } else {
                            SnakeSegments.Dequeue();
                        }
                    }

                    break;
                case Direction.left:
                    newTile = new Point(Head.X - 1, Head.Y);
                    if (SnakeSegments.Contains(newTile)) {
                        GameLoop.Stop();
                        break;
                    }
                    if (newTile.X < 0) {
                        GameLoop.Stop();
                    } else {
                        SnakeSegments.Enqueue(newTile);
                        if (Food == newTile) {
                            PlaceFood();
                            if (GameLoop.Interval > 100) {
                                GameLoop.Interval -= 10;
                            }
                        } else {
                            SnakeSegments.Dequeue();
                        }
                    }

                    break;
                case Direction.right:
                    newTile = new Point(Head.X + 1, Head.Y);
                    if (SnakeSegments.Contains(newTile)) {
                        GameLoop.Stop();
                        break;
                    }
                    if (newTile.X >= Resolution.Width) {
                        GameLoop.Stop();
                    } else {
                        SnakeSegments.Enqueue(newTile);
                        if (Food == newTile) {
                            PlaceFood();
                            if (GameLoop.Interval > 100) {
                                GameLoop.Interval -= 10;
                            }
                        } else {
                            SnakeSegments.Dequeue();
                        }
                    }

                    break;

                default:
                    break;
            }

            Invalidate();
        }

        void PlaceFood() {
            Random r = new Random();
            Point newFood = new Point(r.Next(Resolution.Width), r.Next(Resolution.Height));
            while (SnakeSegments.Contains(newFood)) {
                newFood = new Point(r.Next(Resolution.Width), r.Next(Resolution.Height));
            }
            Food = newFood;
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            foreach (var segment in SnakeSegments) {
                e.Graphics.FillRectangle(Brushes.White, segment.X * PixelSize, segment.Y * PixelSize, PixelSize, PixelSize);
            }

            e.Graphics.FillRectangle(Brushes.Red, Food.X * PixelSize, Food.Y * PixelSize, PixelSize, PixelSize);
        }

        public enum Direction {
            up,
            down,
            left,
            right
        }
    }
}
