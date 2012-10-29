using System;

namespace SpaceHordes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SpaceHordes SpaceGame = new SpaceHordes())
            {
                SpaceGame.Run();
            }
        }
    }
}

