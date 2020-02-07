using FluentScheduler;
using System.IO;
using System.Linq;

namespace PdfViewer.Extensions.Jobs
{
    public class DirectoryCleanUpJob : IJob
    {
        private readonly string _directoryPath;
        public DirectoryCleanUpJob(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public void Execute()
        {
            var directoryInfo = new DirectoryInfo(_directoryPath);

            directoryInfo.EnumerateFiles().ToList().ForEach(f => f.Delete());

            directoryInfo.EnumerateDirectories().ToList().ForEach(d => d.Delete());
        }
    }
}