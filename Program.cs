using System;
using System.Runtime.InteropServices;
using EpApp.Classes;

namespace EpApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("App Started");
            Console.WriteLine("Talking to Reddit...");

            Reddit reddit = new Reddit();
            RedditReponse response = reddit.GetPostsAsync().Result;

            string topImage = response.data.children[0].data.url;

            Console.WriteLine("Reddit data retrieved: " + topImage);
            Console.WriteLine("Downloading file...");

            IFileSaver saver = new FileDownloader();
            string path = saver.Save(topImage);

            Console.WriteLine("File downloaded to: " + path);
            Console.WriteLine("Setting desktop wallpaper...");

            IWallpaperSetter setter = CreateWallpaperSetter();
            setter.SetWallpaper(path);

            Console.WriteLine("Done!");
        }

        private static IWallpaperSetter CreateWallpaperSetter()
        {
            //If Windows
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new WindowsWallpaperImageSetter();
            
            //If OSX
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return new MacOSWallpaperImageSetter();

            throw new NotImplementedException("Cannot set wallpaper for this OS.");
        }
    }
}
