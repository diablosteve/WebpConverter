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
            int newWidth = 0;
            int newHeight = 0;

            string workingFolderPath = Path.Combine(Environment.CurrentDirectory, "workplace");
            List<string> fileEntries = Directory.GetFiles(workingFolderPath).ToList();
            foreach (string fileName in fileEntries) {
                isResize = true;
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
                                var currentW = (double)thisImgFactory.Image.Width;
                                var currentH = (double)thisImgFactory.Image.Height;
                                if (newWidth == 0 && newHeight == 0) {
                                    isResize = false;
                                }
                                else if (newWidth != 0 && newWidth >= currentW) {
                                    isResize = false;
                                }
                                else if (newHeight != 0 && newHeight >= currentH) {
                                    isResize = false;
                                }
                                if (isResize)
                                {
                                    if (newWidth != 0)
                                    {
                                        newHeight = (int)Math.Ceiling(((double)newWidth / currentW) * currentH);
                                    }
                                    else if (newHeight != 0) {
                                        newWidth = (int)Math.Ceiling(((double)newHeight / currentH) * currentW);
                                    }
                                    thisImgFactory.Resize(new Size(newWidth, newHeight)).Save(webPFileStream);
                                }
                                else {
                                    thisImgFactory.Save(webPFileStream);
                                }
                            }
                        }
                    } catch { }
                }
            }
        }
    }
}
