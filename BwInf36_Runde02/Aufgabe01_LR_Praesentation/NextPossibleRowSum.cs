namespace Aufgabe01_LR_Praesentation
{
    /// <summary>
    /// Represents a NextPossibleRowSum type
    /// </summary>
    public struct NextPossibleRowSum
    {
        /// <summary>
        /// The next possible row sum
        /// </summary>
        public int PossibleRowSum { get; set; }

        /// <summary>
        /// The block that needs to be placed next for this <see cref="PossibleRowSum"/>
        /// </summary>
        public int UsedBrickIndex { get; set; }

        public NextPossibleRowSum(int possibleRowSum, int usedBrickIndex)
        {
            PossibleRowSum = possibleRowSum;
            UsedBrickIndex = usedBrickIndex;
        }
    }
}