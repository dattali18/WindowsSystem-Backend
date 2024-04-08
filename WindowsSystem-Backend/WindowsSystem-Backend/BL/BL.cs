namespace WindowsSystem_Backend.BL 
{
    public class Bl 
    {
        public BlLibrary BlLibrary { get;}

        public BlMovie BlMovie { get; }

        public BlTvSeries BlTvSeries { get; }

        public BlJsonConversion BlJsonConversion { get; }

        public Bl()
        {
            BlLibrary = new BlLibrary();
            BlMovie = new BlMovie();
            BlTvSeries = new BlTvSeries();
            BlJsonConversion = new BlJsonConversion();
        }
    }
}