using ImageProcessor.Plugins.WebP.Imaging.Formats;
using ImageProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WebpConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isResize = true;
            int newWidth = 760;

            string workingFolderPath = Path.Combine(Environment.CurrentDirectory, "workplace");
            List<string> fileEntries = Directory.GetFiles(workingFolderPath).ToList();
            foreach (string fileName in fileEntries) {
                var newFileName = Path.GetFileName(fileName);
                newFileName = newFileName.Replace(Path.GetExtension(fileName), ".webp");
                if (File.Exists(fileName))
                {
                    try
                    {
                        using (var webPFileStream = new FileStream(Path.Combine(Environment.CurrentDirectory, "workplace", newFileName), FileMode.OpenOrCreate))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                var thisImgFactory = imageFactory.Load(File.OpenRead(fileName))
                                    .Format(new WebPFormat())
                                    .Quality(100);
                                if (isResize)
                                {
                                    int newHeight = (int)Math.Ceiling(((double)newWidth / (double)thisImgFactory.Image.Width) * (double)thisImgFactory.Image.Height);
                                    thisImgFactory.Resize(new Size(newWidth, newHeight)).Save(webPFileStream);
                                }
                                else {
                                    thisImgFactory.Format(new WebPFormat())
                                        .Quality(100)
                                        .Save(webPFileStream);
                                }
                            }
                        }
                    } catch { }
                }
            }
        }
    }
}
