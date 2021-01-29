using System;
using System.IO;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;

// This example code shows how you could implement the required main function for a 
// Console UWP Application. You can replace all the code inside Main with your own custom code.

// You should also change the Alias value in the AppExecutionAlias Extension in the 
// Package.appxmanifest to a value that you define. To edit this file manually, right-click
// it in Solution Explorer and select View Code, or open it with the XML Editor.

namespace ssml2audio
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {

            // The media object for controlling and playing audio.
            // Windows.UI.Xaml.Controls.MediaElement mediaElement = this.media;

            // The object for controlling the speech synthesis engine (voice).
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();

            // Generate the audio stream from plain text.
            using (SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Hello World"))
            {
                // Get the app's local folder.
                var path = new FileInfo(Environment.GetCommandLineArgs()[0]).DirectoryName;
                var user = (await Windows.System.User.FindAllAsync())[0];
                StorageFolder localFolder = await StorageFolder.GetFolderFromPathForUserAsync(user, path);

                // Send the stream to the audio file.
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);

                    var sampleFile = await localFolder.CreateFileAsync("sample.wav", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteBufferAsync(sampleFile, reader.ReadBuffer((uint)stream.Size));
                }
            }


            if (args.Length == 0)
            {
                Console.WriteLine("Hello - no args");
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine($"arg[{i}] = {args[i]}");
                }
            }
            Console.WriteLine("Press a key to continue: ");
            Console.ReadLine();
        }
    }
}
