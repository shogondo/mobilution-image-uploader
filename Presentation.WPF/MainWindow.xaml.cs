using System;
using System.Windows;
using Microsoft.Win32;
using System.Net.Http;
using ATRealize.Mobilution.ImageUploader.Presentation.WPF.Exchange;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ATRealize.Mobilution.ImageUploader.Presentation.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Upload(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = "*.jpg";
            if (dialog.ShowDialog() == true)
            {
                UploadSelectedFile(dialog.FileName);
            }
        }

        private async void UploadSelectedFile(string path)
        {
            using (var client = new HttpClient())
            {
                var content = new MultipartFormDataContent();

                var model = new ImageExchange { Title = Path.GetFileName(path) };
                addImageToContent(content, model);
                addFileToContent(content, path);
                try
                {
                    var message = await client.PostAsync("http://localhost:51256/api/images", content);

                    if (message.IsSuccessStatusCode)
                    {
                        MessageBox.Show("OK");
                    }
                    else
                    {
                        MessageBox.Show("NG");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void addImageToContent(MultipartFormDataContent content, ImageExchange model)
        {
            var part = new StringContent(serializeToJson(model));
            part.Headers.Add("Content-Disposition", "form-data; name=\"json\"");
            content.Add(part, "json");
        }

        private void addFileToContent(MultipartFormDataContent content, string path)
        {
            var fileName = Path.GetFileName(path);
            var stream = File.OpenRead(path);
            var part = new StreamContent(stream);
            part.Headers.Add("Content-Type", "application/octet-stream");
            part.Headers.Add("Content-Disposition", String.Format("form-data; name=\"file\"; filename=\"{0}\"", fileName));
            content.Add(part, "file", fileName);
        }

        private string serializeToJson(ImageExchange model)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(ImageExchange));
                serializer.WriteObject(stream, model);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
