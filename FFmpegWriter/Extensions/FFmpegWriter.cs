using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;
using Bonsai.Vision;
using Bonsai.IO;
using System.IO;

[Combinator]
[DefaultProperty("FileName")]
[Description("Writes the input image sequence to an FFmpeg process.")]
[WorkflowElementCategory(ElementCategory.Sink)]
public class FFmpegWriter
{
    [Description("The name of the output file.")]
    [Editor("Bonsai.Design.SaveFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
    public string FileName { get; set; }

    [Description("The optional suffix used to generate file names.")]
    public PathSuffix Suffix { get; set; }

    [Description("Indicates whether the output file should be overwritten if it already exists.")]
    public bool Overwrite { get; set; }

    [Description("The playback frame rate of the image sequence.")]
    public int FrameRate { get; set; }

    [Description("The optional set of command-line arguments to use for configuring the video codec.")]
    public string OutputArguments { get; set; }

    public IObservable<IplImage> Process(IObservable<IplImage> source)
    {
        return source.Publish(ps =>
        {
            var fileName = PathHelper.AppendSuffix(FileName, Suffix);
            var overwrite = Overwrite;
            if (File.Exists(fileName) && !overwrite)
            {
                throw new InvalidOperationException(string.Format("The file '{0}' already exists.", fileName));
            }

            PathHelper.EnsureDirectory(fileName);
            var pipe = @"\\.\pipe\" + Path.GetFileNameWithoutExtension(fileName);
            var writer = new ImageWriter { Path = pipe };
            return writer.Process(ps).Merge(ps.Take(1).Delay(TimeSpan.FromSeconds(1)).SelectMany(image =>
            {
                var args = string.Format("-f rawvideo -vcodec rawvideo {0}-s {1}x{2} -r {3} -pix_fmt {4} -i {5} {6} {7}",
                    overwrite ? "-y " : string.Empty,
                    image.Width,
                    image.Height,
                    FrameRate,
                    image.Channels == 1 ? "gray" : "bgr24",
                    pipe,
                    OutputArguments,
                    fileName);
                var ffmpeg = new StartProcess();
                ffmpeg.Arguments = args;
                ffmpeg.FileName = "ffmpeg.exe";
                return ffmpeg.Generate().IgnoreElements().Select(x => default(IplImage));
            }));
        });
    }
}
