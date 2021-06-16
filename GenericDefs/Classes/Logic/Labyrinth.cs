using GenericDefs.Functions.CoordinateGeometry;
using System.Collections.Generic;

namespace GenericDefs.Classes.Logic
{
    /// <summary>
    /// Defines an m X m chambers labyrinth.
    /// Coordinates from 1 to m along X-axis and 1 to m along y-axis;
    /// </summary>
    public class Labyrinth
    {
        int M;
        int Xmin, Xmax;
        int Ymin, Ymax;
        RectangularBoundary<int> B;
        public Labyrinth(int m)
        {
            this.M = m;
            this.Xmax = m; this.Xmin = 1;
            this.Ymax = m; this.Ymin = 1;
            this._chambers = new Dictionary<int, Chamber>();

            this.GenerateBoundary();
            this.GenerateChambers();
            this.SetChamberProperties();
        }

        int _currentChamber = 1;
        bool _isInMotion = false;
        public void StartMoving(int startChamber, int moveCount)
        {
            if (this._isInMotion) return;

            this._isInMotion = true;
            this._currentChamber = startChamber;
            Chamber c = this.GetChamber(startChamber);
            c.EnterHere();

            int curCount = 0;
            while (curCount <= moveCount)
            {
                curCount++;
                c = c.MoveLogically();
                this._currentChamber = c.Number;
            }

            this._isInMotion = false;
        }

        public List<int> GetUnEnteredChambers()
        {
            List<int> cList = new List<int>();

            foreach (KeyValuePair<int, Chamber> kvp in this._chambers)
            {
                Chamber c = kvp.Value;
                if (c.NeverEnteredHere())
                {
                    cList.Add(c.Number);
                }
            }

            return cList;
        }

        void GenerateBoundary()
        {
            B = new RectangularBoundary<int>(this.Xmin, this.Xmax, this.Ymin, this.Ymax);
        }

        int _chamberCount;
        Dictionary<int, Chamber> _chambers;
        void GenerateChambers()
        {
            int row = 1;
            this._chamberCount = 0;
            while (row <= this.M)
            {
                int column = 0;
                while (column <= this.M)
                {
                    this._chamberCount++;
                    column++;
                    _chambers.Add(this._chamberCount, new Chamber(column, row) { Number = GetChamberNumber(column, row) });
                    if (column == this.M) { break; }
                }
                if (row == this.M) { break; }
                row++;
            }
        }

        void SetChamberProperties()
        {
            foreach (KeyValuePair<int, Chamber> kvp in this._chambers)
            {
                Chamber c = kvp.Value;

                bool isBoundary = false;
                if (c.Coordinates.X == this.Xmin)
                {
                    c.IsLeftEdge = true;
                    isBoundary = true;
                    c.Left = null;
                    c.Right = this.GetChamber(c.Coordinates.X + 1, c.Coordinates.Y);
                    c.HorizontalMirror = this.GetChamber(this.M, c.Coordinates.Y);
                }
                else if (c.Coordinates.X == this.Xmax)
                {
                    c.IsRightEdge = true;
                    isBoundary = true;
                    c.Right = null;
                    c.Left = this.GetChamber(c.Coordinates.X - 1, c.Coordinates.Y);
                    c.HorizontalMirror = this.GetChamber(1, c.Coordinates.Y);
                }
                else
                {
                    c.IsRightEdge = false;
                    c.Left = this.GetChamber(c.Coordinates.X - 1, c.Coordinates.Y);
                    c.IsLeftEdge = false;
                    c.Right = this.GetChamber(c.Coordinates.X + 1, c.Coordinates.Y);
                }
                if (c.Coordinates.Y == this.Ymin)
                {
                    c.IsBottomEdge = true;
                    isBoundary = true;
                    c.Below = null;
                    c.Above = this.GetChamber(c.Coordinates.X, c.Coordinates.Y + 1);
                    c.VerticalMirror = this.GetChamber(c.Coordinates.X, this.M);
                }
                else if (c.Coordinates.Y == this.Ymax)
                {
                    c.IsTopEdge = true;
                    isBoundary = true;
                    c.Above = null;
                    c.Below = this.GetChamber(c.Coordinates.X, c.Coordinates.Y - 1);
                    c.VerticalMirror = this.GetChamber(c.Coordinates.X, 1);
                }
                else
                {
                    c.IsBottomEdge = false;
                    c.Above = this.GetChamber(c.Coordinates.X, c.Coordinates.Y + 1);
                    c.IsTopEdge = false;
                    c.Below = this.GetChamber(c.Coordinates.X, c.Coordinates.Y - 1);
                }
                c.IsBoundary = isBoundary;
            }
        }

        Chamber GetChamber(int x, int y)
        {
            return this.GetChamber(GetChamberNumber(x, y));
        }

        Chamber GetChamber(int number) {
            return this._chambers[number];
        }

        int GetChamberNumber(int x, int y)
        {
            return (this.M * (y - 1)) + x;
        }
    }
    public class Chamber
    {
        public Chamber Left;
        public Chamber Right;
        public Chamber Above;
        public Chamber Below;
        public Chamber HorizontalMirror;
        public Chamber VerticalMirror;
        public int Number;

        public TwoDCoordinates<int> Coordinates;
        public Chamber(int x, int y)
        {
            this.Coordinates = new TwoDCoordinates<int>(x, y);
        }

        public bool IsBoundary;
        public bool IsBottomEdge;
        public bool IsTopEdge;
        public bool IsRightEdge;
        public bool IsLeftEdge;

        public Chamber MirrorHorizontal()
        {
            if (this.IsLeftEdge || this.IsRightEdge)
            {
                return this.HorizontalMirror;
            }
            return null;
        }

        public Chamber MirrorVertical()
        {
            if (this.IsTopEdge || this.IsBottomEdge)
            {
                return this.VerticalMirror;
            }
            return null;
        }

        Chamber MoveLeft()
        {
            if (this.IsLeftEdge) { return MirrorHorizontal(); }
            else { return this.Left; }
        }

        Chamber MoveRight()
        {
            if (this.IsRightEdge) { return MirrorHorizontal(); }
            else { return this.Right; }
        }

        Chamber MoveUp()
        {
            if (this.IsTopEdge) { return MirrorVertical(); }
            else { return this.Above; }
        }

        Chamber MoveDown()
        {
            if (this.IsBottomEdge) { return MirrorVertical(); }
            else { return this.Below; }
        }

        public Chamber MoveLogically()
        {
            this.MoveOut();

            Chamber c = null;
            if (this.Number % 4 == 0) { c = this.MoveUp(); }
            else if (this.Number % 4 == 1) { c = this.MoveRight(); }
            else if (this.Number % 4 == 2) { c = this.MoveDown(); }
            else if (this.Number % 4 == 3) { c = this.MoveLeft(); }

            if (c != null) { c.EnterHere(); }
            return c;
        }

        private bool _neverEntered = true;
        public void EnterHere()
        {
            this._neverEntered = false;
            this._isMinotaurHere = true;
        }

        private void MoveOut()
        {
            this._isMinotaurHere = false;
        }

        public bool NeverEnteredHere()
        {
            return this._neverEntered;
        }

        private bool _isMinotaurHere = false;
        public bool IsMinotaurHere()
        {
            return this._isMinotaurHere;
        }
    }
}