class Pathfinder 
{
	public Vector2 QuaxPos;
	public Vector2 StadtPos;
	private NodeElement _start;

	public bool FindPath() 
	{
		
	}
}

class Node : NodeElement
{
	private Quadrat _quadrat;
	private int _startDist;
	private int _zielDist;
	private Node[] _innereQuadrate;
	
	public bool SearchPath(Vector2 targetPos)
	{
	
	}
}

class AbschlussNode : NodeElement
{
	private Quadrat _quadrat;
	private int _startDist;
	private int _zielDist;
	
	public bool SearchPath(Vector2 targetPos)
	{
		
	}
}

class Quadrat 
{
	public enum RechteckTypen
	{
		UNBEKANNT,
		GEMISCHT,
		PASSIERBAR,
		WASSER
	}
	public RechteckTypen RechteckTyp { get; private set; }
	
	/**
	 * REPRAESENTIERT EINEN DROHNEN FLUG !!!
	 */
	private RechteckTypen GetRechteckTyp(int breite) 
	{
		if (allesWasser) return RechteckTypen.WASSER;
		else if (allesLand || breite == 2) return RechteckTypen.PASSIERBAR;
		else return RechteckTypen.GEMISCHT;
	}
}