using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.EngineSpace.HUD.RichBox.Book
{
    //abstract class AbsRichBookMember
    //{ 

    //}

    //class Txt

    struct RichBookSettings
    {
        public Color head1col;
        public Color head2col;
    }

    struct RichBook
    {
        static readonly string[] example = new string[] {
            "<h1>title",
            "<br>text",
            "<p>new paragrapth",
            "<*>bullet point",
            "<h2>subtitle",
            "<br>text <img=pinIcon> more text",
        };
       
        public void GenerateUI(RichBoxContent content, string[] inputLines, RichBookSettings settings)
        {
            foreach (var line in inputLines)
            {
                if (line.StartsWith("<h1>"))
                {
                    //Console.WriteLine($"h1(\"{line.Substring(4)}\")");
                    content.h1(line.Substring(4), settings.head1col);
                }
                else if (line.StartsWith("<h2>"))
                {
                    //Console.WriteLine($"h2(\"{line.Substring(4)}\")");
                    content.h2(line.Substring(4), settings.head2col);
                }
                else if (line.StartsWith("<p>"))
                {
                    //Console.WriteLine($"p(\"{line.Substring(3)}\")");
                    content.newParagraph();
                    ParseInlineElements(content, line.Substring(3));
                }
                else if (line.StartsWith("<*>"))
                {
                    content.Add(new RbImage(SpriteName.warsBulletPoint, 0.8f, null, 0f, 0.3f));
                    ParseInlineElements(content, line.Substring(3));
                    //Console.WriteLine($"bullet(\"{line.Substring(3)}\")");
                }
                else if (line.StartsWith("<br>"))
                {
                    //string content = line.Substring(4);
                    //Console.WriteLine(ParseInlineElements(content));
                    content.newLine();
                    ParseInlineElements(content, line.Substring(4));
                }
                else
                {
                    // Fallback for strings without a known starting tag
                    //Console.WriteLine(ParseInlineElements(line));
                    ParseInlineElements(content, line);
                }
            }
        }

        // Helper method to handle inline tags like <img=pinIcon>
        void ParseInlineElements(RichBoxContent content, string contentLine)
        {
            // Regex looks for exactly <img= followed by anything that isn't a >, then closes with >
            var regex = new Regex(@"<img=([^>]+)>");
            var matches = regex.Matches(contentLine);

            // If there are no inline images, just return standard text
            if (matches.Count == 0)
            {
                //return $"text(\"{contentLine}\")";
                content.Add(new RbText(contentLine));
            }

            // If there are inline images, break the string apart and wrap it in a row
            var result = new List<string>();
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                // Grab the text before the image
                if (match.Index > lastIndex)
                {
                    string textPart = contentLine.Substring(lastIndex, match.Index - lastIndex);
                    //result.Add($"text(\"{textPart}\")");
                    content.Add(new RbText(textPart));
                    //content.hspace();
                }

                // Grab the image name
                string imgName = match.Groups[1].Value;
                //result.Add($"image(\"{imgName}\")");
                if (Enum.TryParse(imgName, out SpriteName sprite))
                {
                    content.Add(new RbImage(sprite));
                    //content.hspace();
                }
                else
                {
                    content.Add(new RbImage(SpriteName.MissingImage));
                }
                // Move the index forward
                lastIndex = match.Index + match.Length;
            }

            // Grab any remaining text after the last image
            if (lastIndex < contentLine.Length)
            {
                string textPart = contentLine.Substring(lastIndex);
                //result.Add($"text(\"{textPart}\")");
                content.Add(new RbText(textPart));
            }

            // Combine into a layout row
            //return $"row({string.Join(", ", result)})";
            
        }

        //    public RichBook(params AbsRichBookMember[] array)
        //        :base(array.Length)
        //    { 
        //        AddRange(array);
        //    }
    }
}
