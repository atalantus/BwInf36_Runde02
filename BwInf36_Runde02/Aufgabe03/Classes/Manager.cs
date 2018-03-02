namespace Aufgabe03.Classes
{
    public class Manager
    {
        private static Manager _instance = null;

        private Manager() { }

        public static Manager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Manager();
                return _instance;
            }
        }
    }
}