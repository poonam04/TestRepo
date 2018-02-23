using System.Collections.Generic;

namespace WebApi.Models
{
  
        public class TopScoringIntent
        {
            public string intent { get; set; } 
            public double score { get; set; }
        }

        public class Entity
        {
            public string entity { get; set; }
            public string type { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }
            public double score { get; set; }
        }

        public class ImageDataProcessing
        {
            public string query { get; set; }
            public TopScoringIntent topScoringIntent { get; set; }
            public List<Entity> entities { get; set; }
        }

        public class DataReponse
        {
            public string query { get; set; }
           // public string imageUrl { get; set; }
            public List<ReposnseEntity> entities { get; set; }
        }

        public class ReposnseEntity
        {
            public string entity { get; set; }
            public string Image { get; set; }           
        }
   
}