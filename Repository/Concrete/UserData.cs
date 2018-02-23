using Repository.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.DTO;
using WebApi.Models;
using Repository.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using WebApi.Helper;

namespace Repository.Concrete
{
    public class UserData : IUserData
    {
        public APIResponse<List<DataReponse>> GetImageData()
        {
           
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<DataReponse> listResponse = new List<DataReponse>();

                //byte[] image = db.ImageData.Where(t => t.Id == 1).FirstOrDefault().Image;
               
                DataReponse data = new DataReponse();
                data.query = "want to jump on a trampoline";
                data.entities = new List<ReposnseEntity> {
                    new ReposnseEntity(){ entity = "play", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQjoRHDfi-RwXDLxSjdM3vPkmzwxbjDJjChnkpclMTOwTfQ3kJs" },
                     new ReposnseEntity(){ entity = "trampoline", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQiogpKXz6aPtiELlQlU_okRXGrM-yO2bK4X4Cvho6FbVHrs4KHVw" }
                };
                listResponse.Add(data);
                if (listResponse.Any())
                {
                    return new APIResponse<List<DataReponse>>() { Data = listResponse, HttpCode = System.Net.HttpStatusCode.OK, Succeeded = true };
                }
                else
                {
                    return new APIResponse<List<DataReponse>>() { HttpCode = System.Net.HttpStatusCode.NoContent, Succeeded = false };
                }
            }
        }

        

        public APIResponse<string> UpdateImageData(ImageUploadRequest model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    string query = string.Empty;
                    using (HttpClient client = new HttpClient())
                    {                 
                        string url = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/8e983fbd-0438-4bd2-9161-8c8cbf81f82e?subscription-key=4e80642610f8481b914a81a0920c8073&verbose=true&timezoneOffset=0&q=want%20to%20jump%20on%20a%20trampoline";
                        HttpResponseMessage responseData = client.GetAsync(url).Result;
                        string resultprofile = responseData.Content.ReadAsStringAsync().Result;
                        ImageDataProcessing dataQuery = JsonConvert.DeserializeObject<ImageDataProcessing>(resultprofile);
                        query = dataQuery.query;
                    }


                    var entity = new ImageDataEntity {
                        ImageName = model.imageName,
                        Query = query,
                        DataResponse = JsonConvert.SerializeObject(model.rootObject),
                        Image = model.imageByte,
                        DateAdded = DateTime.Now,
                        IsLive = true
                       
                    };
                   

                    db.ImageData.Add(entity);
                    db.SaveChanges();

                   
                    foreach (var q in model.tag)
                    {
                        db.ImageTag.Add(new ImageTagEntity()
                        {
                            ImageId= entity.Id,
                            Name = q,
                            DateAdded = DateTime.Now,
                            IsLive = true
                        });
                       
                   }

                    db.SaveChanges();

                    return new APIResponse<string>
                    {
                        Data = "Success",
                        Succeeded = true
                    };
                }
                catch (Exception ex)
                {
                    return new APIResponse<string>
                    {
                        Data = ex.Message,
                        Succeeded = false
                    };
                }
            }
        }
    }
}

