using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YaAPIRoutes.Data;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace YaAPIRoutes.Models
{
    public static class GetDataFromApi
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            var id = "acfd99dd-14ca132a-2001a84c-9f29da24";
            var request = new GetRequest($"https://courier.yandex.ru/vrs/api/v1/result/mvrp/{id}");

            request.Run();

            var response = request.Response;
            var json = JObject.Parse(response);

            var result = json["result"];
            var routes = result["routes"];
            List<Courier> listroutes = new();               // список ИсполнителейМаршрута, содержащих наименования путевых точек маршрута
            List<Courier> listroutesfromdb = new();

            foreach (var route in routes)                   // бежим по маршрутам
            {
                var vehicle_id = route["vehicle_id"];
                var route_vehicle = route["route"];         // массив названий точек маршрута машины

                Courier car = new();
                car.Venicle_Id = (string)vehicle_id;

                List<string> Route = new();
                foreach (var i in route_vehicle)            // бежим по точкам маршрута route
                {

                    var node = i["node"];
                    var value = node["value"];
                    var point = value["point"];
                    var point_ref = value["ref"];
                    var title = value["title"];

                    string point_name = "";

                    if (point_ref != null) point_name = (string)point_ref;
                    if (point_name == "") point_name = (string)value["description"] + " " + (string)value["title"];

                    Route.Add(point_name);
                }

                car.Routejson = JsonSerializer.Serialize<List<string>>(Route);
                listroutes.Add(car); 
            }

             using (var context = new YaAPIRoutesContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<YaAPIRoutesContext>>()))
            {
                context.Courier.RemoveRange(context.Courier);
                context.SaveChanges();
            }        

            using (var context = new YaAPIRoutesContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<YaAPIRoutesContext>>()))
            {
                
                foreach (var i in listroutes) context.Courier.Add(i);
                       
                context.SaveChanges();
            }
        }
    }
}
