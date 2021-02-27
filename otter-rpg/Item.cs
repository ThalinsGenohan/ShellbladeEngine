namespace OtterRPG
{
	public class Item : BaseItem
	{
		public enum ItemCategory
		{
			Restorative,
		}

		public ItemCategory Category { get; set; }
	}
}
