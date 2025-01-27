using Newtonsoft.Json;

public class Class
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonProperty("students")]
    public virtual ICollection<Student> Students { get; set; }
}