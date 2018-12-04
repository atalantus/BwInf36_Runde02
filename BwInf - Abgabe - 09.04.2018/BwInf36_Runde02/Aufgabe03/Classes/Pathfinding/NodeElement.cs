namespace Aufgabe03.Classes.Pathfinding
{
    public abstract class NodeElement
    {
        /// <summary>
        ///     Pathfinding nach Quax
        /// </summary>
        /// <param name="curStatus">Der aktuelle Status der Suche</param>
        /// <returns>Der neuen Status der Suche</returns>
        public abstract QuaxInfo SearchQuax(QuaxInfo curStatus);

        /// <summary>
        ///     Pathfinding zur Stadt
        /// </summary>
        /// <param name="curStatus">Der aktuelle Status der Suche</param>
        /// <returns>Der neuen Status der Suche</returns>
        public abstract PathInfo SearchPath(PathInfo curStatus);
    }
}