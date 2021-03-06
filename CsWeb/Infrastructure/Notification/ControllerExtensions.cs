using System.Web.Mvc;

namespace CsWeb.Infrastructure.Notification
{
	public static class ControllerExtensions
	{
		public static void ShowMessage(this Controller controller, MessageType messageType, string message, bool showAfterRedirect = true)
		{
			var messageTypeKey = messageType.ToString();
			if (showAfterRedirect)
			{
				controller.TempData[messageTypeKey] = message;
			}
			else
			{
				controller.ViewData[messageTypeKey] = message;
			}
		}
	}
}