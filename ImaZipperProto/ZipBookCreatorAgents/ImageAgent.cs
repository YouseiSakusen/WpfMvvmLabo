using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HalationGhost.WinApps.ImaZip.ZipBookCreator
{
	internal class ImageAgent
	{
		//internal async Task<ImageSpecification> GetImageSpecificationAsync(string imageFilePath)
		//{
		//	if (!File.Exists(imageFilePath))
		//		return null;

		//	return await Task.Run(() =>
		//	{
		//		using (var reader = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
		//		{
		//			using (var img = Image.FromStream(reader))
		//			{
		//				return new ImageSpecification(img) { ImageFilePath = imageFilePath };
		//			}
		//		}
		//	});
		//}

		//internal ImageSpecification GetImageSpecification(string imageFilePath)
		//{
		//	if (!File.Exists(imageFilePath))
		//		return null;

		//	using (var reader = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
		//	{
		//		using (var img = Image.FromStream(reader))
		//		{
		//			return new ImageSpecification(img, imageFilePath);
		//		}
		//	}
		//}
	}
}
