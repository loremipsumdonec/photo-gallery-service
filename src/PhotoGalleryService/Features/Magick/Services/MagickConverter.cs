using PhotoGalleryService.Features.Magick.Instructions;
using System.Diagnostics;
using System.Text;

namespace PhotoGalleryService.Features.Magick.Services
{
    public sealed class MagickConverter
        : IDisposable, IConvertContext
    {
        private readonly IList<IInstruction> _instructions;

        private readonly string _out;
        private readonly string _imageMagickPath;
        private readonly string _workfolder;
        private readonly StringBuilder _output;
        private readonly StringBuilder _error;
        private readonly StringBuilder _builder;

        public MagickConverter(string imageMagickPath, string workfolder) 
        {
            _imageMagickPath = imageMagickPath; 
            
            _instructions = new List<IInstruction>();
            _output = new StringBuilder();
            _error = new StringBuilder();
            _builder = new StringBuilder();
            _workfolder = System.IO.Path.Combine(workfolder, Guid.NewGuid().ToString("N"));

            In = System.IO.Path.Combine(_workfolder, "_in");
            _out = System.IO.Path.Combine(_workfolder, "_out");
            Directory.CreateDirectory(_workfolder);

            Append("convert");
            Append(In);
        }

        public void Dispose()
        {
            Directory.Delete(_workfolder, true);
        }

        public string In { get; }

        public string Out
        {
            get 
            {
                return _out + "." + Format;
            }
        }

        public string Format { get; set; }

        public string Arguments
        {
            get
            {
                return _builder.ToString();
            }
        }

        public void Append(string argument)
        {
            _builder.Append(argument);
            _builder.Append(' ');
        }

        public void Add(IInstruction instruction) 
        {
            _instructions.Add(instruction);
        }

        public async Task<byte[]> ConvertAsync(byte[] stream) 
        {
            await File.WriteAllBytesAsync(In, stream);
            Format = "jpg";

            foreach(var instruction in _instructions) 
            {
                instruction.Apply(this);
            }

            if(!_instructions.Any(i=> i is IOutputInstruction)) 
            {
                Append(Out);
            }

            RunMagick();

            return await File.ReadAllBytesAsync(Out);
        }

        private void RunMagick() 
        {
            _output.Clear();
            _error.Clear();

            Process process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = _imageMagickPath;
            process.StartInfo.Arguments = Arguments;
            process.OutputDataReceived += OnOutputDataReceived;
            process.ErrorDataReceived += OnErrorDataReceived;

            bool status = process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            string error = _error.ToString();
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _error.Append(e.Data);
            }
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _output.AppendLine(e.Data);
        }
    }
}
