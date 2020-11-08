using Newtonsoft.Json;

namespace nodeSys2
{
    public class GraphSerialization
    {
        public static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            TypeNameHandling = TypeNameHandling.Auto,
        };

        public static string GraphToJson(Graph graph)
        {


            return JsonConvert.SerializeObject(graph, settings);
        }

        public static Graph JsonToGraph(string json)
        {
            Graph g = JsonConvert.DeserializeObject<Graph>(json, settings);
            g.InitGraph();
            return g;
        }
    }
}
