using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class Serializer {

	public void Serialize(Creature c)
	{
		var serializer = new XmlSerializer (typeof(Creature));
		var stream = new FileStream("seq.xml", FileMode.Create);
		//'this' may be a possible problem:
		serializer.Serialize(stream, this);
		stream.Close();
	}

	public List<MovementNode> Deserialize()
	{
		string path = "seq.xml";
		List<MovementNode> sequence = new List<MovementNode>();
		XmlSerializer serializer = new XmlSerializer(typeof(Creature));
		var stream = new FileStream(path, FileMode.Open);

		var creature = serializer.Deserialize(stream) as Creature;
		sequence = creature.sequence;
		stream.Close();

		return sequence;
	}
}
