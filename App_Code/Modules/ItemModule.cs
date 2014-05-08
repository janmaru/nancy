using Nancy;
using Nancy.Cookies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Drawing;

/// <summary>
/// Summary description for HomeModule
/// </summary>
public class ItemModule : NancyModule
{
    public ItemModule(IRootPathProvider pathProvider)
    {
        User me = null; //add the user  as a property to the model :)


        Before += ctx =>
        {
            if (ctx.Request.Cookies.ContainsKey("flex"))
            {
                var myId = ctx.Request.Cookies["flex"];
                var id_user = new EncryptHelper(AppConfig.Provider,
                                                    Xmlconfig.get(
                                                      "cryptokey",
                                                      pathProvider.GetRootPath()).Value).decrypt(myId);

                if (!string.IsNullOrEmpty(id_user))
                {
                    me = UsersRepository.getById(Convert.ToInt32(id_user));
                    return null; //it means you can carry on!!!!
                }
            }

            var res = new Response();
            res.StatusCode = HttpStatusCode.Forbidden;
            return res;
        };

        Get["/items"] = _ =>
        {
            var model = new
            {
                title = "Son Quattro CMS",
                items = ItemsRepository.Items,
                me = me
            };
            return View["items", model];
        };

        Get[@"/items/(?<order>[0-3])"] = parameters =>
        {
            //*important
            byte order = parameters.order; //I'm forcing the right conversion
            var model = new
            {
                title = "Son Quattro CMS",
                item = ItemsRepository.getByOrder(order),
                me = me
            };
            return View["single_item", model];
        };

        Post["/items/(?<order>[0-3])"] = parameters =>
        {
            byte order = parameters.order;
            Item it = null;

            //check if you're only changing other then the pix
            if (Request.Form.title != null)
            {
                it = new Item
                {
                    Order = parameters.order,
                    Title = Request.Form["title"],
                    Description = Request.Form["description"],
                    Image = Request.Form["image"]
                };
            }
            else
            {
                int x1 = Request.Form.x1;
                int y1 = Request.Form.y1;
                int x2 = Request.Form.x2;
                int y2 = Request.Form.y2;
                int w = Request.Form.w;
                int h = Request.Form.h;

                string image_file = Guid.NewGuid() + ".png";
                string image_file_raw = Request.Form.b64_img;
                var image_b64 = image_file_raw.Replace("data:image/jpeg;base64,", "").Replace("data:image/png;base64,", "").Replace("data:image/gif;base64,", "");
                var rimg = ImageHelper.b64ToImage(image_b64);
                var ratio = Convert.ToDecimal(rimg.Width) / Convert.ToDecimal(528);
                var rect = ImageHelper.ratioRectangle(new Rectangle(x1, y1, w, h), ratio);
                var n_img = ImageHelper.cropImage(rimg, rect);

                //at the end resize 840x540
                var r_img = ImageHelper.resize(n_img, 840);
                r_img.Save(pathProvider.GetRootPath() + @"images\" + image_file);

                it = ItemsRepository.getByOrder(order);
                it.Image = image_file;
            }

            dynamic model = null;
            var result = ItemsRepository.insert(order, it);

            if (result != null)
            {
                model = new
                {
                    title = "Son Quattro CMS",
                    item = result,
                    success = true,
                    messages = new List<string> { "The item has been successfull modified" },
                    me = me
                };
            }
            else
            {
                model = new
                {
                    title = "Son Quattro Items CRUD",
                    item = it, //I'm going to return back the one given
                    success = false,
                    messages = new List<string> { "The item could not be modified" },
                    me = me
                };
            }
            return View["single_item", model];
        };
    }
}
