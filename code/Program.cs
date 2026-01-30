using BlowUp.Parsers;
using BlowUp.Renderers;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;

namespace BlowUp
{
    internal class Program
    {
        private const string Version = "1.6.0";

        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine($"BlowUp v. {Version}.");

            try
            {
                var applicationDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                
                var runnersDirectoryPath = Path.Combine(applicationDirectoryPath, "Runners");

                var baseDirectoryPath = args[0];
                var siteStyleFilePath = args[1];
                var siteHeaderFilePath = (args.Length > 2) && !args[2].Equals("-") ? args[2] : null;
                var siteFooterFilePath = (args.Length > 3) && !args[3].Equals("-") ? args[3] : null;

                ProcessDirectory(baseDirectoryPath, runnersDirectoryPath, siteStyleFilePath, siteHeaderFilePath, siteFooterFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} [{ex.GetType()}]");
            }
        }

        private static void ProcessDirectory(string baseDirectoryPath, string runnersDirectoryPath, string siteStyleFilePath, string siteHeaderFilePath, string siteFooterFilePath)
        {
            foreach (var sourceFilePath in Directory.GetFiles(baseDirectoryPath, "*.bu", SearchOption.AllDirectories))
            {
                ProcessFile(sourceFilePath, runnersDirectoryPath, siteStyleFilePath, siteHeaderFilePath, siteFooterFilePath);
            }
        }

        private static void ProcessFile(string sourceFilePath, string runnersDirectoryPath, string siteStyleFilePath, string siteHeaderFilePath, string siteFooterFilePath)
        {
            Console.WriteLine($"Processing file \"{sourceFilePath}\"...");

            try
            {
                var sourceDirectoryPath = Path.GetDirectoryName(sourceFilePath);

                Environment.CurrentDirectory = sourceDirectoryPath;

                var parser = new BlowUpParser();

                using (var streamReader = new StreamReader(sourceFilePath))
                {
                    string inputLine; while ((inputLine = streamReader.ReadLine()) != null)
                    {
                        parser.ParseLine(inputLine);
                    }
                }

                var rendererFilePath = Path.ChangeExtension(sourceFilePath, HtmlRenderer.GetFileExtension());

                Console.WriteLine($"Generating file \"{rendererFilePath}\"...");

                using (var stream = new FileStream(rendererFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    new HtmlRenderer(parser.Document, stream, sourceDirectoryPath, runnersDirectoryPath, siteStyleFilePath, siteHeaderFilePath, siteFooterFilePath).RenderDocument();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} [{ex.GetType()}]");
            }
        }
    }
}