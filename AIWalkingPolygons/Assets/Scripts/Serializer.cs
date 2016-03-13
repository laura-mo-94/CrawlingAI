using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class Serializer {

	public void Serialize(List<MovementNode> sequence)
	{
		var serializer = new XmlSerializer (typeof(List<MovementNode>));
		var stream = new FileStream("seq.xml", FileMode.Create);
		serializer.Serialize(stream, this);
		stream.Close();
	}

	public List<MovementNode> Deserialize()
	{
		string path = "seq.xml";
		List<MovementNode> sequence = new List<MovementNode>();
		XmlSerializer serializer = new XmlSerializer(typeof(List<MovementNode>));
		var stream = new FileStream(path, FileMode.Open);

		var creature = serializer.Deserialize(stream) as Creature;
		sequence = creature.sequence;
		stream.Close();

		return sequence;
	}
}
