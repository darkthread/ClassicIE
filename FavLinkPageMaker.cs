using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicIE
{
    public class FavLinkPageMaker
    {
        public static string CreateFavLinkPage()
        {
            // create Favorite link list
            var favFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Favorites");

            var sb = new StringBuilder();
            sb.AppendLine(@"
<!DOCTYPE html>            
<html><head>
<meta charset=""utf-8"" />
<title>Favorite Links</title>
<style>
body { font-family: Arial; font-size: 10pt; }
ul { list-style:none; padding-left: 16px; }
li { margin: 5px; }
.folder {
    background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA8AAAANCAYAAAB2HjRBAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAB3RJTUUH5wMXAiohbX2cdAAAARxJREFUKJGFkj1OAzEQhT97V/lrEBKIgiL0lDSIC1BwB27BwaBC9BRIHIGOdIgiUgjZ9c+j8DrrJBAsjTwznjfvzcjm++lIauZIABZhMOMzRleP2MmUfcfG1RzJIiqEJUZLXM5YPt8gN98LNouHoTKjokGYpEAGyQDp7vNgJlMm53fUUtUX5KJqzOj0muHJJUQHeAgtqE2xHFq9YvRxL+RBvnvwoAbCV7L1WwBCEXtqmndQ6Mzv3gSIZb73a2L7NzA3zYzRFw1cAS4LiN2soQeUvlzHrII5zxVd18CnZUW3JduXsj0o7sy0ucSwZsxxAhMSqzZn6hX9rqBg/ge8nRscU3++veAWM5ABY9e/TDmWQbLpI3X5anDA4cUtP6rINSVdpEx8AAAAAElFTkSuQmCC);
    background-repeat: no-repeat;
    background-position: 0 2px;
    padding-left: 20px;
}
.ico { width: 16px; height: 16px; vertical-align: middle; }
</style>
</head><body>");
            sb.AppendLine("<ul>");
            explore(favFolder);
            sb.AppendLine("</ul>");
            sb.AppendLine("</body></html>");
            var favLinkHtmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Classic-IE-Favorites.html");
            File.WriteAllText(favLinkHtmlPath, sb.ToString());
            return favLinkHtmlPath;
            void explore(string path)
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    sb.AppendLine($"<li><span class=folder>{Path.GetFileNameWithoutExtension(dir)}</span><ul>");
                    explore(dir);
                    sb.AppendLine($"</ul></li>");
                }
                foreach (var file in Directory.GetFiles(path, "*.url"))
                {
                    var linkName = Path.GetFileNameWithoutExtension(file);
                    //parse url file
                    var urlFileLines = File.ReadAllLines(file);
                    var url = urlFileLines.FirstOrDefault(l => l.StartsWith("URL="));
                    var ico = urlFileLines.FirstOrDefault(l => l.StartsWith("IconFile="));
                    if (ico != null) {
                        ico = ico.Substring(9);
                        if (!ico.StartsWith("http")) {
                            var ub = new UriBuilder();
                            ub.Path = Environment.ExpandEnvironmentVariables(ico);
                            ub.Scheme = "file";
                            ico = ub.Uri.AbsoluteUri;
                        }
                    }
                    if (url != null)
                    {
                        sb.AppendLine(@$"<li>
                        <img src=""{ico}"" class=ico /> 
                        <a href=""{url.Substring(4)}"" target=_blank>{linkName}</a>
                        </li>");
                    }
                }
            };
        }
    }
}