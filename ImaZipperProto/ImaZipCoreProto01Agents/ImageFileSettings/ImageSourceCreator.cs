using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HalationGhost.WinApps.ImaZip.ImageFileSettings
{
	public class ImageSourceCreator
	{
		public void MergeToList(ObservableCollection<ImageSource> targetCol,
			List<string> sourcePaths,
			ImageSourceType sourceType)
		{
			sourcePaths
				.Where(p => targetCol.All(s => s.Path.Value != p))
				.ToList()
				.ForEach(p => targetCol.Add(new ImageSource(p, sourceType)));
		}

		public List<ImageSource> CreateToList(List<string> sourcePaths, ImageSourceType sourceType)
		{
			var retList = new List<ImageSource>();

			sourcePaths.ForEach(p => retList.Add(new ImageSource(p, sourceType)));

			return retList;
		}
	}
}
