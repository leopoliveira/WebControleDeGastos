using Newtonsoft.Json;

namespace ControleDeGastosV2.Models
{
    public enum Types
    {
        Info,
        Error
    }
    public class Message
    {
        public Types Type { get; set; }
        public string Text { get; set; }
        public Message(string text, Types messageType = Types.Info)
        {
            Type = messageType;
            Text = text;
        }

        public static string Serialize(string text, Types messageType = Types.Info)
        {
            Message message = new Message(text, messageType);
            return JsonConvert.SerializeObject(message);
        }

        public static Message Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<Message>(message);
        }
    }
}
