using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Text;
using Xamarin.Forms;
using ModernHttpClient;

namespace Prototype.Services
{
    public class Group
    {
        public string Nom { get; set; }
        public int Annee { get; set; }
        public string Description { get; set; }
        public string Studio { get; set; }
        public string Type { get; set; }
        public List<string> TypeDeJeux { get; set; }
        public List<string> ListeDevelopper { get; set; }
        public string UrlDownload { get; set; }
        public string UrlImage { get; set; }
        public ImageSource Image
        {
            get
            {
                ImageSource res = null;
                using (var webClient = new HttpClient(new NativeMessageHandler()))
                {
                    var imageBytes = webClient.GetByteArrayAsync(this.UrlImage).Result;
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        res = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    }
                }

                return res;
            }
        }
        public string UrlMiniature { get; set; }
        public ImageSource Miniature
        {
            get
            {
                ImageSource res = null;
                using (var webClient = new HttpClient(new NativeMessageHandler()))
                {
                    var imageBytes = webClient.GetByteArrayAsync(this.UrlMiniature).Result;
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        res = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    }
                }

                return res;
            }
        }
        /*public string Miniature
        {
            get
            {
                WebClient web = new WebClient();
                web.DownloadDataCompleted += (s, e) => {
                    var bytes = e.Result; // get the downloaded data
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string localFilename = this.Studio +".png";
                    string localPath = Path.Combine(documentsPath, localFilename);
                    File.WriteAllBytes(localPath, bytes); // writes to local storage
                };
                web.DownloadDataAsync(new Uri(this.UrlMiniature));
                //byte[] ImageByte = web.DownloadDataTaskAsync(new Uri(this.UrlMiniature)).Result;
                //MemoryStream stream = new MemoryStream(ImageByte);
                //ImageSource image = ImageSource.FromUri(new Uri(this.UrlMiniature));
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), this.Studio + ".png");
            }
        }*/
        
        /*public void Download()
        {
            WebClient web = new WebClient();
            web.DownloadDataCompleted += (s, e) => {
                var bytes = e.Result; // get the downloaded data
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string localFilename = this.Studio + ".png";
                string localPath = Path.Combine(documentsPath, localFilename);
                File.WriteAllBytes(localPath, bytes); // writes to local storage
            };
            web.DownloadDataAsync(new Uri(this.UrlMiniature));
        }*/
    }
}
