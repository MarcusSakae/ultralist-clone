using System.Text;

namespace Cheap.Ultralist.KnockOff
{
    internal class UlConsole
    {
        internal static void Output(List<string> results, bool useColor)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var colors = new Dictionary<string, string> {
                { "[blue]", "\u001b[34m" },
                { "[green]", "\u001b[32m" },
                { "[red]", "\u001b[31m" },
                { "[yellow]", "\u001b[33m" },
                { "[reset]", "\u001b[0m" },
            };
            var no_colors = new Dictionary<string, string> {
                { "[blue]", "" },
                { "[green]", "" },
                { "[red]", "" },
                { "[yellow]", "" },
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