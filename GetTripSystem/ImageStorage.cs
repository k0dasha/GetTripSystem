using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Packaging;

namespace GetTripSystem
{
    public static class ImageStorage
    {
        public static string GetImageFolder()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GetTripSystem",
                "Images"
            );
        }
        public static void CopyImage(string fileName, string filePath)
        {
            //string _filePath = GetImagePath(fileName);
            string folder = GetImageFolder();
            string newPath = Path.Combine(folder, fileName);
            Directory.CreateDirectory(folder);
            try
            {
                File.Copy(filePath, newPath);
            }
            catch (IOException ex)
            {
                throw new Exception("Ошибка копирования файла", ex);
            }
        }
        public static string GetImagePath(string fileName)
        {
            return Path.Combine(GetImageFolder(), fileName);
        }
    }
}
