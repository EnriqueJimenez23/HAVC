using System;
using System.Web;
using System.Web.Mvc;

namespace CsWeb.Infrastructure.Notification
{
	public static class HtmlHelperExtensions
	{
		public static HtmlString RenderMessages(this HtmlHelper htmlHelper)
		{
			var messages = String.Empty;
			foreach (var messageType in Enum.GetNames(typeof(MessageType)))
			{
				var message = htmlHelper.ViewContext.ViewData.ContainsKey(messageType) ? htmlHelper.ViewContext.ViewData[messageType] : htmlHelper.ViewContext.TempData.ContainsKey(messageType) ? htmlHelper.ViewContext.TempData[messageType] : null;
				if (message != null)
				{
				    var classNotification = string.Empty;
				    var icon = string.Empty;
				    switch (messageType)
				    {
                        case "Success":
				            classNotification = "alert-success"; icon = @"<span class=""glyphicon glyphicon-ok"" aria-hidden=""true""></span>  "; break;
                        case "Information":
				            classNotification = "alert-info"; break; 
                        case "Error":
				            classNotification = "alert-danger"; break;
                        case "Warning":
				            classNotification = "alert-warning"; break;

				    }

                    messages += string.Format(@"<div id=""message-box"" class=""alert {0}"" role=""alert"" style=""margin: 10px 0 10px 0;""><button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Close""><span aria-hidden=""true"">&times;</span></button>{1}{2}</div>", classNotification, icon, message);
				}
			}
			return MvcHtmlString.Create(messages);
		}
	}
}