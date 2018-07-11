namespace Aufgabe01_LR_Praesentation
{
    /// <summary>
    /// Represents a wall
    /// </summary>
    public class Wall
    {
        /// <summary>
        /// The rows of the wall
        /// </summary>
        public Row[] Rows { get; set; }

        private Wall(int height)
        {
            Rows = new Row[height];
        }

        /// <summary>
        /// Creates a new wall
        /// </summary>
        /// <param name="height">The number of rows in the wall</param>
        /// <param name="bricksPerRow">The number of bricks per row</param>
        public Wall(int height, int bricksPerRow)
        {
            Rows = new Row[height];
            for (var i = 0; i < Rows.Length; i++)
            {
                Rows[i] = new Row(bricksPerRow);
            }
        }

        /// <summary>
        /// Clones the <see cref="Wall"/>
        /// </summary>
        /// <returns>The cloned wall instance</returns>
        public Wall Clone()
        {
            var wallClone = new Wall(Rows.Length);
            for (var i = 0; i < Rows.Length; i++)
            {
                wallClone.Rows[i] = Rows[i].Clone();
            }

            return wallClone;
        }
    }
}