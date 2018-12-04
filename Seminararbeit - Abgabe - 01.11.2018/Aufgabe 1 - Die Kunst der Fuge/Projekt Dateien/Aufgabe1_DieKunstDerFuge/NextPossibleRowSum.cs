namespace Aufgabe1_DieKunstDerFuge
{
    /// <summary>
    /// Represents a NextPossibleRowSum type.
    /// </summary>
    public struct NextPossibleRowSum
    {
        /// <summary>
        /// The next possible row sum.
        /// </summary>
        public int PossibleRowSum { get; set; }

        /// <summary>
        /// The block that needs to be placed next for this <see cref="PossibleRowSum"/>.
        /// </summary>
        public int UsedBrickIndex { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="possibleRowSum">RowSum.</param>
        /// <param name="usedBrickIndex">Index of the brick used to get the <see cref="PossibleRowSum"/></param>
        public NextPossibleRowSum(int possibleRowSum, int usedBrickIndex)
        {
            PossibleRowSum = possibleRowSum;
            UsedBrickIndex = usedBrickIndex;
        }
    }
}