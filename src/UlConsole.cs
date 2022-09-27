using System.Text;

namespace Cheap.Ultralist.KnockOff
{
    class UlConsole
    {
        // We use some ascii escape codes to color the output.
        // Allows us to embed colors in the string.
        public static void Output(List<string> results, bool useColor)
        {
            var colors = new Dictionary<string, string> {
                { "[blue]", "\u001b[34m" },
                { "[green]", "\u001b[32m" },
                { "[red]", "\u001b[31m" },
                { "[yellow]", "\u001b[33m" },
                { "[purple]", "\u001b[35m" },
                { "[reset]", "\u001b[0m" },
            };
            var no_colors = new Dictionary<string, string> {
                { "[blue]", "" },
                { "[green]", "" },
                { "[red]", "" },
                { "[yellow]", "" },
                { "[purple]", "" },
                { "[reset]", "" },
            };

            foreach (string line in results)
            {
                // Either replace all colors with their ANSI codes, or replace them with nothing
                string replaced = useColor
                    ? colors.Aggregate(line, (current, color) => current.Replace(color.Key, color.Value))
                    : no_colors.Aggregate(line, (current, color) => current.Replace(color.Key, color.Value));
                Console.WriteLine(replaced);
            }

        }

    }
}