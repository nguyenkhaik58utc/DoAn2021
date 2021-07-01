using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace iDAS.Core
{
    public class CaptchaValid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (HttpContext.Current.Session["Captcha"] == null || HttpContext.Current.Session["Captcha"].ToString() != value.ToString())
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }

    public class Captcha
    {
        public ActionResult CreateCaptcha(string prefix, bool effect = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            //generate new question 
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //add question to session 
            if (FormsAuthentication.CookiesSupported)
                HttpContext.Current.Session["Captcha" + prefix] = a + b;

            //image stream 
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (System.Drawing.Image bmp = new Bitmap(110, 20))
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //draw effect 
                if (effect)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 5; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (110 / 3));
                        x = rand.Next(0, 110);
                        y = rand.Next(0, 20);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //draw captcha 
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, -3);

                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = new FileContentResult(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }
    }
}
