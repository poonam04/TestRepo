using MyApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;
using WebGrease.Css.Extensions;


namespace MyApplication.Controllers
{
    public class HomeController : Controller
    {
        const string subscriptionKey = "f8d649a9c02f4ac9a750bbcfd8902389";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze";
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase Image)
        {
            var extension = Path.GetExtension(Image.FileName);

            var response = await MakeAnalysisRequest(Image);


            //var parsedResponse = JsonPrettyPrint(response);
            var parsedResponse = FromJSON<WebApi.Models.RootObject>(response);
            var tags = parsedResponse.tags.Where(x => x.confidence > 0.90).Select(x => x.name).ToList();

           
            var rootData = JsonConvert.DeserializeObject<WebApi.Models.RootObject>(response);
            ImageUploadRequest uploadData = new ImageUploadRequest();
            uploadData.rootObject = rootData;
            uploadData.imageName = DateTime.Now.ToString("yyyyMMddHHmmss")+"."+ extension;
            uploadData.tag = tags;
           

            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            file.InputStream.Position = 0;
            int fileSizeInBytes = file.ContentLength;
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] data = target.ToArray();

            uploadData.imageByte = data;
            string serializedData = JsonConvert.SerializeObject(uploadData);
            HttpResponseMessage responseData = PostJsonRequest("http://localhost:6364/api/v1/admin/updateuserimage", serializedData, null);
            string resultprofile = responseData.Content.ReadAsStringAsync().Result;
            if (responseData.StatusCode == HttpStatusCode.OK)
            {
                return Json(tags, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
                // return Content(JsonPrettyPrint(response));
               // return Json(tags, JsonRequestBehavior.AllowGet);
            // return View();
            //postedFile.InputStream

        }

        private static HttpResponseMessage PostJsonRequest(string route, string contentBody, string token)
        {
            using (HttpClient client = new HttpClient())
            {
              
                if (!string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(contentBody.ToString(), Encoding.UTF8, "application/json");
                return client.PostAsync(route, content).Result;
            }
        }
        public async Task<string> MakeAnalysisRequest(HttpPostedFileBase Image)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            //string requestParameters = "visualFeatures=Categories,Description,Color&language=en";
            string requestParameters = "visualFeatures=Categories,Tags,Description,Color&language=en";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            MemoryStream stream = new MemoryStream();
            Image.InputStream.CopyTo(stream);


            byte[] byteData = stream.ToArray();

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                return contentString;

                // Display the JSON response.
                //Console.WriteLine("\nResponse:\n");
                //Console.WriteLine(JsonPrettyPrint(contentString));
            }
        }


        public async void MakeAnalysisRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            //string requestParameters = "visualFeatures=Categories,Description,Color&language=en";
            string requestParameters = "visualFeatures=Categories,Tags,Description,Color&language=en";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.

            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                Console.WriteLine("\nResponse:\n");
                Console.WriteLine(JsonPrettyPrint(contentString));
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }


        /// <summary>
        /// Formats the given JSON string by adding line breaks and indents.
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        public string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            string INDENT_STRING = "    ";
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < json.Length; i++)
            {
                var ch = json[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && json[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }


        private T FromJSON<T>(string input)
        {
            MemoryStream stream = new MemoryStream();

            try
            {
                DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(T));
                stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
                T obj = (T)jsSerializer.ReadObject(stream);

                return obj;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }
    }
}