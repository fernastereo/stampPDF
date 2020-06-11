using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using System;
using System.IO;
using MessagingToolkit.QRCode.Codec;
using System.Drawing;

namespace iTextSharp_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileNameExisting = @"C:\cualquierfolder\teststamp.pdf";
            string fileNameStamped = @"C:\cualquierfolder\teststamped.pdf";
            string imgSrc = @"C:\cualquierfolder\ronald.jpg";
            string imgGeneratedQR = @"C:\cualquierfolder\generatedqr.jpeg";

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(fileNameExisting), new PdfWriter(fileNameStamped));
            //Aqui tomo la imagen del archivo para enviarla al pdf
            ImageData img = ImageDataFactory.Create(imgSrc);

            //Aqui genero el qr
            string qrCode = "www.curaduria1cartagena.com";
            QRCodeEncoder qrImg = new QRCodeEncoder();
            qrImg.QRCodeVersion = 0;
            var imgQR = new Bitmap(qrImg.Encode(qrCode));
            imgQR.Save(imgGeneratedQR, System.Drawing.Imaging.ImageFormat.Jpeg);
            //Aqui tomo la imagen del QR para enviarla al pdf
            ImageData QR = ImageDataFactory.Create(imgGeneratedQR);

            PdfPage page;
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                //Aqui selecciono la pagina
                page = pdfDoc.GetPage(i);

                //Aqui agrego la firma
                float x = 10;
                float y = 10;
                float w = img.GetWidth() / 2;
                float h = img.GetHeight() / 2;
                new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc)
                    .AddImage(img, x, y, w, false);

                //Aqui agrego el QR
                x = 15 + w;
                y = 10;
                w = QR.GetWidth() / 2;
                new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc)
                        .AddImage(QR, x, y, w, false);
            }
            
            pdfDoc.Close();
            Console.WriteLine("Archivo Sellado!");
        }
    }
}
