using ATRealize.Mobilution.ImageUploader.Presentation.WPF.MobilutionServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATRealize.Mobilution.ImageUploader.Presentation.WPF.ViewModels
{
    public class MainViewModel
    {
        public ImageCriteria Criteria { get; private set; }

        public ObservableCollection<Image> Images { get; private set; }

        public MainViewModel()
        {
            Criteria = new ImageCriteria();
            Images = new ObservableCollection<Image>();
        }

        public async Task<SearchResponse> SearchAsync()
        {
            Images.Clear();
            var client = new ImageServiceClient();
            var response = await client.SearchAsync(new SearchRequest(Criteria));
            response.SearchResult.ForEach(o => Images.Add(o));
            return response;
        }

        public async Task<CreateResponse> CreateAsync(string path)
        {
            var request = new ImageRequest
            {
                Content = File.ReadAllBytes(path),
                ContentType = "image/jpg",
                FileName = Path.GetFileName(path),
                ThumbnailOption = new ThumbnailOption
                {
                    Width = 300
                }
            };
            var client = new ImageServiceClient();
            var response = await client.CreateAsync(new CreateRequest(request));
            return response;
        }
    }
}
