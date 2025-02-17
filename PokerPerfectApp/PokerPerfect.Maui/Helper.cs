namespace PokerPerfect.Maui
{
	internal static class Helper
	{
		public static readonly List<string> Names = new();
//		public static bool IsEdit;
		public static int GameId;
		public static int ChipsetId;

    public static async Task DeleteEntitiesAsync<T>(IEnumerable<T> entities, Func<T, Task> deleteAction)
    {
      foreach (var entity in entities)
        await deleteAction(entity);
    }

    public static void MapProperties(object source, object destination)
    {
      var sourceType = source.GetType();
      var destinationType = destination.GetType();

      var sourceProperties = sourceType.GetProperties();
      var destinationProperties = destinationType.GetProperties();

      foreach (var sourceProperty in sourceProperties)
      {
        var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

        if (destinationProperty != null && destinationProperty.CanWrite)
        {
          var value = sourceProperty.GetValue(source);
          destinationProperty.SetValue(destination, value);
        }
      }
    }

    public static void Add(string name)
		{
			Names.Add(name);
		}
		public static void Clear()
		{
			Names.Clear();
		}

		public static async void ShowMessage(string message)
		{
			await Application.Current.MainPage.DisplayAlert("Note", message, "OK");
		}
	}
}
