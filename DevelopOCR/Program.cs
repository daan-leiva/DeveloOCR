using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Tesseract;
using System.Diagnostics;

namespace DevelopOCR
{
    class Program
    {
        static void Main(string[] args)
        {
            string test_image_path = "C:/Users/Bruce Huffa/source/repos/DevelopOCR/DevelopOCR/test.png";
            // copy the image to a new location
            Image image = Image.FromFile(MakeImageBlackNWhite(test_image_path));
            string edited_image = "C:/Users/Bruce Huffa/source/repos/DevelopOCR/DevelopOCR/test_edit.png";
            image.Save(edited_image);
            string buffer_image = "C:/Users/Bruce Huffa/source/repos/DevelopOCR/DevelopOCR/test_buffer.png";
            TesseractTest(edited_image, buffer_image);
        }

        private static void TesseractTest(string image_path, string buffer_path)
        {
            //Bitmap image = new Bitmap("C:/Users/Bruce Huffa/Desktop/testImageYarnFieldAllocation_3_a.PNG");
            //MakeImageBlackNWhite(image);

            // take image from screenshot by using yarn_rectangle coordinates
            string testImagePath = image_path;
            // launch engine
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    //engine.SetVariable("tessedit_char_whitelist", "9");
                    using (var img = Pix.LoadFromFile(testImagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());
                            Console.WriteLine("Text (GetText): \r\n{0}", text);
                            Console.WriteLine("Text (iterator):");

                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();
                                do
                                {
                                    do
                                    {

                                        do
                                        {
                                            do
                                            {
                                                do
                                                {
                                                    if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                                    {
                                                        Console.WriteLine("<BLOCK>");
                                                    }
                                                    if (iter.GetText(PageIteratorLevel.Symbol).Trim().Contains("9"))
                                                    {
                                                        

                                                        Rect location = new Rect();
                                                        iter.TryGetBoundingBox(PageIteratorLevel.Symbol, out location);
                                                        if (location.Y1 < 200 && location.Y1 > 60)
                                                        {
                                                            Console.Write(iter.GetText(PageIteratorLevel.Symbol));
                                                            Console.Write(" ");
                                                            Console.Write("[" + location.X1 + ", " + location.Y1 + "]");

                                                            Image image = Image.FromFile(image_path);
                                                            using (Graphics g = Graphics.FromImage(image))
                                                            {
                                                                // Modify the image using g here... 
                                                                // Create a brush with an alpha value and use the g.FillRectangle function
                                                                Color customColor = Color.FromArgb(50, Color.Black);
                                                                SolidBrush shadowBrush = new SolidBrush(customColor);
                                                                g.FillRectangles(shadowBrush, new RectangleF[] { new RectangleF(location.X1, location.Y1, location.Width, location.Height) });
                                                            }
                                                            image.Save(buffer_path);
                                                            image.Dispose();
                                                            image = null;
                                                            Image buffer_image = Image.FromFile(buffer_path);
                                                            buffer_image.Save(image_path);
                                                            buffer_image.Dispose();
                                                            buffer_image = null;
                                                        }
                                                    }
                                                } while (iter.Next(PageIteratorLevel.Word, PageIteratorLevel.Symbol));
                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                {
                                                    Console.WriteLine();
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }


        private static string MakeImageBlackNWhite(string image_path)
        {
            Bitmap SourceImage = new Bitmap(image_path);
            using (Graphics gr = Graphics.FromImage(SourceImage)) // SourceImage is a Bitmap object
            {
                var gray_matrix = new float[][] {
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                new float[] { 0,      0,      0,      1, 0 },
                new float[] { 0,      0,      0,      0, 1 }
            };

                var ia = new System.Drawing.Imaging.ImageAttributes();
                ia.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(gray_matrix));
                ia.SetThreshold(0.3f); // Change this threshold as needed
                var rc = new Rectangle(0, 0, SourceImage.Width, SourceImage.Height);
                gr.DrawImage(SourceImage, rc, 0, 0, SourceImage.Width, SourceImage.Height, GraphicsUnit.Pixel, ia);
            }
            string bw_path = "C:/Users/Bruce Huffa/source/repos/DevelopOCR/DevelopOCR/test_bw.png";
            SourceImage.Save(bw_path);
            return bw_path;
        }
    }
}
