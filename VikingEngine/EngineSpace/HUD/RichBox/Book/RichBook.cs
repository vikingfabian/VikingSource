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
            "continue text",
            "<p>new paragrapth",
            "<*>bullet point",
            "<h2>subtitle",
            "<br>text <img=InterfaceIconCamera> more text",
            "<br>Quotes are <name=ItemName>"
        };
       
        public void GenerateUI(RichBoxContent content, string[] inputLines, Dictionary<string, string> quotes, RichBookSettings settings)
        {
            foreach (var line in inputLines)
            {
                if (line.StartsWith("<h1>"))
                {
                    //Console.WriteLine($"h1(\"{line.Substring(4)}\")");
                    if (content.Count > 0)
                    {
                        content.newParagraph();
                    }
                    //content.h1(line.Substring(4), settings.head1col);
                    content.Add(new RbBeginTitle(1));
                    ParseInlineElements(content, line.Substring(4), settings.head1col, quotes);
                }
                else if (line.StartsWith("<h2>"))
                {
                    //Console.WriteLine($"h2(\"{line.Substring(4)}\")");
                    if (content.Count > 0)
                    {
                        content.newParagraph();
                    }
                    //content.h2(line.Substring(4), settings.head2col);
                    content.Add(new RbBeginTitle(1));
                    ParseInlineElements(content, line.Substring(4), settings.head2col, quotes);
                }
                else if (line.StartsWith("<p>"))
                {
                    //Console.WriteLine($"p(\"{line.Substring(3)}\")");
                    content.newParagraph();
                    ParseInlineElements(content, line.Substring(3), null, quotes);
                }
                else if (line.StartsWith("<*>"))
                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.warsBulletPoint, 0.8f, null, 0f, 0.3f));
                    ParseInlineElements(content, line.Substring(3), null, quotes);
                    //Console.WriteLine($"bullet(\"{line.Substring(3)}\")");
                }
                else if (line.StartsWith("<br>"))
                {
                    //string content = line.Substring(4);
                    //Console.WriteLine(ParseInlineElements(content));
                    content.newLine();
                    ParseInlineElements(content, line.Substring(4), null, quotes);
                }
                else
                {
                    // Fallback for strings without a known starting tag
                    //Console.WriteLine(ParseInlineElements(line));
                    ParseInlineElements(content, line, null, quotes);
                }
            }
        }

        // Helper method to handle inline tags like <img=pinIcon> and <name=ItemName>
        void ParseInlineElements(RichBoxContent content, string contentLine, Color? color, Dictionary<string, string> quotes)
        {
            // Regex looks for either <img= or <name= followed by anything that isn't a >, then closes with >
            // Group 1 captures the tag type ("img" or "name")
            // Group 2 captures the value
            var regex = new Regex(@"<(img|name)=([^>]+)>");
            var matches = regex.Matches(contentLine);

            // If there are no inline elements, just return standard text
            if (matches.Count == 0)
            {
                content.Add(new RbText(contentLine, color));
                return;
            }

            // If there are inline elements, break the string apart and wrap it in a row
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                // Grab the text before the tag
                if (match.Index > lastIndex)
                {
                    string textPart = contentLine.Substring(lastIndex, match.Index - lastIndex);
                    content.Add(new RbText(textPart, color));
                }

                string tagType = match.Groups[1].Value;
                string tagValue = match.Groups[2].Value;

                if (tagType == "img")
                {
                    if (Enum.TryParse(tagValue, out SpriteName sprite))
                    {
                        content.Add(new RbImage(sprite));
                    }
                    else
                    {
                        content.Add(new RbImage(SpriteName.MissingImage));
                    }
                }
                else if (tagType == "name")
                {
                    // Look up the name in the quotes dictionary
                    if (quotes != null && quotes.TryGetValue(tagValue, out string quoteText))
                    {
                        content.Add(new RbText(quoteText, color));
                    }
                    else
                    {
                        // Fallback if the quote key doesn't exist
                        content.Add(new RbText($"[Missing Quote: {tagValue}]"));
                    }
                }

                // Move the index forward past the matched tag
                lastIndex = match.Index + match.Length;
            }

            // Grab any remaining text after the last tag
            if (lastIndex < contentLine.Length)
            {
                string textPart = contentLine.Substring(lastIndex);
                content.Add(new RbText(textPart, color));
            }
        }

    }
}
