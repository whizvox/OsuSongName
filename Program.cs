using System;

namespace OsuSongName
{

    class MainClass
    {

        private static long delay;
        private static System.IO.FileInfo filePath;
        private static bool invalidArgs = false;

        public static void Main(string[] args)
        {
            CheckArguments(args);
            bool breakLoop = false;
            long last = (long) DateTime.Now.TimeOfDay.TotalMilliseconds - delay;
            long current;
            string songName = null;
            while (!breakLoop)
            {
                if (invalidArgs)
                {
                    Console.Beep();
                    Console.WriteLine("\n" + Constants.MSG_ERRORS);
                    switch (Console.ReadKey().KeyChar)
                    {
                        case '1':
                            invalidArgs = false;
                            break;
                        case '2':
                            breakLoop = true;
                            break;
                    }
                }

                if (breakLoop) break;

                if ((current = (long) DateTime.Now.TimeOfDay.TotalMilliseconds) - last >= delay)
                {
                    last = current;
                    if (!IsOsuRunning())
                    {
                        Console.Clear();
                        Console.Beep();
                        Console.WriteLine(Constants.MSG_NOT_RUNNING);
                        breakLoop |= Console.ReadKey().KeyChar == '2';
                    }
                    else
                    {
                        if (!System.IO.File.Exists(filePath.FullName))
                        {
                            System.IO.File.CreateText(filePath.FullName);
                        }
                        string currentSongName = GetSongName();
                        if (currentSongName != null && currentSongName != songName)
                        {
                            System.IO.File.WriteAllText(filePath.FullName, String.Format(Constants.FORMAT_PLAYING, currentSongName));
                            songName = currentSongName;
                            Console.Clear();
                            Console.WriteLine(String.Format(Constants.FORMAT_INFO, delay, filePath, String.Format(Constants.FORMAT_PLAYING, songName)));
                        }
                    }
                }
            }
        }

        public static void CheckArguments(string[] args)
        {
            string delayArgValue = GetArgumentValue(args, Constants.DELAY_DEFAULT.ToString(), "d", "delay");
            try
            {
                delay = long.Parse(delayArgValue);
            }
            catch
            {
                Console.WriteLine(String.Format(Constants.FORMAT_INVALID_DELAY, delayArgValue));
                delay = Constants.DELAY_DEFAULT;
                invalidArgs = true;
            }
            // min = 0, max = 5000 (5 sec)
            if (delay < 0 || delay > 5000)
            {
                Console.WriteLine(String.Format(Constants.FORMAT_INVALID_DELAY, delayArgValue));
                delay = Constants.DELAY_DEFAULT;
                invalidArgs = true;
            }
            string fileArgValue = GetArgumentValue(args, Constants.FILE_DEFAULT, "f", "file");
            try
            {
                filePath = new System.IO.FileInfo(fileArgValue);
            }
            catch
            {
                Console.WriteLine(String.Format(Constants.FORMAT_INVALID_FILE, fileArgValue));
                invalidArgs = true;
            }
        }

        public static string GetSongName()
        {
            foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcessesByName(Constants.OSU_NAME))
            {
                if (process.MainWindowTitle.Length > 0)
                {
                    // if song isn't being played, then title will read "osu!", "osu!cuttingedge" or "osu!beta"
                    if (!process.MainWindowTitle.Contains("-"))
                    {
                        return Constants.SELECTING;
                    }
                    try
                    {
                        return process.MainWindowTitle.Substring(process.MainWindowTitle.IndexOf("-") + 2);
                    }
                    // could throw exception if another process name is "osu!" and the title is less than 6 chars
                    catch {}
                }
            }
            return null;
        }

        public static bool IsOsuRunning()
        {
            return System.Diagnostics.Process.GetProcessesByName(Constants.OSU_NAME).Length != 0;
        }

        // works assuming none of the argument names have '=' in them
        public static string GetArgumentValue(string[] argsAll, string defaultValue, params string[] argNames)
        {
            foreach (string arg in argsAll)
            {
                foreach (string name in argNames)
                {
                    if (arg.StartsWith(name))
                    {
                        try
                        {
                            return arg.Substring(arg.IndexOf("=") + 1);
                        }
                        catch {
                            Console.WriteLine(String.Format(Constants.FORMAT_INVALID_VALUE, arg));
                        }
                    }
                }
            }
            return defaultValue;
        }

    }

    struct Constants
    {

        public const string
            OSU_NAME = "osu!",
            FORMAT_PLAYING = "Playing: {0}",
            SELECTING = "( selecting... )",
            MSG_NOT_RUNNING = "It seems as no instances of osu! are running.\nPress [1] to refresh or [2] to close.",
            MSG_ERRORS = "Press [1] to continue using defaults, or press [2] to close.",
            FORMAT_RUNNING = "Writing song name to [{0}]...\n{1}",
            FORMAT_INVALID_FILE = "WARNING! Invalid file: {0}",
            FORMAT_INVALID_DELAY = "WARNING! Invalid delay: {0}",
            FORMAT_INVALID_VALUE = "WARNING! Invalid argument with value: {0}",
            FORMAT_INFO = "Delay: {0} ms\nFile: {1}\n{2}";

        public static readonly string FILE_DEFAULT = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "osu_song_name.txt");

        // milliseconds
        public const long DELAY_DEFAULT = 250;

    }

}
