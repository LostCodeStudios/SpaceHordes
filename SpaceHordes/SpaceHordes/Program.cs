using System;

namespace SpaceHordes
{
#if WINDOWS || XBOX
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
#endif
}

