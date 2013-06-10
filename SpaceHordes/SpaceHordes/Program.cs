namespace SpaceHordes
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            using (SpaceHordes SpaceGame = new SpaceHordes())
            {
                SpaceGame.Run();
            }
        }
    }
}