using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ssml2audio
{
    class TextToSpeech
    {
        public static async Task SSML2AudioFile(string fileName) {
            // The object for controlling the speech synthesis engine (voice).
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();

            var path = new FileInfo(Environment.GetCommandLineArgs()[0]).DirectoryName;
            var SsmlText = File.ReadAllText(path + "/" + $"{fileName}.xml");
            // Generate the audio stream from the SSML text.
            using (SpeechSynthesisStream stream = await synth.SynthesizeSsmlToStreamAsync(SsmlText))
            {
                // Get the app's local folder.
                
                var user = (await Windows.System.User.FindAllAsync())[0];
                StorageFolder localFolder = await StorageFolder.GetFolderFromPathForUserAsync(user, path);

                // Send the stream to the audio file.
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);

                    var sampleFile = await localFolder.CreateFileAsync($"{fileName}.wav", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteBufferAsync(sampleFile, reader.ReadBuffer((uint)stream.Size));
                }
            }
        }
    }
}
