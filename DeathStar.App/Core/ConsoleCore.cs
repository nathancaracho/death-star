using System;
using DeathStar.App.Domain.Models;
using McMaster.Extensions.CommandLineUtils;

namespace DeathStar.App.Core
{
    public static class ConsoleCore
    {
        private const string _deathStarSuffix = "(º-) ";

        public static void Title()
        {
            var msg = @"
 _______   _______      ___   .___________. __    __            _______..___________.    ___      .______      
|       \ |   ____|    /   \  |           ||  |  |  |          /       ||           |   /   \     |   _  \     
|  .--.  ||  |__      /  ^  \ `---|  |----`|  |__|  |  ______ |   (----``---|  |----`  /  ^  \    |  |_)  |    
|  |  |  ||   __|    /  /_\  \    |  |     |   __   | |______| \   \        |  |      /  /_\  \   |      /     
|  '--'  ||  |____  /  _____  \   |  |     |  |  |  |      .----)   |       |  |     /  _____  \  |  |\  \----.
|_______/ |_______|/__/     \__\  |__|     |__|  |__|      |_______/        |__|    /__/     \__\ | _| `._____|
                                  
          .          .
  .          .                  .          .              .
        +.           _____  .        .        + .                    .   + .                    .
    .        .   ,-~'     '~-.                                +
    |-|        ,^ ___         ^. +                  .    .       .
              / .^   ^.         \         .      
             !  l  o  !          !  .         + .    .       |-|    
     .       |_ `.___.'        _,|                    +
             |^~'-----------''~ ^|       +    |-|       x        +   .
   +       . !                   !     .       
              ^.               .^            .            +.
                ''-.._____.,-' .                    .
          +           .                .   +                       .
   +          .             +                                  .
          .             .      .       
            ";
            Console.WriteLine(msg);
        }
        public static void Success(string msg) => WriteLineWithColor($"Success: May the force be with \n {msg}", ConsoleColor.Green);
        public static void Warning(string msg) => WriteLineWithColor($"Warning: It's a trap!! \n {msg}", ConsoleColor.Yellow);
        public static void Error(string msg) => WriteLineWithColor($"ERROR NOOOOOOOOOOO!! \n {msg}", ConsoleColor.Red);
        public static void Message(string msg) => WriteLineWithColor($"{_deathStarSuffix} {msg}", ConsoleColor.Gray);
        public static void ProgressBar(ProgressBar progress) => Console.WriteLine(progress);

        public static bool GetYesNo(string msg) => Prompt.GetYesNo($"Warning It's a trap!! \n {msg}", false, ConsoleColor.Yellow);

        public static void ProgressBar(int done, int total)
        {
            Console.CursorLeft = 0;
            WriteLineWithColor($"{_deathStarSuffix} {(ProgressBar)(done, total)}", ConsoleColor.Blue);
            Console.CursorTop = Console.GetCursorPosition().Top - 1;
        }
        private static void WriteLineWithColor(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }


    }
}