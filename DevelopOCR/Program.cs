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
        }

        private void TessaractTest(/*string text_to_find*/)
        {
            //Bitmap image = new Bitmap("C:/Users/Bruce Huffa/Desktop/testImageYarnFieldAllocation_3_a.PNG");
            //MakeImageBlackNWhite(image);

            // take image from screenshot by using yarn_rectangle coordinates
            string testImagePath = "C:/Users/Bruce Huffa/Desktop/testImageYarnFieldAllocation_a.PNG";
            // launch engine
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    engine.SetVariable("tessedit_char_whitelist", "0123456789");
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
                                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                                {
                                                    Console.WriteLine("<BLOCK>");
                                                }
                                                Console.Write(iter.GetText(PageIteratorLevel.Word));
                                                Console.Write(" ");
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
            //Console.Write("Press any key to continue . . . ");
            //Console.ReadKey(true);
        }


        private Bitmap MakeImageBlackNWhite(Bitmap SourceImage)
        {
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
                ia.SetThreshold(0.5f); // Change this threshold as needed
                var rc = new Rectangle(0, 0, SourceImage.Width, SourceImage.Height);
                gr.DrawImage(SourceImage, rc, 0, 0, SourceImage.Width, SourceImage.Height, GraphicsUnit.Pixel, ia);
            }
            SourceImage.Save("C:/Users/Bruce Huffa/Desktop/testImageYarnFieldAllocation_3_a.PNG");
            return SourceImage;
        }
    }
}
