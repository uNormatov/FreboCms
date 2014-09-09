using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using FCore.Helper;

namespace FModules
{
    public class CaptchaHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            RenderImage(context);
        }

        protected void RenderImage(HttpContext context)
        {
            int width = ValidationHelper.GetInteger(context.Request.QueryString["width"], 150);
            int height = ValidationHelper.GetInteger(context.Request.QueryString["height"], 40);
            int length = ValidationHelper.GetInteger(context.Request.QueryString["length"], 6);
            string sessionId = ValidationHelper.GetString(context.Request.QueryString["sessionid"], string.Empty);
            object drawString = context.Session[sessionId];
            if (drawString == null)
                drawString = CreateRandomWord(length);
            context.Session.Add(sessionId, drawString);
            using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                using (Graphics graph = Graphics.FromImage(bitmap))
                {
                    Rectangle rect = new Rectangle(0, 0, width - 1, height - 1);
                    graph.FillRectangle(Brushes.White, rect);

                    String[] crypticFonts = new String[11];
                    crypticFonts[0] = "Arial";
                    crypticFonts[1] = "Verdana";
                    crypticFonts[2] = "Comic Sans MS";
                    crypticFonts[3] = "Impact";
                    crypticFonts[4] = "Haettenschweiler";
                    crypticFonts[5] = "Lucida Sans Unicode";
                    crypticFonts[6] = "Garamond";
                    crypticFonts[7] = "Courier New";
                    crypticFonts[8] = "Book Antiqua";
                    crypticFonts[9] = "Arial Narrow";
                    crypticFonts[10] = "Estrangelo Edessa";


                    Font drawFont;
                    string font = string.Empty;
                    string letter = string.Empty;
                    using (SolidBrush drawBrush = new SolidBrush(Color.Black))
                    {
                           graph.DrawRectangle(new Pen(Color.Red, 0), rect);
                        for (int i = 0; i <= drawString.ToString().Length - 1; i++)
                        {
                            font = crypticFonts[new Random().Next(i)];
                            drawFont = new Font(font, 19, FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout);
                            letter = drawString.ToString().Substring(i, 1);
                            graph.DrawString(letter, drawFont, drawBrush, i * 20, 2);
                            graph.Flush();
                        }
                    }
                    context.Response.ContentType = "image/jpeg";
                    bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    graph.Dispose();
                    bitmap.Dispose();
                    context.Response.End();
                    context.Response.Flush();
                }
            }

        }

        private string CreateRandomWord(int numberOfChars)
        {
            if (numberOfChars > 36)
            {
                throw new InvalidOperationException("Random Word Charecters can not be greater than 36.");
            }
            char[] columns = new char[36];

            for (int charPos = 65; charPos < 65 + 26; charPos++)
                columns[charPos - 65] = (char)charPos;

            for (int intPos = 48; intPos <= 57; intPos++)
                columns[26 + (intPos - 48)] = (char)intPos;

            StringBuilder randomBuilder = new StringBuilder();


            Random randomSeed = new Random();
            for (int incr = 0; incr < numberOfChars; incr++)
            {
                randomBuilder.Append(columns[randomSeed.Next(36)].ToString());

            }

            return randomBuilder.ToString();
        }
    }
}
