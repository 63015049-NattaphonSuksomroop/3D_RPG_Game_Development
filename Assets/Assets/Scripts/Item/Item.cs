
using UnityEngine;

namespace Item {
	public class Item : ScriptableObject {
		[Header("Item Information")]
		[SerializeField]
		Sprite _itemIcon;
		public Sprite ItemIcon { get => _itemIcon;}

		[SerializeField]
		string _name;
		public string Name { get => _name; }

	}
}
