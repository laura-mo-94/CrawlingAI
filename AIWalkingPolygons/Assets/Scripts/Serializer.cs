using UnityEngine;
using System.Collections;

public class Serializer : MonoBehaviour {

	public void Serialize(List<MovementNode> sequence)
	{
		using (var stream = File.Create("seq.xml")) {
			var serializer = new XmlSerializer(typeof(List<MovementNode>));
			serializer.Serialize(stream, sequence);
		}
	}

	public List<MovementNode> Deserialize()
	{
		// IF DOESN'T WORK, change to the path of "seq.xml" on your computer
		string path = "seq.xml";
		List<MovementNode> sequence = new List<MovementNode>();
		XmlSerializer serializer = new XmlSerializer(typeof(List<MovementNode>));
		StreamReader reader = new StreamReader(path);

		sequence = (List<MovementNode>)serializer.Deserialize(reader);
		reader.Close();

		return sequence;
	}
}
