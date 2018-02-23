using Repository.Concrete;
using Repository.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Http;
using WebApi.Models;
using WebGrease.Css.Extensions;

namespace WebApi.Controllers
{
    [RoutePrefix("api/v1/user")]
    public class UserController : ApiController
    {

        const string subscriptionKey = "f8d649a9c02f4ac9a750bbcfd8902389";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze";
        /// <summary>
        /// Image API
        /// </summary>
        /// <returns></returns>
        [Route("~/api/v1/user/imagedata")]
        public HttpResponseMessage GetImage()
        {
            IUserData _repository = new UserData();
            var imageList = _repository.GetImageData();
            if (imageList.Succeeded == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK, imageList.Data);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error occured");
            }


        }


        [Route("~/api/v1/user/imagedata")]
        public HttpResponseMessage GetImage(string requestText)
        {
            IUserData _repository = new UserData();
            var imageList = _repository.GetImageData();
            if (imageList.Succeeded == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK, imageList.Data);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error occured");
            }


        }

        [Route("~/api/v1/user/postuserimage")]
        [AllowAnonymous]
        public HttpResponseMessage PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {



                            var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName + extension);

                            postedFile.SaveAs(filePath);


                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }


        [Route("~/api/v1/admin/updateuserimage")]
        [HttpPost]
        public HttpResponseMessage UpdateImageRequest(ImageUploadRequest model)
        {
            IUserData _repository = new UserData();
            var result = _repository.UpdateImageData(model);
            if (result.Succeeded == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error occured");
            }
        }
    }

}
