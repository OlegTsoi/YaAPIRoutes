using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace YaAPIRoutes.Models
{
    
    [Serializable]
    public class Courier 
    {

        public int Id { set; get; }
        public string Venicle_Id { get; set; }
        public string Routejson { get; set; }
        


        public List<string> RoutejsonToList()

        {
          
            List<string> Route = JsonSerializer.Deserialize<List<string>>(this.Routejson);
            return Route;
        }
    }
}
